import { fetchWrapperClient } from "@/lib/fetchWrapperClient";
import { ProjectProgress, Paging, ApiResponse } from "@/types";

export async function getProjectProgressList(
  query?: string
): Promise<ApiResponse<Paging<ProjectProgress>>> {
  console.log("Getting project progress list", query);
  const endpoint = query ? `projectprogress${query}` : "projectprogress";
  return fetchWrapperClient.get(endpoint);
}

export async function getProjectProgressById(
  id: string
): Promise<ApiResponse<ProjectProgress>> {
  return fetchWrapperClient.get(`projectprogress/${id}`);
}

export async function addProjectProgress(
  progressData: Partial<ProjectProgress>
): Promise<ApiResponse<ProjectProgress>> {
  console.log("Adding project progress:", progressData);
  return fetchWrapperClient.post("project/projectprogress", progressData);
}

export async function updateProjectProgress(
  id: string,
  progressData: Partial<ProjectProgress>
): Promise<ApiResponse<ProjectProgress>> {
  console.log("Updating project progress:", id, progressData);
  return fetchWrapperClient.patch(
    `project/projectprogress/${id}`,
    progressData
  );
}

export async function deleteProjectProgress(
  id: string
): Promise<ApiResponse<null>> {
  return fetchWrapperClient.del(`project/projectprogress/${id}`);
}

export async function getProjectProgressByProjectId(
  projectId: string
): Promise<ApiResponse<ProjectProgress[]>> {
  return fetchWrapperClient.get(`project/${projectId}/progress`);
}

export async function approveProjectProgress(
  id: string,
  approvalData: {
    approvedNote?: string;
    isApproved: boolean;
  }
): Promise<ApiResponse<ProjectProgress>> {
  return fetchWrapperClient.put(
    `project/projectprogress/${id}/approve`,
    approvalData
  );
}
