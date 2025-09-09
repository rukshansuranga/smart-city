"use client";
import { useState, useEffect } from "react";
import {
  Spinner,
  Badge,
  Button,
  Modal,
  ModalHeader,
  ModalBody,
} from "flowbite-react";
import {
  HiCheckCircle,
  HiClock,
  HiExclamationCircle,
  HiPencilAlt,
} from "react-icons/hi";
import { getProjectProgressByProjectId } from "@/app/api/client/projectProgressActions";
import { ProjectProgress } from "@/types";
import { ProjectProgressApprovedStatus } from "@/enums";
import ProjectProgressApprovalModal from "./ProjectProgressApprovalModal";

interface ProjectProgressListProps {
  projectId: string;
  onEditProgress?: (progress: ProjectProgress) => void;
  refreshTrigger?: number;
  initialData?: ProjectProgress[];
}

export default function ProjectProgressList({
  projectId,
  onEditProgress,
  refreshTrigger,
  initialData,
}: ProjectProgressListProps) {
  const [progressList, setProgressList] = useState<ProjectProgress[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [selectedProgress, setSelectedProgress] =
    useState<ProjectProgress | null>(null);

  // Approval modal state
  const [showApprovalModal, setShowApprovalModal] = useState(false);
  const [approvalProgress, setApprovalProgress] =
    useState<ProjectProgress | null>(null);

  // Handle opening modal to view progress details
  const handleViewProgress = (progress: ProjectProgress) => {
    setSelectedProgress(progress);
    setShowModal(true);
  };

  // Handle closing modal
  const handleCloseModal = () => {
    setShowModal(false);
    setSelectedProgress(null);
  };

  // Handle opening approval modal
  const handleApprovalModal = (progress: ProjectProgress) => {
    setApprovalProgress(progress);
    setShowApprovalModal(true);
  };

  // Handle closing approval modal
  const handleCloseApprovalModal = () => {
    setShowApprovalModal(false);
    setApprovalProgress(null);
  };

  // Handle approval success
  const handleApprovalSuccess = () => {
    // Refresh the progress list after successful approval
    const loadProgressData = async () => {
      if (!initialData) {
        try {
          const response = await getProjectProgressByProjectId(projectId);
          if (response.isSuccess && response.data) {
            setProgressList(response.data);
          } else {
            console.error(
              "Error refreshing project progress:",
              response.message
            );
            setProgressList([]);
          }
        } catch (error) {
          console.error("Error refreshing project progress:", error);
          setProgressList([]);
        }
      }
    };

    if (projectId) {
      loadProgressData();
    }
  };

  useEffect(() => {
    const loadProgressData = async () => {
      if (initialData) {
        // Use provided initial data
        setProgressList(initialData);
        setIsLoading(false);
        return;
      }

      // Fetch data if no initial data provided
      setIsLoading(true);
      try {
        const response = await getProjectProgressByProjectId(projectId);
        if (response.isSuccess && response.data) {
          setProgressList(response.data);
        } else {
          console.error("Error fetching project progress:", response.message);
          setProgressList([]);
        }
      } catch (error) {
        console.error("Error fetching project progress:", error);
        setProgressList([]);
      } finally {
        setIsLoading(false);
      }
    };

    if (projectId) {
      loadProgressData();
    }
  }, [projectId, refreshTrigger, initialData]);

  const getStatusIcon = (status?: ProjectProgressApprovedStatus) => {
    switch (status) {
      case ProjectProgressApprovedStatus.Approved:
        return <HiCheckCircle className="w-5 h-5 text-green-500" />;
      case ProjectProgressApprovedStatus.Pending:
        return <HiClock className="w-5 h-5 text-blue-500" />;
      case ProjectProgressApprovedStatus.Rejected:
        return <HiExclamationCircle className="w-5 h-5 text-red-500" />;
      default:
        return <HiClock className="w-5 h-5 text-gray-500" />;
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-8">
        <Spinner aria-label="Loading project progress" size="lg" />
      </div>
    );
  }

  if (progressList.length === 0) {
    return (
      <div className="text-center py-8">
        <p className="text-gray-500 dark:text-gray-400">
          Unable to load project progress data.
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Progress Overview */}
      {/* <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div className="bg-white dark:bg-gray-800 p-4 rounded-lg shadow">
          <h4 className="text-sm font-medium text-gray-500 dark:text-gray-400 mb-2">
            Overall Progress
          </h4>
          <div className="space-y-2">
            <div className="flex justify-between text-sm">
              <span className="text-gray-900 dark:text-white">Progress</span>
              <span className="text-gray-900 dark:text-white">
                {progressData?.overallProgress || 0}%
              </span>
            </div>
            <Progress
              progress={progressData?.overallProgress || 0}
              color="blue"
            />
          </div>
        </div>

        <div className="bg-white dark:bg-gray-800 p-4 rounded-lg shadow">
          <h4 className="text-sm font-medium text-gray-500 dark:text-gray-400">
            Tasks Completed
          </h4>
          <p className="mt-2 text-2xl font-semibold text-gray-900 dark:text-white">
            {progressData?.tasksCompleted || 0}/{progressData?.totalTasks || 0}
          </p>
        </div>

        <div className="bg-white dark:bg-gray-800 p-4 rounded-lg shadow">
          <h4 className="text-sm font-medium text-gray-500 dark:text-gray-400">
            Days Remaining
          </h4>
          <p className="mt-2 text-2xl font-semibold text-gray-900 dark:text-white">
            {progressData?.daysRemaining || 0}
          </p>
        </div>
      </div> */}

      {/* Task List */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow">
        <div className="p-6 border-b border-gray-200 dark:border-gray-600">
          <div className="flex items-center justify-between">
            <div>
              <h4 className="text-lg font-medium text-gray-900 dark:text-white">
                Progress Updates
              </h4>
              <p className="text-sm text-gray-600 dark:text-gray-400">
                Track project progress submissions and approvals
              </p>
            </div>
          </div>
        </div>
        {progressList && progressList.length > 0 ? (
          <div className="divide-y divide-gray-200 dark:divide-gray-600">
            {progressList.map((progress) => (
              <div key={progress.projectProgressId} className="p-6">
                <div className="flex items-start justify-between">
                  <div className="flex items-start space-x-3 flex-1">
                    <div className="flex-shrink-0 mt-1">
                      {getStatusIcon(progress.projectProgressApprovedStatus)}
                    </div>
                    <div className="flex-1 min-w-0">
                      <div className="flex items-center justify-between mb-2">
                        <h5 className="text-sm font-medium text-gray-900 dark:text-white">
                          {progress.summary || "Progress Update"}
                        </h5>
                        <div className="flex items-center space-x-2">
                          <span className="text-xs font-medium text-blue-600 dark:text-blue-400">
                            {progress.progressPercentage}%
                          </span>
                          <Badge
                            color={
                              progress.projectProgressApprovedStatus ===
                              ProjectProgressApprovedStatus.Approved
                                ? "success"
                                : progress.projectProgressApprovedStatus ===
                                  ProjectProgressApprovedStatus.Rejected
                                ? "failure"
                                : "gray"
                            }
                            size="sm"
                          >
                            {progress.projectProgressApprovedStatus ===
                            ProjectProgressApprovedStatus.Approved
                              ? "Approved"
                              : progress.projectProgressApprovedStatus ===
                                ProjectProgressApprovedStatus.Rejected
                              ? "Rejected"
                              : "Pending"}
                          </Badge>
                        </div>
                      </div>
                      {progress.description && (
                        <p className="text-sm text-gray-600 dark:text-gray-400 mb-2">
                          {progress.description}
                        </p>
                      )}
                      <div className="flex items-center space-x-4 text-xs text-gray-500 dark:text-gray-400">
                        <span>
                          Progress Date:{" "}
                          {new Date(progress.progressDate).toLocaleDateString()}
                        </span>
                        {progress.approvedBy && (
                          <span>Approved by: {progress.approvedBy}</span>
                        )}
                        {progress.approvedAt && (
                          <span>
                            Approved:{" "}
                            {new Date(progress.approvedAt).toLocaleDateString()}
                          </span>
                        )}
                      </div>
                      {progress.approvedNote && (
                        <div className="mt-2">
                          <p className="text-xs text-gray-600 dark:text-gray-400">
                            <strong>Note:</strong> {progress.approvedNote}
                          </p>
                        </div>
                      )}

                      <div className="mt-3 pt-3 border-t border-gray-100 dark:border-gray-600 space-y-2">
                        <div className="flex flex-wrap gap-2">
                          {onEditProgress && (
                            <Button
                              color="gray"
                              size="xs"
                              onClick={() => onEditProgress(progress)}
                              className="text-xs"
                            >
                              Edit Progress
                            </Button>
                          )}
                          <Button
                            color="gray"
                            size="xs"
                            onClick={() => handleViewProgress(progress)}
                            className="text-xs"
                          >
                            View Details
                          </Button>
                        </div>
                        <div className="flex flex-wrap gap-2">
                          <Button
                            color="blue"
                            size="xs"
                            onClick={() => handleApprovalModal(progress)}
                            className="text-xs"
                          >
                            <HiPencilAlt className="w-3 h-3 mr-1" />
                            Update Approval
                          </Button>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        ) : (
          <div className="p-8 text-center">
            <div className="text-gray-400 dark:text-gray-500 mb-2">
              <svg
                className="mx-auto h-12 w-12"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 48 48"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </div>
            <h3 className="text-sm font-medium text-gray-900 dark:text-white mb-1">
              No Progress Updates
            </h3>
            <p className="text-sm text-gray-500 dark:text-gray-400">
              Progress updates will appear here as they are added to the
              project.
            </p>
          </div>
        )}
      </div>

      {/* Progress Detail Modal */}
      <Modal show={showModal} onClose={handleCloseModal} size="xl">
        <ModalHeader>Progress Details</ModalHeader>
        <ModalBody>
          {selectedProgress && (
            <div className="space-y-4">
              <div className="flex items-center justify-between">
                <h4 className="text-lg font-medium text-gray-900 dark:text-white">
                  {selectedProgress.summary || "Progress Update"}
                </h4>
                <div className="flex items-center space-x-2">
                  <span className="text-sm font-medium text-blue-600 dark:text-blue-400">
                    {selectedProgress.progressPercentage}%
                  </span>
                  <Badge
                    color={
                      selectedProgress.projectProgressApprovedStatus ===
                      ProjectProgressApprovedStatus.Approved
                        ? "success"
                        : selectedProgress.projectProgressApprovedStatus ===
                          ProjectProgressApprovedStatus.Rejected
                        ? "failure"
                        : "gray"
                    }
                  >
                    {selectedProgress.projectProgressApprovedStatus ===
                    ProjectProgressApprovedStatus.Approved
                      ? "Approved"
                      : selectedProgress.projectProgressApprovedStatus ===
                        ProjectProgressApprovedStatus.Rejected
                      ? "Rejected"
                      : "Pending"}
                  </Badge>
                </div>
              </div>

              {selectedProgress.description && (
                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                    Description
                  </label>
                  <p className="text-sm text-gray-600 dark:text-gray-400 bg-gray-50 dark:bg-gray-700 p-3 rounded-lg">
                    {selectedProgress.description}
                  </p>
                </div>
              )}

              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                    Progress Date
                  </label>
                  <p className="text-sm text-gray-900 dark:text-white">
                    {new Date(
                      selectedProgress.progressDate
                    ).toLocaleDateString()}
                  </p>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                    Progress Percentage
                  </label>
                  <p className="text-sm text-gray-900 dark:text-white">
                    {selectedProgress.progressPercentage}%
                  </p>
                </div>

                {selectedProgress.approvedBy && (
                  <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                      Approved By
                    </label>
                    <p className="text-sm text-gray-900 dark:text-white">
                      {selectedProgress.approvedBy}
                    </p>
                  </div>
                )}

                {selectedProgress.approvedAt && (
                  <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                      Approved Date
                    </label>
                    <p className="text-sm text-gray-900 dark:text-white">
                      {new Date(
                        selectedProgress.approvedAt
                      ).toLocaleDateString()}
                    </p>
                  </div>
                )}
              </div>

              {selectedProgress.approvedNote && (
                <div>
                  <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                    Approval Note
                  </label>
                  <p className="text-sm text-gray-600 dark:text-gray-400 bg-gray-50 dark:bg-gray-700 p-3 rounded-lg">
                    {selectedProgress.approvedNote}
                  </p>
                </div>
              )}

              <div className="flex items-center space-x-2">
                <div className="flex-shrink-0">
                  {selectedProgress.projectProgressApprovedStatus ===
                  ProjectProgressApprovedStatus.Approved ? (
                    <HiCheckCircle className="w-5 h-5 text-green-500" />
                  ) : selectedProgress.projectProgressApprovedStatus ===
                    ProjectProgressApprovedStatus.Rejected ? (
                    <HiExclamationCircle className="w-5 h-5 text-red-500" />
                  ) : (
                    <HiClock className="w-5 h-5 text-blue-500" />
                  )}
                </div>
                <span className="text-sm text-gray-600 dark:text-gray-400">
                  Status:{" "}
                  {selectedProgress.projectProgressApprovedStatus ===
                  ProjectProgressApprovedStatus.Approved
                    ? "This progress update has been approved"
                    : selectedProgress.projectProgressApprovedStatus ===
                      ProjectProgressApprovedStatus.Rejected
                    ? "This progress update has been rejected"
                    : "This progress update is pending approval"}
                </span>
              </div>
            </div>
          )}
        </ModalBody>
      </Modal>

      {/* Project Progress Approval Modal */}
      <ProjectProgressApprovalModal
        isOpen={showApprovalModal}
        onClose={handleCloseApprovalModal}
        projectProgressId={approvalProgress?.projectProgressId || ""}
        currentStatus={approvalProgress?.projectProgressApprovedStatus}
        currentNote={approvalProgress?.approvedNote}
        onSuccess={handleApprovalSuccess}
      />
    </div>
  );
}
