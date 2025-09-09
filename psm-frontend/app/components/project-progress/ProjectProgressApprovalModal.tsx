"use client";

import React from "react";
import { useForm } from "react-hook-form";
import * as z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Button,
} from "flowbite-react";
import Input from "@/app/components/forms/Input";
import SelectField from "@/app/components/forms/Select";
import { ProjectProgressApprovedStatus } from "@/enums";
import { updateProjectProgress } from "@/app/api/client/projectProgressActions";
import toast from "react-hot-toast";

// Zod schema for validation
const approvalSchema = z.object({
  approvedNote: z.string().optional(),
  projectProgressApprovedStatus: z.coerce
    .number()
    .refine((val) => val in ProjectProgressApprovedStatus, {
      message: "Invalid approval status",
    }),
});

type ApprovalFormData = z.infer<typeof approvalSchema>;

interface ProjectProgressApprovalModalProps {
  isOpen: boolean;
  onClose: () => void;
  projectProgressId: string;
  currentStatus?: ProjectProgressApprovedStatus;
  currentNote?: string;
  onSuccess?: () => void;
}

// Status options for the select field
const statusOptions = [
  { value: ProjectProgressApprovedStatus.Pending, text: "Pending" },
  { value: ProjectProgressApprovedStatus.Approved, text: "Approved" },
  { value: ProjectProgressApprovedStatus.Rejected, text: "Rejected" },
];

export default function ProjectProgressApprovalModal({
  isOpen,
  onClose,
  projectProgressId,
  currentStatus = ProjectProgressApprovedStatus.Pending,
  currentNote = "",
  onSuccess,
}: ProjectProgressApprovalModalProps) {
  const {
    control,
    handleSubmit,
    reset,
    formState: { isSubmitting },
  } = useForm<ApprovalFormData>({
    resolver: zodResolver(approvalSchema),
    mode: "onChange",
    defaultValues: {
      approvedNote: currentNote,
      projectProgressApprovedStatus: currentStatus,
    },
  });

  // Reset form when modal opens or closes
  React.useEffect(() => {
    if (isOpen) {
      reset({
        approvedNote: currentNote,
        projectProgressApprovedStatus: currentStatus,
      });
    }
  }, [isOpen, currentNote, currentStatus, reset]);

  const onSubmit = async (data: ApprovalFormData) => {
    try {
      // Update only the approval fields
      const response = await updateProjectProgress(projectProgressId, {
        approvedNote: data.approvedNote,
        projectProgressApprovedStatus: data.projectProgressApprovedStatus,
      });

      if (response.isSuccess) {
        toast.success(
          response.message || "Project progress updated successfully!"
        );

        // Call success callback if provided
        if (onSuccess) {
          onSuccess();
        }

        // Close the modal
        onClose();
      } else {
        toast.error(response.message || "Failed to update project progress");
      }
    } catch (error) {
      console.error("Error updating project progress:", error);
      toast.error("Failed to update project progress. Please try again.");
    }
  };

  const handleClose = () => {
    reset();
    onClose();
  };

  return (
    <Modal show={isOpen} onClose={handleClose} size="md">
      <ModalHeader>Update Project Progress Approval</ModalHeader>
      <ModalBody>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <SelectField
              name="projectProgressApprovedStatus"
              control={control}
              label="Approval Status"
              showlabel={true}
              required={true}
              options={statusOptions}
              placeholder="Select approval status"
            />
          </div>

          <div>
            <Input
              name="approvedNote"
              control={control}
              label="Approval Note"
              showlabel={true}
              placeholder="Enter approval note (optional)"
              type="text"
            />
          </div>
        </form>
      </ModalBody>
      <ModalFooter>
        <Button
          onClick={handleSubmit(onSubmit)}
          disabled={isSubmitting}
          className="bg-blue-600 hover:bg-blue-700"
        >
          {isSubmitting ? "Updating..." : "Update"}
        </Button>
        <Button color="gray" onClick={handleClose} disabled={isSubmitting}>
          Cancel
        </Button>
      </ModalFooter>
    </Modal>
  );
}
