import * as z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Tender, Option } from "@/types";
import Input from "../forms/Input";
import TextArea from "../forms/TextArea";
import SelectField from "../forms/Select";
import DateField from "../forms/DateField";
import { useEffect, useRef, useState } from "react";
import { getAllProjects } from "@/app/api/client/projectActions";
import { getCompanies } from "@/app/api/client/commonActions";
import { Button, Label, Spinner } from "flowbite-react";
import { formatNodaDateTime } from "@/lib/dateUtils";
import toast from "react-hot-toast";

import {
  getTendersId,
  postTender,
  updateTender,
} from "@/app/api/client/tenderActions";
import { useRouter } from "next/navigation";
import { Controller, FieldValues, useForm } from "react-hook-form";

const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
const ACCEPTED_FILE_TYPES = [
  "application/pdf",
  "application/msword",
  "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
];

const schema = z.object({
  subject: z.string().min(1, "Project name is required"),
  note: z.string().optional(),
  projectId: z.string().min(1, "Project is required"),
  contractorId: z.string().min(1, "Company is required"),
  bidAmount: z.number().min(0, "Bid amount must be a positive number"),
  submittedDate: z.date({ required_error: "Submitted date is required" }),
  tenderDocument: z
    .custom<File | string | null>(
      (val) => {
        if (!val) return true;
        if (typeof val === "string") return true;
        if (!(val instanceof File)) return false;
        if (val.size > MAX_FILE_SIZE) return false;
        if (!ACCEPTED_FILE_TYPES.includes(val.type)) return false;
        return true;
      },
      {
        message: "Please upload a PDF or Word document less than 5MB",
      }
    )
    .optional(),
});

export default function TenderForm({
  tenderId,
  selectedProjectId,
  onClose,
  onSuccess,
}: {
  tenderId?: string;
  selectedProjectId?: string;
  onClose?: () => void;
  onSuccess?: () => void;
}) {
  const [projects, setProjects] = useState<Option[]>([]);
  const [contractors, setContractors] = useState<Option[]>([]);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const fileUpload = useRef<HTMLInputElement>(null);

  const router = useRouter();

  const {
    control,
    handleSubmit,
    reset,
    setValue,
    formState: { errors },
  } = useForm<Tender>({
    resolver: zodResolver(schema),
    mode: "onChange",
    defaultValues: {
      subject: "",
      note: "",
      bidAmount: 0,
      submittedDate: new Date(),
    },
  });

  const onSubmit = async (data: FieldValues) => {
    // Handle form submission logic here
    console.log("Form Data:", data, selectedProjectId);

    // Validate that selectedProjectId exists
    if (!selectedProjectId) {
      toast.error("No project selected");
      return;
    }

    setSubmitting(true);

    try {
      const form = new FormData();
      form.append("subject", data.subject);
      form.append("note", data.note || "");
      form.append("projectId", selectedProjectId);
      form.append("contractorId", data.contractorId);
      form.append("bidAmount", data.bidAmount.toString());
      // Format date for NodaTime LocalDateTime backend
      form.append("submittedDate", formatNodaDateTime(data.submittedDate));
      if (data.tenderDocument) {
        form.append("tenderDocument", data.tenderDocument);
      }

      let result;
      if (tenderId) {
        // Update existing tender
        result = await updateTender(tenderId, form);
      } else {
        // Create new tender
        result = await postTender(form);
      }

      // Handle the new API response structure
      if (result.isSuccess) {
        toast.success(
          result.message ||
            (tenderId
              ? "Tender updated successfully!"
              : "Tender created successfully!")
        );

        // Call success callback if provided, otherwise navigate
        if (onSuccess) {
          onSuccess();
        } else {
          router.push("/tender/list/" + selectedProjectId);
        }

        // Close modal if callback provided
        if (onClose) {
          onClose();
        }
      } else {
        // Show error messages
        if (result.errors && result.errors.length > 0) {
          result.errors.forEach((error: string) => {
            toast.error(error);
          });
        } else {
          toast.error(result.message || "Failed to save tender");
        }
      }
    } catch (error) {
      console.error("Error submitting tender:", error);
      toast.error("An unexpected error occurred");
    } finally {
      setSubmitting(false);
    }
  };

  console.log("form errors:", errors);

  useEffect(() => {
    // Fetch projects and contractors when the component mounts
    setLoading(true);
    const projectListPromise = getAllProjects();
    const companyListPromise = getCompanies();
    const tenderPromise = tenderId
      ? getTendersId(tenderId)
      : Promise.resolve(null);

    Promise.all([projectListPromise, companyListPromise, tenderPromise])
      .then(([projectData, companyData, tenderResponse]) => {
        // Handle projects data
        if (projectData && Array.isArray(projectData)) {
          // If projectData is directly an array (old format)
          setProjects([
            { value: "", text: "Select Project" },
            ...projectData.map((project) => ({
              value: project?.id?.toString() ?? "",
              text: project.subject,
            })),
          ]);
        } else if (projectData?.isSuccess && projectData?.data) {
          // If projectData is ApiResponse format
          setProjects([
            { value: "", text: "Select Project" },
            ...projectData.data.map((project) => ({
              value: project?.id?.toString() ?? "",
              text: project.subject,
            })),
          ]);
        }

        // Handle company data
        if (companyData?.isSuccess && companyData?.data) {
          setContractors([
            { value: "", text: "Select Contractor" },
            ...companyData.data.map((contractor) => ({
              value: contractor?.contractorId?.toString(),
              text: contractor.name,
            })),
          ]);
        } else if (companyData && Array.isArray(companyData)) {
          // Fallback for old format
          setContractors([
            { value: "", text: "Select Contractor" },
            ...companyData.map((contractor) => ({
              value: contractor?.contractorId?.toString(),
              text: contractor.name,
            })),
          ]);
        }

        if (tenderId && tenderResponse) {
          if (tenderResponse.isSuccess && tenderResponse.data) {
            const tenderData = tenderResponse.data;
            reset({
              ...tenderData,
              projectId: tenderData.projectId.toString(),
              contractorId: tenderData.contractorId.toString(),
              submittedDate: new Date(tenderData.submittedDate),
            });
          } else {
            toast.error(tenderResponse.message || "Failed to load tender data");
          }
        }
      })
      .catch((error) => {
        console.error("Error fetching projects or contractors:", error);
        toast.error("Failed to load form data");
      })
      .finally(() => {
        setLoading(false);
      });
  }, [tenderId, reset]);

  useEffect(() => {
    if (selectedProjectId) {
      setValue("projectId", selectedProjectId);
    }
  }, [selectedProjectId, setValue]);

  if (loading) {
    return (
      <div className="flex justify-center items-center py-8">
        <Spinner aria-label="Loading form data" size="lg" />
      </div>
    );
  }

  console.log("projects:", projects);

  return (
    <div>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="flex flex-col gap-4">
          <div>
            <SelectField
              disabled={true}
              showlabel
              label="Project"
              placeholder="Select Project"
              name="projectId"
              options={projects}
              control={control}
            />
          </div>
          <div>
            <Input
              className="w-full"
              showlabel
              label="Tender Name"
              type="text"
              placeholder="Enter Tender Name"
              name="subject"
              control={control}
            />
          </div>
          <div>
            <TextArea
              showlabel
              label="Note"
              rows={3}
              placeholder="Enter Note"
              name="note"
              control={control}
            />
          </div>
          <div className="flex gap-4">
            <div className="w-1/2">
              <DateField
                showlabel
                label="Submitted Date"
                placeholder="Select Submitted Date"
                name="submittedDate"
                control={control}
                required={true}
              />
            </div>
            <div className="w-1/2">
              <Input
                className="w-full"
                showlabel
                label="Bid Amount"
                type="number"
                placeholder="Enter Bid Amount"
                name="bidAmount"
                control={control}
              />
            </div>
          </div>

          <div className="flex gap-4">
            <div className="w-1/2">
              <SelectField
                showlabel
                label="Contractor"
                placeholder="Select Contractor"
                name="contractorId"
                options={contractors}
                control={control}
              />
            </div>
            <div className="w-1/2">
              <Label htmlFor="tenderDocument" className="mb-2 block">
                Tender Document
              </Label>
              <Controller
                name="tenderDocument"
                control={control}
                render={({ field: { onChange, value } }) => {
                  function handleFileChange(
                    event: React.ChangeEvent<HTMLInputElement>
                  ) {
                    const file = event.target.files && event.target.files[0];
                    if (file) {
                      // Pass the actual File object to the form
                      onChange(file);
                    } else {
                      onChange(null);
                    }
                  }

                  return (
                    <div className="flex gap-2 items-center w-full border border-gray-300 rounded-lg p-3 dark:border-gray-600 dark:bg-gray-700">
                      <input
                        type="file"
                        ref={fileUpload}
                        onChange={handleFileChange}
                        className="hidden"
                        accept=".pdf,.doc,.docx"
                      />
                      <div className="flex-1 text-sm text-gray-600 dark:text-gray-400">
                        {value instanceof File
                          ? value.name
                          : typeof value === "string"
                          ? value
                          : "No file selected"}
                      </div>
                      <Button
                        type="button"
                        size="sm"
                        onClick={() => fileUpload.current?.click()}
                      >
                        Choose File
                      </Button>
                    </div>
                  );
                }}
              />
            </div>
          </div>
        </div>

        <div className="flex justify-end gap-3 mt-6 pt-4 border-t border-gray-200 dark:border-gray-600">
          {onClose && (
            <Button color="gray" onClick={onClose} disabled={submitting}>
              Cancel
            </Button>
          )}
          <Button type="submit" disabled={submitting}>
            {submitting ? (
              <>
                <Spinner aria-label="Submitting" size="sm" className="mr-2" />
                {tenderId ? "Updating..." : "Creating..."}
              </>
            ) : (
              <>{tenderId ? "Update Tender" : "Create Tender"}</>
            )}
          </Button>
        </div>
      </form>
    </div>
  );
}
