import * as z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Tender, Option } from "@/types";
import Input from "../components/forms/Input";
import TextArea from "../components/forms/TextArea";
import SelectField from "../components/forms/Select";
import { useEffect, useState } from "react";
import { getAllProjects } from "../api/actions/projectActions";
import { getCompanies } from "../api/actions/commonActions";
import { Button, Spinner } from "flowbite-react";

import {
  getTendersId,
  postTender,
  updateTender,
} from "../api/actions/tenderActions";
import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";

const schema = z.object({
  name: z.string().min(1, "Project name is required"),
  note: z.string().optional(),
  projectId: z.string().min(1, "Project is required"),
  companyId: z.string().min(1, "Company is required"),
  bidAmount: z.number().min(0, "Bid amount must be a positive number"),
  //submittedDate: z.date().optional().nullable(),
});

export default function TenderForm({
  tenderId,
  selectedProjectId,
}: {
  tenderId?: string;
  selectedProjectId?: string;
}) {
  const [projects, setProjects] = useState<Option[]>([]);
  const [companies, setCompanies] = useState<Option[]>([]);
  const [loading, setLoading] = useState(false);

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
      name: "test tender",
      note: "notee",
      bidAmount: 1000000,
    }, // or null or undefined
  });

  const onSubmit = async (data: Tender) => {
    // Handle form submission logic here
    console.log("Form Data:", data, selectedProjectId);

    if (tenderId) {
      // Update existing project
      await updateTender(tenderId, {
        ...data,
        projectId: selectedProjectId ?? "",
      });
    } else {
      // Create new project
      await postTender({ ...data, projectId: selectedProjectId ?? "" });
    }

    router.push("/tender/list/" + selectedProjectId);
  };

  console.log("form errors:", errors);

  useEffect(() => {
    // Fetch projects and companies when the component mounts

    setLoading(true);
    const projectListPromise = getAllProjects();
    const companyListPromise = getCompanies();
    const tenderPromise = getTendersId(tenderId!);

    Promise.all([projectListPromise, companyListPromise, tenderPromise])
      .then(([projectData, companyData, tenderData]) => {
        setProjects([
          { value: "", text: "Select Project" },
          ...projectData.map((project) => ({
            value: project?.id?.toString() ?? "",
            text: project.name,
          })),
        ]);

        setCompanies([
          { value: "", text: "Select Company" },
          ...companyData.map((company) => ({
            value: company.id.toString(),
            text: company.name,
          })),
        ]);

        if (tenderId) {
          reset({
            ...tenderData,
            projectId: tenderData.projectId.toString(),
            companyId: tenderData.companyId.toString(),
          });
        }
      })
      .catch((error) => {
        console.error("Error fetching projects or companies:", error);
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
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  return (
    <div>
      <h1>Tender Form</h1>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="flex flex-col jus gap-4 mx-10">
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
              name="name"
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
          <div>
            <div>
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
            <div>
              <SelectField
                showlabel
                label="Company"
                placeholder="Select Company"
                name="companyId"
                options={companies}
                control={control}
              />
            </div>
          </div>
          <div className="flex justify-end mt-4">
            <Button type="submit">
              {tenderId ? "Update Tender" : "Create Tender"}
            </Button>
          </div>
        </div>
      </form>
    </div>
  );
}
