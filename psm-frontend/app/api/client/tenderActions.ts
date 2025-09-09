import { fetchWrapperClient } from "@/lib/fetchWrapperClient";
import { Paging, Tender, ApiResponse } from "@/types";

export async function filterTender(
  query: string
): Promise<ApiResponse<Paging<Tender>>> {
  return fetchWrapperClient.get(`tender${query}`);
}

// Add this function to post a new project
export async function postTender(
  tender: FormData | Record<string, unknown>
): Promise<ApiResponse<Tender>> {
  return fetchWrapperClient.postFormData("tender", tender);
}

export async function getTendersByProjectIdId(
  id: string
): Promise<ApiResponse<Tender[]>> {
  return fetchWrapperClient.get(`tender/project/${id}`);
}

export async function getTendersId(id: string): Promise<ApiResponse<Tender>> {
  return fetchWrapperClient.get(`tender/${id}`);
}

export async function getTenderByTenderId(
  id: string
): Promise<ApiResponse<Tender>> {
  return fetchWrapperClient.get(`tender/${id}`);
}

export async function updateTender(
  id: string,
  tender: FormData | Record<string, unknown>
): Promise<ApiResponse<Tender>> {
  return fetchWrapperClient.putFormData(`tender/${id}`, tender);
}

export async function deleteTender(id: string): Promise<ApiResponse<null>> {
  return fetchWrapperClient.del(`tender/${id}`);
}
