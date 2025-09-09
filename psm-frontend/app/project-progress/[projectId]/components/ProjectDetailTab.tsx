"use client";

import React from "react";
import { Project } from "@/types";
import { ProjectStatus, ProjectType } from "@/enums";

interface ProjectDetailTabProps {
  projectId: string;
  project: Project;
}

export function ProjectDetailTab({ project }: ProjectDetailTabProps) {
  const getStatusColor = (status: ProjectStatus) => {
    switch (status) {
      case ProjectStatus.Completed:
        return "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300";
      case ProjectStatus.InProgress:
        return "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-300";
      case ProjectStatus.OnHold:
        return "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300";
      default:
        return "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300";
    }
  };

  return (
    <div className="space-y-6">
      {/* Basic Information */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 p-6">
        <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">
          Basic Information
        </h3>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Project Name
            </label>
            <div className="text-gray-900 dark:text-white font-medium">
              {project.subject}
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Status
            </label>
            <span
              className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getStatusColor(
                project.status
              )}`}
            >
              {ProjectStatus[project.status]}
            </span>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Project Type
            </label>
            <div className="text-gray-900 dark:text-white">
              {ProjectType[project.type]}
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              City
            </label>
            <div className="text-gray-900 dark:text-white">{project.city}</div>
          </div>

          {project.estimatedCost && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Estimated Cost
              </label>
              <div className="text-gray-900 dark:text-white font-medium">
                ${project.estimatedCost.toLocaleString()}
              </div>
            </div>
          )}

          {project.location && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Location
              </label>
              <div className="text-gray-900 dark:text-white">
                {project.location}
              </div>
            </div>
          )}
        </div>

        {project.description && (
          <div className="mt-6">
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
              Description
            </label>
            <div className="text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-700 p-4 rounded-md">
              {project.description}
            </div>
          </div>
        )}

        {project.locationNote && (
          <div className="mt-4">
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
              Location Notes
            </label>
            <div className="text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-700 p-4 rounded-md">
              {project.locationNote}
            </div>
          </div>
        )}
      </div>

      {/* Timeline Information */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 p-6">
        <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">
          Timeline Information
        </h3>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {project.startDate && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Start Date
              </label>
              <div className="text-gray-900 dark:text-white">
                {new Date(project.startDate).toLocaleDateString()}
              </div>
            </div>
          )}

          {project.endDate && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                End Date
              </label>
              <div className="text-gray-900 dark:text-white">
                {new Date(project.endDate).toLocaleDateString()}
              </div>
            </div>
          )}

          {project.tenderOpeningDate && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Tender Opening Date
              </label>
              <div className="text-gray-900 dark:text-white">
                {new Date(project.tenderOpeningDate).toLocaleDateString()}
              </div>
            </div>
          )}

          {project.tenderClosingDate && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Tender Closing Date
              </label>
              <div className="text-gray-900 dark:text-white">
                {new Date(project.tenderClosingDate).toLocaleDateString()}
              </div>
            </div>
          )}

          {project.createdAt && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Created Date
              </label>
              <div className="text-gray-900 dark:text-white">
                {new Date(project.createdAt).toLocaleDateString()}
              </div>
            </div>
          )}

          {project.updatedAt && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Last Updated
              </label>
              <div className="text-gray-900 dark:text-white">
                {new Date(project.updatedAt).toLocaleDateString()}
              </div>
            </div>
          )}
        </div>
      </div>

      {/* Location Information */}
      {project.latitude && project.longitude && (
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 p-6">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">
            Geographic Location
          </h3>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Latitude
              </label>
              <div className="text-gray-900 dark:text-white font-mono">
                {project.latitude}
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Longitude
              </label>
              <div className="text-gray-900 dark:text-white font-mono">
                {project.longitude}
              </div>
            </div>
          </div>

          {/* You could add a map component here if needed */}
          <div className="mt-4">
            <a
              href={`https://www.google.com/maps?q=${project.latitude},${project.longitude}`}
              target="_blank"
              rel="noopener noreferrer"
              className="inline-flex items-center px-3 py-2 text-sm font-medium text-blue-600 bg-blue-50 border border-blue-200 rounded-md hover:bg-blue-100 dark:bg-blue-900/20 dark:border-blue-800 dark:text-blue-400 dark:hover:bg-blue-900/30"
            >
              View on Google Maps
              <svg
                className="w-4 h-4 ml-2"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M10 6H6a2 2 0 00-2 2v10a2 2 0 002 2h10a2 2 0 002-2v-4M14 4h6m0 0v6m0-6L10 14"
                />
              </svg>
            </a>
          </div>
        </div>
      )}

      {/* Documents */}
      {project.specificationDocument && (
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 p-6">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">
            Documents
          </h3>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
              Specification Document
            </label>
            <div className="flex items-center p-3 bg-gray-50 dark:bg-gray-700 rounded-md">
              <svg
                className="w-5 h-5 text-gray-400 mr-2"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
                />
              </svg>
              <span className="text-gray-900 dark:text-white">
                {typeof project.specificationDocument === "string"
                  ? project.specificationDocument
                  : "Document attached"}
              </span>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
