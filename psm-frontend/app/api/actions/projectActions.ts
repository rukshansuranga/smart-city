import { fetchWrapper } from "@/lib/fetchWrapper";
import { Paging, Project } from "@/types";

export async function filterProjects(query: string): Promise<Paging<Project>> {
  return fetchWrapper.get(`project${query}`);
}

// Add this function to post a new project
export async function postProject(project: Partial<Project>): Promise<Project> {
  return fetchWrapper.post("project", project);
}

export async function getProjectById(id: string): Promise<Project> {
  return fetchWrapper.get(`project/${id}`);
}

export async function getAllProjects(): Promise<Project[]> {
  return fetchWrapper.get(`project/all`);
}

export async function updateProject(
  id: string,
  project: Partial<Project>
): Promise<Project> {
  return fetchWrapper.put(`project/${id}`, project);
}
