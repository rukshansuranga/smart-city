"use client";
import { Controller, FieldValues, useForm } from "react-hook-form";
import * as z from "zod";
import Input from "../components/forms/Input";
//import TextArea from "../components/forms/TextArea";

import { Button, Label, Spinner } from "flowbite-react";
import { zodResolver } from "@hookform/resolvers/zod";
//import SelectField from "../components/forms/Select";
import TextArea from "../components/forms/TextArea";
import SelectField from "../components/forms/Select";
import DateField from "../components/forms/DateField";
import { useEffect, useRef, useState } from "react";
import {
  createProject,
  getProject,
  updateProject,
} from "../api/client/projectActions";
import { getTendersByProjectIdId } from "../api/client/tenderActions";
import { typeList, statusList } from "../../utility/Constants"; // Importing the constants
import { useRouter } from "next/navigation";
import AutoCompleteMap from "../components/map/autocomplete";
import { useCouncilStore } from "@/store";
import { ProjectType, ProjectStatus } from "@/enums";
import { Project, Tender } from "@/types";
import { toast } from "react-hot-toast";

const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
const ACCEPTED_FILE_TYPES = [
  "application/pdf",
  "application/msword",
  "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
];

const schema = z.object({
  subject: z.string().trim().min(1, "Project name is required"),
  description: z.string().optional(),
  city: z.string().trim().min(1, "City is required"),
  type: z.coerce.number().refine((val) => val in ProjectType, {
    message: "Invalid project type",
  }),
  specificationDocument: z
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
  estimatedCost: z.number().optional(),
  startDate: z.date().optional().nullable(),
  endDate: z.date().optional().nullable(),
  tenderOpeningDate: z.date().optional().nullable(),
  tenderClosingDate: z.date().optional().nullable(),
  latitude: z.number().optional(),
  longitude: z.number().optional(),
  status: z.coerce.number().refine((val) => val in ProjectStatus, {
    message: "Invalid project status",
  }),
  location: z.string().optional(),
  locationNote: z.string().optional().nullable(),
  awardedTenderId: z.coerce.number().optional(),
});

export default function ProjectForm({
  projectId,
  initialData,
  onSuccess,
}: {
  projectId?: string;
  initialData?: Project | null;
  onSuccess?: () => void;
}) {
  const fileUpload = useRef<HTMLInputElement>(null);
  const router = useRouter();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const { location: storeLocation } = useCouncilStore();
  const [city, setCity] = useState<{ lat: number; lng: number }>(storeLocation);
  const [tenders, setTenders] = useState<Tender[]>([]);
  const [showAwardTender, setShowAwardTender] = useState<boolean>(false);

  useEffect(() => {
    setCity(storeLocation);
  }, [storeLocation]);

  const { control, handleSubmit, reset, setValue, getValues } =
    useForm<Project>({
      resolver: zodResolver(schema),
      mode: "onChange",
      defaultValues: {
        subject: "",
        type: ProjectType.Road,
        tenderOpeningDate: new Date(),
        tenderClosingDate: null,
        startDate: new Date(),
        endDate: null,
        status: ProjectStatus.New,
        specificationDocument: "Please Upload File",
        city: "Mahara",
        latitude: 12.25,
        longitude: 77.75,
        estimatedCost: 1000000,
        location: "",
        description: "",
        locationNote: "",
      },
    });

  // Set initial data if provided, otherwise fetch project data if editing
  useEffect(() => {
    if (initialData) {
      // Use provided initial data
      reset({
        ...initialData,
        tenderOpeningDate: initialData.tenderOpeningDate
          ? new Date(initialData.tenderOpeningDate)
          : undefined,
        tenderClosingDate: initialData.tenderClosingDate
          ? new Date(initialData.tenderClosingDate)
          : undefined,
        startDate: initialData.startDate
          ? new Date(initialData.startDate)
          : undefined,
        endDate: initialData.endDate
          ? new Date(initialData.endDate)
          : undefined,
      });

      // Check if tender closing date has passed and fetch tenders
      if (initialData.tenderClosingDate) {
        const closingDate = new Date(initialData.tenderClosingDate);
        const today = new Date();
        const isClosingDatePassed = closingDate < today;
        setShowAwardTender(isClosingDatePassed);

        if (isClosingDatePassed) {
          // Fetch tenders for this project
          getTendersByProjectIdId(initialData.id?.toString() || "").then(
            (result) => {
              if (result.isSuccess && result.data) {
                setTenders(result.data);
              }
            }
          );
        }
      }

      setIsLoading(false);
    } else if (projectId) {
      // Fetch data if no initial data provided
      setIsLoading(true);
      getProject(projectId)
        .then((response) => {
          if (!response.isSuccess) {
            toast.error(response.message || "Failed to fetch project data");
            return;
          }

          const project = response.data;

          reset({
            ...project,
            tenderOpeningDate: project.tenderOpeningDate
              ? new Date(project.tenderOpeningDate)
              : undefined,
            tenderClosingDate: project.tenderClosingDate
              ? new Date(project.tenderClosingDate)
              : undefined,
            startDate: project.startDate
              ? new Date(project.startDate)
              : undefined,
            endDate: project.endDate ? new Date(project.endDate) : undefined,
          });

          // Check if tender closing date has passed and fetch tenders
          if (project.tenderClosingDate) {
            const closingDate = new Date(project.tenderClosingDate);
            const today = new Date();
            const isClosingDatePassed = closingDate < today;
            setShowAwardTender(isClosingDatePassed);

            if (isClosingDatePassed) {
              // Fetch tenders for this project
              getTendersByProjectIdId(projectId).then((result) => {
                if (result.isSuccess && result.data) {
                  setTenders(result.data);
                }
              });
            }
          }
        })
        .finally(() => {
          setIsLoading(false);
        });
    }
  }, [projectId, initialData, reset]);

  const onSubmit = async (data: FieldValues) => {
    setIsLoading(true);

    try {
      const form = new FormData();
      form.append("subject", data.subject);
      form.append("description", data.description);
      form.append("estimatedCost", data.estimatedCost); //new Date().toISOString().split('T')[0]
      form.append(
        "startDate",
        data.startDate
          ? new Date(data.startDate).toISOString().split("T")[0]
          : ""
      );
      form.append(
        "endDate",
        data.endDate ? new Date(data.endDate).toISOString().split("T")[0] : ""
      );
      form.append(
        "tenderOpeningDate",
        data.tenderOpeningDate
          ? new Date(data.tenderOpeningDate).toISOString().split("T")[0]
          : ""
      );
      form.append(
        "tenderClosingDate",
        data.tenderClosingDate
          ? new Date(data.tenderClosingDate).toISOString().split("T")[0]
          : ""
      );
      form.append("city", data.city);
      form.append("type", String(data.type));
      form.append("status", String(data.status));
      form.append("latitude", data.latitude);
      form.append("longitude", data.longitude);
      form.append("specificationDocument", data.specificationDocument);
      form.append("location", data.location);
      form.append("locationNote", data.locationNote);

      // Add awardedTenderId if it exists
      if (data.awardedTenderId) {
        form.append("awardedTenderId", data.awardedTenderId);
      }

      if (projectId) {
        await updateProject(projectId, form);
        // Call onSuccess callback if provided, otherwise navigate
        if (onSuccess) {
          onSuccess();
        } else {
          router.push("/project");
        }
      } else {
        //await createProject(projectData);

        try {
          await createProject(form);
          router.push("/project");
        } catch (error) {
          console.error("Failed to upload file:", error);
          setIsLoading(false);
          return;
        }
      }
    } catch (error) {
      console.error("Error creating project:", error);
    } finally {
      setIsLoading(false);
      // Only navigate if no onSuccess callback for updates
      if (!projectId || !onSuccess) {
        // Navigation is handled above for specific cases
      }
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  async function handleSelectPlace(lat: number, lng: number, city: string) {
    setCity({
      lat,
      lng,
    });

    setValue("city", city);
    setValue("latitude", lat);
    setValue("longitude", lng);
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-col jus gap-4 mx-10">
        <div>
          <Input
            className="w-full"
            showlabel
            label="Project Name"
            type="text"
            placeholder="Enter Project Name"
            name="subject"
            control={control}
          />
        </div>
        <div>
          <TextArea
            showlabel
            label="Description"
            rows={3}
            placeholder="Enter Description"
            name="description"
            control={control}
          />
        </div>
        <div className="flex gap-3">
          <div className="w-1/4">
            <SelectField
              showlabel
              label="Type"
              name="type"
              options={typeList}
              control={control}
            />
          </div>
          <div className="w-1/4">
            {projectId ? (
              <SelectField
                showlabel
                label="Status"
                placeholder="Enter Status"
                name="status"
                options={statusList}
                control={control}
              />
            ) : (
              <Input
                className="w-full"
                showlabel
                label="Status"
                type="text"
                value={ProjectStatus[getValues("status")]}
                name="status"
                control={control}
              />
            )}
          </div>
          <div className="w-1/4">
            <Input
              className="w-full"
              showlabel
              label="Estimated Cost"
              type="number"
              placeholder="Estimated Location"
              name="estimatedCost"
              control={control}
            />
          </div>
          <div className="w-1/4 self-end mb-2">
            <Controller
              name="specificationDocument"
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
                  <div className="flex justify-end gap-2 items-center w-full border border-gray-300 rounded-md">
                    <input
                      className=" w-2"
                      type="file"
                      ref={fileUpload}
                      onChange={handleFileChange}
                      style={{ opacity: "0" }}
                    />
                    <Label>
                      {value instanceof File
                        ? value.name
                        : typeof value === "string"
                        ? value
                        : "No file selected"}
                    </Label>
                    <Button
                      type="button"
                      onClick={() => fileUpload.current?.click()}
                    >
                      Upload
                    </Button>
                  </div>
                );
              }}
            />
          </div>
        </div>
        <div className="flex gap-3">
          <div className="flex w-1/12 items-end mb-2">
            <AutoCompleteMap
              handleSelectPlace={handleSelectPlace}
              position={{ lat: city?.lat, lng: city?.lng }}
            />
          </div>
          <div className="flex w-11/12 gap-2">
            <div className="w-1/3">
              <Input
                className="w-full"
                showlabel
                label="City"
                type="text"
                placeholder="Select City"
                name="city"
                control={control}
              />
            </div>
            <div className="w-1/3">
              <Input
                className="w-full"
                showlabel
                label="Latitude"
                type="text"
                placeholder="Select Latitude"
                name="latitude"
                control={control}
              />
            </div>
            <div className="w-1/3">
              <Input
                className="w-full"
                showlabel
                label="Longitude"
                type="text"
                placeholder="Select Logngitude"
                name="longitude"
                control={control}
              />
            </div>
          </div>
        </div>

        <div className="flex gap-3">
          <div className="w-1/3">
            <Input
              className="w-full"
              showlabel
              label="Address"
              type="text"
              placeholder="Address"
              name="location"
              control={control}
            />
          </div>
          <div className="w-2/3">
            <Input
              className="w-full"
              showlabel
              label="Location Note"
              type="text"
              placeholder="Location Note"
              name="locationNote"
              control={control}
            />
          </div>
        </div>

        <div className="flex gap-3">
          <div className="w-1/4">
            <DateField
              className="w-full"
              showlabel
              label="Start Date"
              name="startDate"
              control={control}
            />
          </div>
          <div className="w-1/4">
            <DateField
              className="w-full"
              showlabel
              label="End Date"
              name="endDate"
              control={control}
            />
          </div>
          <div className="w-1/4">
            <DateField
              className="w-full"
              showlabel
              label="Tender Opening Date"
              name="tenderOpeningDate"
              control={control}
            />
          </div>
          <div className="w-1/4">
            <DateField
              className="w-full"
              showlabel
              label="Tender Closing Date"
              name="tenderClosingDate"
              control={control}
            />
          </div>
        </div>

        {/* Award Tender Section - Only visible after tender closing date */}
        {showAwardTender && tenders.length > 0 && (
          <div className="border-t pt-4 mt-4">
            <div className="mb-4">
              <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                Award Tender
              </h3>
              <p className="text-sm text-gray-600 dark:text-gray-400">
                Select a tender to award from the submitted tenders for this
                project.
              </p>
            </div>
            <div className="w-1/2">
              <SelectField
                showlabel
                label="Select Tender to Award"
                placeholder="Choose a tender..."
                name="awardedTenderId"
                options={tenders.map((tender) => ({
                  text: `${tender.contractor?.name || "Unknown"} - ${
                    tender.bidAmount?.toLocaleString() || "N/A"
                  } `,
                  value: tender.tenderId?.toString() || "",
                }))}
                control={control}
              />
            </div>
          </div>
        )}

        <div>
          <Button type="submit">
            {projectId ? "Update Project" : "Create Project"}
          </Button>
        </div>
      </div>
    </form>
  );
}
