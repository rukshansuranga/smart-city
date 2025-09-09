"use client";

import React, { useState, useEffect } from "react";
import { Spinner, Badge, Card } from "flowbite-react";
import {
  HiLocationMarker,
  HiCalendar,
  HiCurrencyDollar,
  HiDocument,
} from "react-icons/hi";
import { getProject } from "@/app/api/client/projectActions";
import { Project } from "@/types";
import { ProjectStatus, ProjectType } from "@/enums";

interface ProjectDetailProps {
  projectId: string;
}

export default function ProjectDetail({ projectId }: ProjectDetailProps) {
  const [project, setProject] = useState<Project | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadProjectDetails = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const response = await getProject(projectId);
        if (response.isSuccess && response.data) {
          setProject(response.data);
        } else {
          setError(response.message || "Failed to load project details");
        }
      } catch (error) {
        console.error("Error fetching project details:", error);
        setError("Failed to load project details");
      } finally {
        setIsLoading(false);
      }
    };

    if (projectId) {
      loadProjectDetails();
    }
  }, [projectId]);

  const getStatusBadge = (status: ProjectStatus) => {
    const statusConfig = {
      [ProjectStatus.New]: { color: "gray", text: "New" },
      [ProjectStatus.InProgress]: { color: "info", text: "In Progress" },
      [ProjectStatus.Completed]: { color: "success", text: "Completed" },
      [ProjectStatus.OnHold]: { color: "warning", text: "On Hold" },
    };

    const config = statusConfig[status] || statusConfig[ProjectStatus.New];

    return (
      <Badge color={config.color} size="sm">
        {config.text}
      </Badge>
    );
  };

  const getProjectTypeText = (type: ProjectType) => {
    switch (type) {
      case ProjectType.Road:
        return "Road Construction";
      case ProjectType.Building:
        return "Building Construction";
      case ProjectType.Irrigation:
        return "Irrigation";
      default:
        return "Unknown";
    }
  };

  const formatCurrency = (amount?: number) => {
    if (!amount) return "Not specified";
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: "USD",
    }).format(amount);
  };

  const formatDate = (date?: Date | null) => {
    if (!date) return "Not specified";
    return new Date(date).toLocaleDateString("en-US", {
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-8">
        <Spinner aria-label="Loading project details" size="lg" />
      </div>
    );
  }

  if (error || !project) {
    return (
      <div className="text-center py-8">
        <p className="text-red-500 dark:text-red-400">
          {error || "Project not found"}
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Project Header */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-6">
        <div className="flex flex-col sm:flex-row sm:items-start sm:justify-between mb-4">
          <div>
            <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">
              {project.subject}
            </h2>
            <div className="flex items-center space-x-4">
              {getStatusBadge(project.status)}
              <span className="text-sm text-gray-500 dark:text-gray-400">
                {getProjectTypeText(project.type)}
              </span>
            </div>
          </div>
          <div className="mt-4 sm:mt-0 text-sm text-gray-500 dark:text-gray-400">
            <div>Created: {formatDate(project.createdAt)}</div>
            <div>Updated: {formatDate(project.updatedAt)}</div>
          </div>
        </div>

        {project.description && (
          <p className="text-gray-700 dark:text-gray-300 leading-relaxed">
            {project.description}
          </p>
        )}
      </div>

      {/* Project Details Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {/* Location Information */}
        <Card>
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 flex items-center">
            <HiLocationMarker className="w-5 h-5 mr-2 text-blue-500" />
            Location Information
          </h3>
          <div className="space-y-3">
            <div>
              <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                City/Area
              </label>
              <p className="text-gray-900 dark:text-white">{project.city}</p>
            </div>
            {project.location && (
              <div>
                <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                  Location
                </label>
                <p className="text-gray-900 dark:text-white">
                  {project.location}
                </p>
              </div>
            )}
            {project.locationNote && (
              <div>
                <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                  Location Notes
                </label>
                <p className="text-gray-900 dark:text-white">
                  {project.locationNote}
                </p>
              </div>
            )}
            {project.latitude && project.longitude && (
              <div>
                <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                  Coordinates
                </label>
                <p className="text-gray-900 dark:text-white">
                  {project.latitude}, {project.longitude}
                </p>
              </div>
            )}
          </div>
        </Card>

        {/* Financial Information */}
        <Card>
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 flex items-center">
            <HiCurrencyDollar className="w-5 h-5 mr-2 text-green-500" />
            Financial Information
          </h3>
          <div className="space-y-3">
            <div>
              <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                Estimated Cost
              </label>
              <p className="text-xl font-semibold text-gray-900 dark:text-white">
                {formatCurrency(project.estimatedCost)}
              </p>
            </div>
          </div>
        </Card>

        {/* Timeline Information */}
        <Card>
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 flex items-center">
            <HiCalendar className="w-5 h-5 mr-2 text-purple-500" />
            Timeline Information
          </h3>
          <div className="space-y-3">
            <div>
              <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                Project Start Date
              </label>
              <p className="text-gray-900 dark:text-white">
                {formatDate(project.startDate)}
              </p>
            </div>
            <div>
              <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                Project End Date
              </label>
              <p className="text-gray-900 dark:text-white">
                {formatDate(project.endDate)}
              </p>
            </div>
            <div>
              <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                Tender Opening Date
              </label>
              <p className="text-gray-900 dark:text-white">
                {formatDate(project.tenderOpeningDate)}
              </p>
            </div>
            <div>
              <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                Tender Closing Date
              </label>
              <p className="text-gray-900 dark:text-white">
                {formatDate(project.tenderClosingDate)}
              </p>
            </div>
          </div>
        </Card>

        {/* Documents */}
        <Card>
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 flex items-center">
            <HiDocument className="w-5 h-5 mr-2 text-blue-500" />
            Documents
          </h3>
          <div className="space-y-3">
            <div>
              <label className="text-sm font-medium text-gray-500 dark:text-gray-400">
                Specification Document
              </label>
              {project.specificationDocument ? (
                <a
                  href={
                    typeof project.specificationDocument === "string"
                      ? project.specificationDocument
                      : "#"
                  }
                  className="text-blue-600 dark:text-blue-400 hover:underline block"
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  View Document
                </a>
              ) : (
                <p className="text-gray-500 dark:text-gray-400">
                  No document available
                </p>
              )}
            </div>
          </div>
        </Card>
      </div>

      {/* Statistics */}
      {(project.tenders || project.complains) && (
        <Card>
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">
            Project Statistics
          </h3>
          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <div className="text-center">
              <p className="text-2xl font-bold text-blue-600 dark:text-blue-400">
                {project.tenders?.length || 0}
              </p>
              <p className="text-sm text-gray-500 dark:text-gray-400">
                Total Tenders
              </p>
            </div>
            <div className="text-center">
              <p className="text-2xl font-bold text-red-600 dark:text-red-400">
                {project.complains?.length || 0}
              </p>
              <p className="text-sm text-gray-500 dark:text-gray-400">
                Total Complaints
              </p>
            </div>
            <div className="text-center">
              <p className="text-2xl font-bold text-green-600 dark:text-green-400">
                {project.status === ProjectStatus.Completed ? "100" : "0"}%
              </p>
              <p className="text-sm text-gray-500 dark:text-gray-400">
                Completion Rate
              </p>
            </div>
          </div>
        </Card>
      )}
    </div>
  );
}
