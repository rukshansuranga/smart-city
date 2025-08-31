"use client";
import { useParams } from "next/navigation";
import { HiClipboardList, HiCurrencyDollar, HiChartPie } from "react-icons/hi";
import ProjectForm from "../../ProjectForm";
import TenderListComponent from "../../../components/tender/TenderList";
import { ProjectProgressList } from "../../../components/project-progress";
import { useState, useEffect, useCallback } from "react";
import { getProject } from "../../../api/client/projectActions";
import { getTendersByProjectIdId } from "../../../api/client/tenderActions";
import { getProjectProgressByProjectId } from "../../../api/client/projectProgressActions";
import { Project, Tender, ProjectProgress } from "@/types";
import { Spinner } from "flowbite-react";
import toast from "react-hot-toast";

export default function UpdateProject() {
  const { id } = useParams<{ id: string }>();
  const [activeTab, setActiveTab] = useState("details");

  // Data states
  const [projectData, setProjectData] = useState<Project | null>(null);
  const [tendersData, setTendersData] = useState<Tender[]>([]);
  const [progressData, setProgressData] = useState<ProjectProgress[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Load all data when component mounts
  const loadAllData = useCallback(async () => {
    if (!id) return;

    setIsLoading(true);
    setError(null);

    try {
      // Load all data in parallel
      const [project, tenders, progress] = await Promise.all([
        getProject(id),
        getTendersByProjectIdId(id),
        getProjectProgressByProjectId(id),
      ]);

      if (!project.isSuccess) {
        toast.error(project.message || "Failed to load project");
        setProgressData([]);
      }

      if (!tenders.isSuccess) {
        toast.error(tenders.message || "Failed to load tenders");
        setTendersData([]);
      }

      setProjectData(project.data);
      setTendersData(tenders.data);
      setProgressData(progress);
    } catch (err) {
      console.error("Error loading data:", err);
      setError("Failed to load project data");
    } finally {
      setIsLoading(false);
    }
  }, [id]);

  useEffect(() => {
    loadAllData();
  }, [loadAllData]);

  // Refresh functions for child components
  const refreshTenders = useCallback(async () => {
    if (!id) return;
    try {
      const response = await getTendersByProjectIdId(id);

      if (!response.isSuccess) {
        toast.error(response.message || "Failed to load tenders");
        setTendersData([]);
        return;
      }

      setTendersData(response.data);
    } catch (err) {
      console.error("Error refreshing tenders:", err);
    }
  }, [id]);

  const refreshProgress = useCallback(async () => {
    if (!id) return;
    try {
      const progress = await getProjectProgressByProjectId(id);
      setProgressData(progress);
    } catch (err) {
      console.error("Error refreshing progress:", err);
    }
  }, [id]);

  // Check if tender tab should be enabled
  const isTenderTabEnabled = (project: Project): boolean => {
    if (!project.tenderOpeningDate) {
      return false;
    }

    const today = new Date();
    const tenderOpeningDate = new Date(project.tenderOpeningDate);

    // Set time to start of day for accurate comparison
    today.setHours(0, 0, 0, 0);
    tenderOpeningDate.setHours(0, 0, 0, 0);

    return tenderOpeningDate <= today;
  };

  // Effect to handle tab switching when tender becomes disabled
  useEffect(() => {
    if (
      projectData &&
      activeTab === "tender" &&
      !isTenderTabEnabled(projectData)
    ) {
      setActiveTab("details");
    }
  }, [projectData, activeTab]);

  const tabs = [
    { id: "details", label: "Project Details", icon: HiClipboardList },
    { id: "tender", label: "Tender", icon: HiCurrencyDollar },
    { id: "progress", label: "Progress", icon: HiChartPie },
  ];

  const renderTabContent = () => {
    if (isLoading) {
      return (
        <div className="flex justify-center items-center py-12">
          <Spinner aria-label="Loading project data" size="lg" />
        </div>
      );
    }

    if (error) {
      return (
        <div className="flex flex-col items-center justify-center py-12">
          <div className="text-red-500 mb-4">{error}</div>
          <button
            onClick={loadAllData}
            className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
          >
            Retry
          </button>
        </div>
      );
    }

    switch (activeTab) {
      case "details":
        return (
          <div className="p-4">
            <ProjectForm projectId={id} initialData={projectData} />
          </div>
        );
      case "tender":
        if (projectData && !isTenderTabEnabled(projectData)) {
          return (
            <div className="p-4">
              <div className="text-center py-12">
                <div className="max-w-sm mx-auto">
                  <div className="w-16 h-16 mx-auto mb-4 text-gray-300 dark:text-gray-600">
                    <svg
                      fill="currentColor"
                      viewBox="0 0 20 20"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        fillRule="evenodd"
                        d="M5 9V7a5 5 0 0110 0v2a2 2 0 012 2v5a2 2 0 01-2 2H5a2 2 0 01-2-2v-5a2 2 0 012-2zm8-2v2H7V7a3 3 0 016 0z"
                        clipRule="evenodd"
                      />
                    </svg>
                  </div>
                  <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-2">
                    Tender Not Available
                  </h3>
                  <p className="text-gray-500 dark:text-gray-400 mb-4">
                    {projectData.tenderOpeningDate
                      ? `This tender will be available on ${new Date(
                          projectData.tenderOpeningDate
                        ).toLocaleDateString()}`
                      : "No tender opening date has been set for this project"}
                  </p>
                  {projectData.tenderOpeningDate && (
                    <div className="text-sm text-gray-400 dark:text-gray-500">
                      Current date: {new Date().toLocaleDateString()}
                    </div>
                  )}
                </div>
              </div>
            </div>
          );
        }
        return (
          <div className="p-4">
            <div className="mb-4">
              <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                Tender Management
              </h3>
              <p className="text-sm text-gray-600 dark:text-gray-400">
                View and manage tenders for this project
              </p>
            </div>
            <TenderListComponent
              projectId={id}
              initialData={tendersData}
              onDataChange={refreshTenders}
            />
          </div>
        );
      case "progress":
        return (
          <div className="p-4">
            <div className="mb-6">
              <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                Project Progress
              </h3>
              <p className="text-sm text-gray-600 dark:text-gray-400">
                Track project milestones and completion status
              </p>
            </div>
            <ProjectProgressList
              projectId={id}
              initialData={progressData}
              onDataChange={refreshProgress}
            />
          </div>
        );
      default:
        return null;
    }
  };

  return (
    <div className="flex flex-col gap-4">
      <div className="mb-4">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
          Update Project
        </h1>
        <p className="text-gray-600 dark:text-gray-400">
          Manage project details, tenders, and track progress
        </p>
      </div>

      {/* Custom Tabs */}
      <div className="border-b border-gray-200 dark:border-gray-700">
        <nav className="-mb-px flex space-x-8">
          {tabs.map((tab) => {
            const Icon = tab.icon;
            const isDisabled =
              tab.id === "tender" &&
              projectData &&
              !isTenderTabEnabled(projectData);
            const isActive = activeTab === tab.id;

            return (
              <button
                key={tab.id}
                onClick={() => {
                  if (!isDisabled) {
                    setActiveTab(tab.id);
                  }
                }}
                disabled={!!isDisabled}
                className={`flex items-center py-4 px-1 border-b-2 font-medium text-sm transition-colors ${
                  isActive
                    ? "border-blue-500 text-blue-600 dark:text-blue-500 dark:border-blue-500"
                    : isDisabled
                    ? "border-transparent text-gray-300 cursor-not-allowed dark:text-gray-600"
                    : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300"
                }`}
                title={
                  isDisabled && projectData
                    ? projectData.tenderOpeningDate
                      ? `Tender opens on ${new Date(
                          projectData.tenderOpeningDate
                        ).toLocaleDateString()}`
                      : "No tender opening date set"
                    : undefined
                }
              >
                <Icon className="w-5 h-5 mr-2" />
                {tab.label}
              </button>
            );
          })}
        </nav>
      </div>

      {/* Tab Content */}
      <div className="bg-white dark:bg-gray-900 rounded-lg">
        {renderTabContent()}
      </div>
    </div>
  );
}
