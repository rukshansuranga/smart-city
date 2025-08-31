import { ApiResponse, Paging, Project } from "@/types";
import { fetchWrapperClient } from "@/lib/fetchWrapperClient";

export async function getProject(id: string): Promise<ApiResponse<Project>> {
  return fetchWrapperClient.get(`project/${id}`);
}

export async function getAllProjects(): Promise<Project[]> {
  return fetchWrapperClient.get(`project/all`);
}

export async function filterProjects(
  query: string
): Promise<ApiResponse<Paging<Project>>> {
  return fetchWrapperClient.get(`project${query}`);
}

export async function createProject(
  project: FormData | Record<string, unknown>
): Promise<Project> {
  console.log("Creating project with data:", project);
  return fetchWrapperClient.postFormData("project", project);
}

export async function updateProject(
  id: string,
  project: FormData | Record<string, unknown>
): Promise<Project> {
  return fetchWrapperClient.putFormData(`project/${id}`, project);
}

export async function deleteProject(id: string): Promise<void> {
  return fetchWrapperClient.del(`project/${id}`);
}

export async function getProjectsByContractor(
  contractorId: string
): Promise<Project[]> {
  return fetchWrapperClient.get(`project/contractor/${contractorId}`);
}

export async function getProjectById(
  id: string
): Promise<ApiResponse<Project>> {
  return fetchWrapperClient.get(`project/${id}`);
}
