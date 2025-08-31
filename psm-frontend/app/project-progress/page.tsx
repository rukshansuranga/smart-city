"use client";

import React, { useState, useEffect } from "react";
import Link from "next/link";
import { useSession } from "next-auth/react";
import { PageProtection } from "../components/PageProtection";
import { Project } from "@/types";
import { ProjectStatus, ProjectType } from "@/enums";
import { filterProjects } from "../api/client/projectActions";
import { getProjectsByContractor } from "../api/client/projectActions";

function ProjectProgressContent() {
  const { data: session } = useSession();
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchContractorProjects = async () => {
      if (!session?.user?.id) {
        setLoading(false);
        return;
      }

      try {
        setLoading(true);
        // Try the new contractor-specific endpoint first, fall back to filter query
        let response;
        try {
          response = await getProjectsByContractor(session.user.id);
        } catch (error) {
          console.warn(
            "Contractor-specific endpoint not available, using filter:",
            error
          );
          response = await filterProjects(`?contractorId=${session.user.id}`);

          if (!response.isSuccess) {
            setError(response.message || "Failed to load projects");
          }
        }
        setProjects(response?.data || []);
      } catch (err) {
        console.error("Error fetching contractor projects:", err);
        setError("Failed to load projects");
      } finally {
        setLoading(false);
      }
    };

    fetchContractorProjects();
  }, []);

  if (loading) {
    return (
      <div className="p-6">
        <div className="flex items-center justify-center h-64">
          <div className="text-gray-600 dark:text-gray-400">
            Loading projects...
          </div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="p-6">
        <div className="flex items-center justify-center h-64">
          <div className="text-red-600 dark:text-red-400">{error}</div>
        </div>
      </div>
    );
  }

  return (
    <div className="p-6">
      <div className="mb-6">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
          My Projects Progress
        </h1>
        <p className="text-gray-600 dark:text-gray-400">
          Manage progress for projects under your responsibility
        </p>
      </div>

      {projects.length === 0 ? (
        <div className="text-center py-12">
          <div className="text-gray-500 dark:text-gray-400 mb-4">
            No projects assigned to you yet.
          </div>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {projects.map((project) => (
            <div
              key={project.id}
              className="bg-white dark:bg-gray-800 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300"
            >
              <div className="p-6">
                <div className="flex items-start justify-between mb-4">
                  <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 line-clamp-2">
                    {project.subject}
                  </h3>
                  <span
                    className={`px-2 py-1 text-xs font-medium rounded-full ${
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

                {project.description && (
                  <p className="text-gray-600 dark:text-gray-400 text-sm mb-4 line-clamp-3">
                    {project.description}
                  </p>
                )}

                <div className="space-y-2 mb-4">
                  <div className="flex items-center text-sm text-gray-500 dark:text-gray-400">
                    <span className="font-medium mr-2">City:</span>
                    <span>{project.city}</span>
                  </div>
                  <div className="flex items-center text-sm text-gray-500 dark:text-gray-400">
                    <span className="font-medium mr-2">Type:</span>
                    <span>{ProjectType[project.type]}</span>
                  </div>
                  {project.estimatedCost && (
                    <div className="flex items-center text-sm text-gray-500 dark:text-gray-400">
                      <span className="font-medium mr-2">Budget:</span>
                      <span>${project.estimatedCost.toLocaleString()}</span>
                    </div>
                  )}
                </div>

                <Link
                  href={`/project-progress/${project.id}`}
                  className="inline-flex items-center justify-center w-full px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-transparent rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 dark:bg-blue-500 dark:hover:bg-blue-600 transition-colors duration-200"
                >
                  Manage Progress
                </Link>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default function ProjectProgressPage() {
  return (
    <PageProtection permission="projectprogress">
      <ProjectProgressContent />
    </PageProtection>
  );
}
