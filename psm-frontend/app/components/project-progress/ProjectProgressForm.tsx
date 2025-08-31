"use client";

import React, { useState, useEffect, useCallback } from "react";
import {
  Modal,
  Button,
  Label,
  TextInput,
  Textarea,
  Select,
} from "flowbite-react";
import { ProjectProgress, Project } from "@/types";
import { ProjectProgressApprovedStatus } from "@/enums";
import { getAllProjects } from "@/app/api/actions/projectActions";

interface ProjectProgressFormProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (progressData: Partial<ProjectProgress>) => Promise<void>;
  editingProgress: ProjectProgress | null;
  defaultProjectId?: string;
}

export default function ProjectProgressForm({
  isOpen,
  onClose,
  onSubmit,
  editingProgress,
  defaultProjectId,
}: ProjectProgressFormProps) {
  const [formData, setFormData] = useState<Partial<ProjectProgress>>({
    projectId: "",
    summary: "",
    description: "",
    progressDate: new Date(),
    progressPercentage: 0,
    projectProgressApprovedStatus: ProjectProgressApprovedStatus.Pending,
  });
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  const resetForm = useCallback(() => {
    setFormData({
      projectId: defaultProjectId || "",
      summary: "",
      description: "",
      progressDate: new Date(),
      progressPercentage: 0,
      projectProgressApprovedStatus: ProjectProgressApprovedStatus.Pending,
    });
  }, [defaultProjectId]);

  useEffect(() => {
    if (isOpen) {
      loadProjects();
      if (editingProgress) {
        setFormData(editingProgress);
      } else {
        resetForm();
      }
    }
  }, [isOpen, editingProgress, resetForm]);

  const loadProjects = async () => {
    setLoading(true);
    try {
      const projectsData = await getAllProjects();
      setProjects(projectsData);
    } catch (error) {
      console.error("Error loading projects:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
    >
  ) => {
    const { name, value } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]:
        name === "progressPercentage"
          ? Math.min(100, Math.max(0, parseInt(value) || 0))
          : name === "progressDate"
          ? new Date(value)
          : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (
      !formData.projectId ||
      !formData.summary ||
      formData.progressPercentage === undefined
    ) {
      alert("Please fill in all required fields.");
      return;
    }

    setSubmitting(true);
    try {
      await onSubmit(formData);
      resetForm();
    } catch (error) {
      console.error("Error submitting form:", error);
      alert("Error submitting progress. Please try again.");
    } finally {
      setSubmitting(false);
    }
  };

  const formatDateForInput = (date: Date) => {
    return date.toISOString().split("T")[0];
  };

  return (
    <Modal show={isOpen} onClose={onClose} size="lg">
      <div className="p-6">
        <div className="flex items-center justify-between pb-4 mb-4 border-b border-gray-200">
          <h3 className="text-xl font-semibold text-gray-900">
            {editingProgress ? "Edit Project Progress" : "Add Project Progress"}
          </h3>
          <button
            onClick={onClose}
            className="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5"
          >
            <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
              <path
                fillRule="evenodd"
                d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                clipRule="evenodd"
              />
            </svg>
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <Label htmlFor="projectId">Project *</Label>
            <Select
              id="projectId"
              name="projectId"
              value={formData.projectId}
              onChange={handleInputChange}
              required
              disabled={loading}
            >
              <option value="">Select a project...</option>
              {projects.map((project) => (
                <option key={project.id} value={project.id?.toString()}>
                  {project.subject} ({project.id})
                </option>
              ))}
            </Select>
          </div>

          <div>
            <Label htmlFor="summary">Summary *</Label>
            <TextInput
              id="summary"
              name="summary"
              value={formData.summary}
              onChange={handleInputChange}
              placeholder="Brief summary of progress"
              required
            />
          </div>

          <div>
            <Label htmlFor="description">Description</Label>
            <Textarea
              id="description"
              name="description"
              value={formData.description}
              onChange={handleInputChange}
              placeholder="Detailed description of progress"
              rows={4}
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <Label htmlFor="progressDate">Progress Date *</Label>
              <TextInput
                id="progressDate"
                name="progressDate"
                type="date"
                value={
                  formData.progressDate
                    ? formatDateForInput(formData.progressDate)
                    : ""
                }
                onChange={handleInputChange}
                required
              />
            </div>

            <div>
              <Label htmlFor="progressPercentage">Progress Percentage *</Label>
              <TextInput
                id="progressPercentage"
                name="progressPercentage"
                type="number"
                min="0"
                max="100"
                value={formData.progressPercentage}
                onChange={handleInputChange}
                placeholder="0-100"
                required
              />
            </div>
          </div>

          <div>
            <Label htmlFor="projectProgressApprovedStatus">
              Approval Status
            </Label>
            <Select
              id="projectProgressApprovedStatus"
              name="projectProgressApprovedStatus"
              value={formData.projectProgressApprovedStatus}
              onChange={handleInputChange}
            >
              <option value={ProjectProgressApprovedStatus.Pending}>
                Pending
              </option>
              <option value={ProjectProgressApprovedStatus.Approved}>
                Approved
              </option>
              <option value={ProjectProgressApprovedStatus.Rejected}>
                Rejected
              </option>
            </Select>
          </div>

          <div>
            <Label htmlFor="approvedNote">Approval Note</Label>
            <Textarea
              id="approvedNote"
              name="approvedNote"
              value={formData.approvedNote || ""}
              onChange={handleInputChange}
              placeholder="Note from approver (if any)"
              rows={2}
            />
          </div>

          <div className="flex justify-end space-x-3 pt-4 border-t border-gray-200">
            <Button
              type="button"
              color="alternative"
              onClick={onClose}
              disabled={submitting}
            >
              Cancel
            </Button>
            <Button
              type="submit"
              disabled={submitting}
              className="bg-blue-600 hover:bg-blue-700"
            >
              {submitting ? "Saving..." : editingProgress ? "Update" : "Add"}{" "}
              Progress
            </Button>
          </div>
        </form>
      </div>
    </Modal>
  );
}
