"use client";

import React, { useState, useEffect, useCallback } from "react";
import { ProjectProgress } from "@/types";
import { ProjectProgressApprovedStatus } from "@/enums";
import { getProjectProgressList } from "@/app/api/client/projectProgressActions";

interface ProjectProgressListTabProps {
  loading: boolean;
  onEditProgress: (progress: ProjectProgress) => void;
  refreshTrigger?: number;
}

// Mock data - replace with actual API call
const mockProjectProgressData: ProjectProgress[] = [
  {
    projectProgressId: "1",
    projectId: "1", // Changed to match Project.id
    summary: "Foundation work completed",
    description:
      "All foundation work has been completed successfully. Ready to proceed to next phase.",
    progressDate: "2024-12-01",
    progressPercentage: 25,
    approvedBy: "John Doe",
    approvedAt: new Date("2024-12-02"),
    approvedNote: "Work meets quality standards",
    projectProgressApprovedStatus: ProjectProgressApprovedStatus.Approved,
  },
  {
    projectProgressId: "2",
    projectId: "1", // Changed to match Project.id
    summary: "Structural framework in progress",
    description:
      "Steel framework installation is 50% complete. Weather conditions are favorable.",
    progressDate: "2024-12-15",
    progressPercentage: 50,
    approvedBy: undefined,
    approvedAt: undefined,
    approvedNote: undefined,
    projectProgressApprovedStatus: ProjectProgressApprovedStatus.Pending,
  },
  {
    projectProgressId: "3",
    projectId: "2", // Changed to match Project.id
    summary: "Initial site preparation",
    description:
      "Site clearing and preparation work needs revision due to environmental concerns.",
    progressDate: "2024-11-20",
    progressPercentage: 15,
    approvedBy: "Jane Smith",
    approvedAt: new Date("2024-11-22"),
    approvedNote: "Environmental impact assessment required",
    projectProgressApprovedStatus: ProjectProgressApprovedStatus.Rejected,
  },
];

export default function ProjectProgressListTab({
  loading,
  onEditProgress,
  refreshTrigger,
}: ProjectProgressListTabProps) {
  const [progressList, setProgressList] = useState<ProjectProgress[]>([]);
  const [dataLoading, setDataLoading] = useState(false);

  const loadProgressData = useCallback(async () => {
    if (loading) return;

    setDataLoading(true);
    try {
      // Try to use actual API first, fall back to mock data if it fails
      try {
        const response = await getProjectProgressList();
        if (response.isSuccess && response.data) {
          setProgressList(response.data.records || []);
        } else {
          console.warn("API response not successful:", response.message);
          setProgressList(mockProjectProgressData);
        }
      } catch (apiError) {
        console.warn("API call failed, using mock data:", apiError);
        // Use mock data as fallback
        setProgressList(mockProjectProgressData);
      }
    } catch (error) {
      console.error("Error loading progress data:", error);
      setProgressList([]);
    } finally {
      setDataLoading(false);
    }
  }, [loading]);

  useEffect(() => {
    loadProgressData();
  }, [loadProgressData, refreshTrigger]);

  const getStatusBadge = (status?: ProjectProgressApprovedStatus) => {
    switch (status) {
      case ProjectProgressApprovedStatus.Approved:
        return (
          <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-green-100 text-green-800">
            Approved
          </span>
        );
      case ProjectProgressApprovedStatus.Pending:
        return (
          <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-yellow-100 text-yellow-800">
            Pending
          </span>
        );
      case ProjectProgressApprovedStatus.Rejected:
        return (
          <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-red-100 text-red-800">
            Rejected
          </span>
        );
      default:
        return (
          <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-gray-100 text-gray-800">
            Unknown
          </span>
        );
    }
  };

  const formatDate = (date: Date) => {
    return new Intl.DateTimeFormat("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
    }).format(date);
  };

  if (loading || dataLoading) {
    return (
      <div className="p-6">
        <div className="animate-pulse">
          <div className="h-4 bg-gray-200 rounded w-1/4 mb-4"></div>
          <div className="space-y-3">
            {[...Array(3)].map((_, i) => (
              <div key={i} className="h-16 bg-gray-200 rounded"></div>
            ))}
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50">
          <tr>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Project ID
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Summary
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Progress
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Date
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Status
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Approved By
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Actions
            </th>
          </tr>
        </thead>
        <tbody className="bg-white divide-y divide-gray-200">
          {progressList.map((progress) => (
            <tr key={progress.projectProgressId} className="hover:bg-gray-50">
              <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                {progress.projectId}
              </td>
              <td className="px-6 py-4 whitespace-nowrap">
                <div className="text-sm font-medium text-gray-900">
                  {progress.summary}
                </div>
                <div className="text-sm text-gray-500 truncate max-w-xs">
                  {progress.description}
                </div>
              </td>
              <td className="px-6 py-4 whitespace-nowrap">
                <div className="flex items-center">
                  <div className="w-full bg-gray-200 rounded-full h-2.5 mr-2 max-w-[100px]">
                    <div
                      className="bg-blue-600 h-2.5 rounded-full"
                      style={{ width: `${progress.progressPercentage}%` }}
                    ></div>
                  </div>
                  <span className="text-sm text-gray-600 min-w-[35px]">
                    {progress.progressPercentage}%
                  </span>
                </div>
              </td>
              <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                {formatDate(new Date(progress.progressDate))}
              </td>
              <td className="px-6 py-4 whitespace-nowrap">
                {getStatusBadge(progress.projectProgressApprovedStatus)}
              </td>
              <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                {progress.approvedBy || "-"}
              </td>
              <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                <button
                  onClick={() => onEditProgress(progress)}
                  className="text-indigo-600 hover:text-indigo-900 mr-3"
                >
                  Edit
                </button>
                <button className="text-red-600 hover:text-red-900">
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {progressList.length === 0 && (
        <div className="p-6 text-center text-gray-500">
          No project progress records found.
        </div>
      )}
    </div>
  );
}
