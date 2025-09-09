import { fetchWrapperClient } from "@/lib/fetchWrapperClient";
import { ApiResponse, Coordinator, Paging } from "@/types";

// Create new project coordinator
export async function createCoordinator(
  coordinator: Partial<Coordinator>
): Promise<ApiResponse<Coordinator>> {
  console.log("Creating coordinator:", coordinator);
  return fetchWrapperClient.post("project/coordinator", coordinator);
}

// Get all project coordinators
export async function getAllCoordinators(): Promise<
  ApiResponse<Paging<Coordinator>>
> {
  return fetchWrapperClient.get("project/coordinator");
}

// Get coordinator by ID
export async function getCoordinatorById(
  id: number
): Promise<ApiResponse<Coordinator>> {
  return fetchWrapperClient.get(`project/coordinator/${id}`);
}

// Get coordinators by project ID
export async function getCoordinatorsByProjectId(
  projectId: string | number
): Promise<ApiResponse<Coordinator[]>> {
  return fetchWrapperClient.get(`project/${projectId}/coordinator`);
}

// Update project coordinator
export async function updateCoordinator(
  id: number,
  coordinator: Partial<Coordinator>
): Promise<ApiResponse<Coordinator>> {
  console.log("Updating coordinator:", id, coordinator);
  return fetchWrapperClient.put(`project/coordinator/${id}`, coordinator);
}

// Delete project coordinator
export async function deleteCoordinator(
  id: number
): Promise<ApiResponse<void>> {
  console.log("Deleting coordinator:", id);
  return fetchWrapperClient.del(`project/coordinator/${id}`);
}

// Get coordinators with filtering and pagination
export async function filterCoordinators(
  query: string
): Promise<ApiResponse<Paging<Coordinator>>> {
  console.log("Filtering coordinators:", query);
  return fetchWrapperClient.get(`project/coordinator${query}`);
}
