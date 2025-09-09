"use client";

import React, { useState, useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import { PageProtection } from "../../components/PageProtection";
import {
  ProjectProgressTab,
  ProjectDetailTab,
  BidInformationTab,
} from "./components";
import { Project } from "@/types";
import { ProjectStatus } from "@/enums";
import { getProjectById } from "../../api/client/projectActions";

function ProjectProgressDetailContent() {
  const params = useParams();
  const router = useRouter();
  const projectId = params.projectId as string;

  const [activeTab, setActiveTab] = useState("progress");
  const [project, setProject] = useState<Project | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProject = async () => {
      if (!projectId) {
        setError("Project ID is required");
        setLoading(false);
        return;
      }

      try {
        setLoading(true);
        const response = await getProjectById(projectId);

        if (!response.isSuccess) {
          setError(response.message || "Failed to load project");
          setProject(null);
        }

        setProject(response.data || null);
      } catch (err) {
        console.error("Error fetching project:", err);
        setError("Failed to load project details");
      } finally {
        setLoading(false);
      }
    };

    fetchProject();
  }, [projectId]);

  const handleBackToProjects = () => {
    router.push("/project-progress");
  };

  if (loading) {
    return (
      <div className="p-6">
        <div className="flex items-center justify-center h-64">
          <div className="text-gray-600 dark:text-gray-400">
            Loading project details...
          </div>
        </div>
      </div>
    );
  }

  if (error || !project) {
    return (
      <div className="p-6">
        <div className="flex items-center justify-center h-64">
          <div className="text-center">
            <div className="text-red-600 dark:text-red-400 mb-4">
              {error || "Project not found"}
            </div>
            <button
              onClick={handleBackToProjects}
              className="px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-transparent rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
            >
              Back to Projects
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="p-6">
      {/* Header with back button and project info */}
      <div className="mb-6">
        <div className="flex items-center gap-4 mb-4">
          <button
            onClick={handleBackToProjects}
            className="flex items-center text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-white"
          >
            <svg
              className="w-5 h-5 mr-2"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M15 19l-7-7 7-7"
              />
            </svg>
            Back to Projects
          </button>
        </div>

        <div className="flex items-start justify-between">
          <div>
            <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">
              {project.subject}
            </h1>
            <div className="flex items-center gap-4 text-sm text-gray-600 dark:text-gray-400">
              <span>City: {project.city}</span>
              <span>Type: {project.type}</span>
              <span
                className={`px-2 py-1 rounded-full text-xs font-medium ${
                  project.status === ProjectStatus.InProgress
                    ? "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300"
                    : project.status === ProjectStatus.New
                    ? "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300"
                    : project.status === ProjectStatus.Completed
                    ? "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-300"
                    : "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300"
                }`}
              >
                {ProjectStatus[project.status]}
              </span>
            </div>
          </div>
          {project.estimatedCost && (
            <div className="text-right">
              <div className="text-sm text-gray-500 dark:text-gray-400">
                Estimated Cost
              </div>
              <div className="text-lg font-semibold text-gray-900 dark:text-white">
                ${project.estimatedCost.toLocaleString()}
              </div>
            </div>
          )}
        </div>
      </div>

      {/* Tabs Navigation */}
      <div className="w-full">
        <div className="border-b border-gray-200 dark:border-gray-600">
          <nav className="-mb-px flex space-x-8">
            <button
              onClick={() => setActiveTab("progress")}
              className={`py-2 px-1 border-b-2 font-medium text-sm ${
                activeTab === "progress"
                  ? "border-blue-500 text-blue-600 dark:text-blue-400"
                  : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300"
              }`}
            >
              Project Progress
            </button>
            <button
              onClick={() => setActiveTab("details")}
              className={`py-2 px-1 border-b-2 font-medium text-sm ${
                activeTab === "details"
                  ? "border-blue-500 text-blue-600 dark:text-blue-400"
                  : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300"
              }`}
            >
              Project Details
            </button>
            <button
              onClick={() => setActiveTab("bids")}
              className={`py-2 px-1 border-b-2 font-medium text-sm ${
                activeTab === "bids"
                  ? "border-blue-500 text-blue-600 dark:text-blue-400"
                  : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300"
              }`}
            >
              Bid Information
            </button>
          </nav>
        </div>

        {/* Tab Content */}
        <div className="mt-6">
          {activeTab === "progress" && (
            <ProjectProgressTab projectId={projectId} />
          )}
          {activeTab === "details" && (
            <ProjectDetailTab projectId={projectId} project={project} />
          )}
          {activeTab === "bids" && (
            <BidInformationTab projectId={projectId} project={project} />
          )}
        </div>
      </div>
    </div>
  );
}

export default function ProjectProgressDetailPage() {
  return (
    <PageProtection permission="projectprogress">
      <ProjectProgressDetailContent />
    </PageProtection>
  );
}
