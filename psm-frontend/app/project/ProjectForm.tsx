"use client";
import { Controller, useForm } from "react-hook-form";
import * as z from "zod";
import Input from "../components/forms/Input";
//import TextArea from "../components/forms/TextArea";

import { Button, Label, Spinner } from "flowbite-react";
import { zodResolver } from "@hookform/resolvers/zod";
//import SelectField from "../components/forms/Select";
import { Project } from "@/types";
import TextArea from "../components/forms/TextArea";
import SelectField from "../components/forms/Select";
import DateField from "../components/forms/DateField";
import { useEffect, useRef, useState } from "react";
import {
  postProject,
  getProjectById,
  updateProject,
} from "../api/actions/projectActions";
import { typeList, statusList } from "../../utility/Constants"; // Importing the constants
import { useRouter } from "next/navigation";

const schema = z.object({
  name: z.string().trim().min(1, "Project name is required"),
  description: z.string().optional(),
  location: z.string().optional(),
  type: z.string().min(1, "Type is required"),
  specificationDocument: z.string().optional(),
  estimatedCost: z.number().optional(),
  tenderOpeningDate: z.date().optional().nullable(),
  tenderClosingDate: z.date().optional().nullable(),
  latitude: z.number().optional(),
  longitude: z.number().optional(),
  status: z.string().min(1, "Status is required"),
});

export default function ProjectForm({ projectId }: { projectId?: string }) {
  const fileUpload = useRef(null);
  const router = useRouter();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const {
    control,
    handleSubmit,
    reset,
    // formState: { errors },
  } = useForm<Project>({
    resolver: zodResolver(schema),
    mode: "onChange",
    defaultValues: {
      name: "test project",
      tenderOpeningDate: null,
      tenderClosingDate: null,
      status: "1",
      specificationDocument: "test.pdf",
      latitude: 12.25,
      longitude: 77.75,
      estimatedCost: 1000000,
    },
  });

  // Fetch project data if editing
  useEffect(() => {
    if (projectId) {
      setIsLoading(true);
      getProjectById(projectId)
        .then((project) => {
          console.log("Fetched project:", project);
          reset({
            ...project,
            tenderOpeningDate: project.tenderOpeningDate
              ? new Date(project.tenderOpeningDate)
              : undefined,
            tenderClosingDate: project.tenderClosingDate
              ? new Date(project.tenderClosingDate)
              : undefined,
          });
        })
        .finally(() => {
          setIsLoading(false);
        });
    }
  }, [projectId, reset]);

  const onSubmit = async (data) => {
    // Handle form submission logic here
    console.log("Form Data:", data);

    try {
      const projectData = {
        ...data,
        tenderOpeningDate: data.tenderOpeningDate?.toISOString().slice(0, 10),
        tenderClosingDate: data.tenderClosingDate?.toISOString().slice(0, 10),
      };

      if (projectId) {
        // Update existing project
        await updateProject(projectId, projectData);
        console.log("Project updated:", projectData);
      } else {
        // Create new project
        await postProject(projectData);
        console.log("Project created:", projectData);
      }
      // Optionally, show a success message or redirect
    } catch (error) {
      console.error("Error creating project:", error);
      // Optionally, show an error message to the user
    }
    router.push("/project"); // Redirect to project list or another page after submission
  };

  function handleFileChange(event) {
    const file = event.target.files[0];
    if (file) {
      // Handle the file upload logic here
      console.log("Selected file:", file.name);
    }
  }

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
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
            name="name"
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
          <div className="w-1/3">
            <SelectField
              showlabel
              label="Type"
              placeholder="Enter Location"
              name="type"
              options={typeList}
              control={control}
            />
          </div>
          <div className="w-1/3">
            <SelectField
              showlabel
              label="Status"
              placeholder="Enter Status"
              name="status"
              options={statusList}
              control={control}
            />
          </div>
          <div className="w-1/3 self-end mb-2">
            <Controller
              name="specificationDocument"
              control={control}
              render={({ field: { onChange, value } }) => {
                function handleFileChange(event) {
                  const file = event.target.files[0];
                  if (file) {
                    // Handle the file upload logic here
                    console.log("Selected file:", file.name);
                  }
                  //save the file name or path to the form state
                  onChange(file ? file.name : "");
                }

                return (
                  <div className="flex justify-end gap-2 items-center">
                    <input
                      type="file"
                      ref={fileUpload}
                      onChange={handleFileChange}
                      style={{ opacity: "0" }}
                    />
                    <Label>{value}</Label>
                    <Button
                      type="button"
                      onClick={() => fileUpload.current.click()}
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
          <div className="w-1/3">
            <Input
              className="w-full"
              showlabel
              label="Location"
              type="text"
              placeholder="Enter Location"
              name="location"
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

        <div className="flex gap-3">
          <div className="w-1/3">
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
          <div className="w-1/3">
            <DateField
              className="w-full"
              showlabel
              label="Opening Date"
              placeholder="Tender Opening date"
              name="tenderOpeningDate"
              control={control}
            />
          </div>
          <div className="w-1/3">
            <DateField
              className="w-full"
              showlabel
              label="Closing Date"
              placeholder="Tender Closing date"
              name="tenderClosingDate"
              control={control}
            />
          </div>
        </div>
        <div>
          <Button type="submit">
            {projectId ? "Update Project" : "Create Project"}
          </Button>
        </div>
      </div>
    </form>
  );
}
