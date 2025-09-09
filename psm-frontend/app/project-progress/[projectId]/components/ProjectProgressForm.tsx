"use client";

import React from "react";
import { useForm } from "react-hook-form";
import * as z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "flowbite-react";
import Input from "@/app/components/forms/Input";
import TextAreaField from "@/app/components/forms/TextArea";
import DateField from "@/app/components/forms/DateField";
import { ProjectProgress } from "@/types";

// Zod schema for validation
const projectProgressSchema = z.object({
  summary: z.string().min(1, "Summary is required"),
  description: z.string().optional(),
  progressDate: z.date({
    required_error: "Progress date is required",
    invalid_type_error: "Please select a valid date",
  }),
  progressPercentage: z.coerce
    .number()
    .min(0, "Progress percentage must be at least 0")
    .max(100, "Progress percentage cannot exceed 100"),
});

type ProjectProgressFormData = z.infer<typeof projectProgressSchema>;

interface ProjectProgressFormProps {
  initialData?: ProjectProgress | null;
  onSubmit: (data: ProjectProgressFormData) => Promise<void>;
  onCancel: () => void;
  isSubmitting: boolean;
}

export default function ProjectProgressForm({
  initialData,
  onSubmit,
  onCancel,
  isSubmitting,
}: ProjectProgressFormProps) {
  const isEditing = !!initialData;

  const { control, handleSubmit } = useForm<ProjectProgressFormData>({
    resolver: zodResolver(projectProgressSchema),
    mode: "onChange",
    defaultValues: {
      summary: initialData?.summary || "",
      description: initialData?.description || "",
      progressDate: initialData?.progressDate
        ? new Date(initialData.progressDate)
        : new Date(),
      progressPercentage: initialData?.progressPercentage || 0,
    },
  });

  const handleFormSubmit = async (data: ProjectProgressFormData) => {
    try {
      await onSubmit(data);
    } catch (error) {
      console.error("Error submitting form:", error);
    }
  };

  return (
    <div className="bg-white dark:bg-gray-800 rounded-lg p-6 w-full max-w-md">
      <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
        {isEditing ? "Edit Progress" : "Add New Progress"}
      </h3>

      <form onSubmit={handleSubmit(handleFormSubmit)} className="space-y-4">
        <Input
          name="summary"
          label="Summary"
          placeholder="Brief summary of progress"
          control={control}
          showlabel
          required
        />

        <TextAreaField
          name="description"
          label="Description"
          placeholder="Detailed description of progress"
          rows={3}
          control={control}
          showlabel
        />

        <Input
          name="progressPercentage"
          label="Progress Percentage"
          type="number"
          placeholder="0-100"
          control={control}
          showlabel
          required
        />

        <DateField
          name="progressDate"
          label="Progress Date"
          placeholder="Select progress date"
          control={control}
          showlabel
          required
        />

        <div className="flex gap-3 pt-4">
          <Button
            type="submit"
            disabled={isSubmitting}
            className="flex-1"
            color="blue"
          >
            {isSubmitting
              ? "Saving..."
              : isEditing
              ? "Update Progress"
              : "Add Progress"}
          </Button>
          <Button
            type="button"
            onClick={onCancel}
            className="flex-1"
            color="gray"
            outline
          >
            Cancel
          </Button>
        </div>
      </form>
    </div>
  );
}
