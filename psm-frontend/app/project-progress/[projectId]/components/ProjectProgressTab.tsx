"use client";

import React, { useState, useEffect } from "react";
import { ProjectProgress } from "@/types";
import { ProjectProgressApprovedStatus } from "@/enums";
import {
  getProjectProgressByProjectId,
  addProjectProgress,
  updateProjectProgress,
  deleteProjectProgress,
} from "../../../api/client/projectProgressActions";
import ProjectProgressForm from "./ProjectProgressForm";

export function ProjectProgressTab({ projectId }: { projectId: string }) {
  const [progressList, setProgressList] = useState<ProjectProgress[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingProgress, setEditingProgress] =
    useState<ProjectProgress | null>(null);
  const [submitting, setSubmitting] = useState(false);

  const fetchProjectProgress = React.useCallback(async () => {
    try {
      setLoading(true);
      const progress = await getProjectProgressByProjectId(projectId);
      setProgressList(progress);
    } catch (error) {
      console.error("Error fetching project progress:", error);
    } finally {
      setLoading(false);
    }
  }, [projectId]);

  useEffect(() => {
    fetchProjectProgress();
  }, [fetchProjectProgress]);

  const handleOpenModal = (progress?: ProjectProgress) => {
    setEditingProgress(progress || null);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingProgress(null);
  };

  const handleFormSubmit = async (data: {
    summary: string;
    description?: string;
    progressPercentage: number;
    progressDate: Date;
  }) => {
    setSubmitting(true);

    try {
      const progressData = {
        summary: data.summary,
        description: data.description || "",
        progressPercentage: data.progressPercentage,
        progressDate: data.progressDate.toISOString().split("T")[0],
        projectId,
      };

      if (editingProgress) {
        await updateProjectProgress(
          editingProgress.projectProgressId,
          progressData
        );
      } else {
        await addProjectProgress(progressData);
      }

      await fetchProjectProgress();
      handleCloseModal();
    } catch (error) {
      console.error("Error saving project progress:", error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleDelete = async (progressId: string) => {
    if (!confirm("Are you sure you want to delete this progress entry?")) {
      return;
    }

    try {
      await deleteProjectProgress(progressId);
      await fetchProjectProgress();
    } catch (error) {
      console.error("Error deleting project progress:", error);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-32">
        <div className="text-gray-600 dark:text-gray-400">
          Loading progress...
        </div>
      </div>
    );
  }

  return (
    <div>
      {/* Header with Add Button */}
      <div className="flex justify-between items-center mb-6">
        <div>
          <h2 className="text-lg font-semibold text-gray-900 dark:text-white">
            Project Progress Tracking
          </h2>
          <p className="text-gray-600 dark:text-gray-400 text-sm">
            Track and manage progress updates for this project
          </p>
        </div>
        <button
          onClick={() => handleOpenModal()}
          className="px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-transparent rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
        >
          Add Progress
        </button>
      </div>

      {/* Progress List */}
      {progressList.length === 0 ? (
        <div className="text-center py-12 bg-gray-50 dark:bg-gray-800 rounded-lg">
          <div className="text-gray-500 dark:text-gray-400 mb-4">
            No progress entries found for this project.
          </div>
          <button
            onClick={() => handleOpenModal()}
            className="px-4 py-2 text-sm font-medium text-blue-600 bg-white border border-blue-600 rounded-md hover:bg-blue-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
          >
            Add First Progress Entry
          </button>
        </div>
      ) : (
        <div className="space-y-4">
          {progressList.map((progress) => (
            <div
              key={progress.projectProgressId}
              className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 p-6"
            >
              <div className="flex justify-between items-start mb-4">
                <div className="flex-1">
                  <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-2">
                    {progress.summary}
                  </h3>
                  <div className="flex items-center gap-4 text-sm text-gray-600 dark:text-gray-400 mb-2">
                    <span>Progress: {progress.progressPercentage}%</span>
                    <span>
                      Date:{" "}
                      {new Date(progress.progressDate).toLocaleDateString()}
                    </span>
                    {progress.projectProgressApprovedStatus !== undefined && (
                      <span
                        className={`px-2 py-1 rounded-full text-xs font-medium ${
                          progress.projectProgressApprovedStatus ===
                          ProjectProgressApprovedStatus.Approved
                            ? "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300"
                            : progress.projectProgressApprovedStatus ===
                              ProjectProgressApprovedStatus.Rejected
                            ? "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300"
                            : "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300"
                        }`}
                      >
                        {
                          ProjectProgressApprovedStatus[
                            progress.projectProgressApprovedStatus
                          ]
                        }
                      </span>
                    )}
                  </div>
                  <div className="w-full bg-gray-200 dark:bg-gray-700 rounded-full h-2 mb-3">
                    <div
                      className="bg-blue-600 h-2 rounded-full"
                      style={{ width: `${progress.progressPercentage}%` }}
                    ></div>
                  </div>
                </div>
                <div className="flex gap-2 ml-4">
                  <button
                    onClick={() => handleOpenModal(progress)}
                    className="px-3 py-1 text-sm text-blue-600 hover:bg-blue-50 dark:hover:bg-blue-900/20 rounded"
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => handleDelete(progress.projectProgressId)}
                    className="px-3 py-1 text-sm text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 rounded"
                  >
                    Delete
                  </button>
                </div>
              </div>

              {progress.description && (
                <p className="text-gray-700 dark:text-gray-300 mb-4">
                  {progress.description}
                </p>
              )}

              {progress.approvedNote && (
                <div className="bg-gray-50 dark:bg-gray-700 rounded p-3">
                  <div className="text-sm font-medium text-gray-900 dark:text-white mb-1">
                    Approval Note:
                  </div>
                  <div className="text-sm text-gray-700 dark:text-gray-300">
                    {progress.approvedNote}
                  </div>
                  {progress.approvedAt && (
                    <div className="text-xs text-gray-500 dark:text-gray-400 mt-1">
                      Approved on:{" "}
                      {new Date(progress.approvedAt).toLocaleDateString()}
                    </div>
                  )}
                </div>
              )}
            </div>
          ))}
        </div>
      )}

      {/* Modal */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-gray-50 bg-opacity-50 flex items-center justify-center z-50">
          <ProjectProgressForm
            initialData={editingProgress}
            onSubmit={handleFormSubmit}
            onCancel={handleCloseModal}
            isSubmitting={submitting}
          />
        </div>
      )}
    </div>
  );
}
