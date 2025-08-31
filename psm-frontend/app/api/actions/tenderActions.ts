import { fetchWrapper } from "@/lib/fetchWrapper";
import { Paging, Tender, ApiResponse } from "@/types";

export async function filterTender(
  query: string
): Promise<ApiResponse<Paging<Tender>>> {
  return fetchWrapper.get(`tender${query}`);
}

export async function postTender(tender: Tender): Promise<ApiResponse<Tender>> {
  return fetchWrapper.post("tender", tender);
}

export async function getTendersByProjectIdId(
  id: string
): Promise<ApiResponse<Tender[]>> {
  return fetchWrapper.get(`tender/project/${id}`);
}

export async function getTendersId(id: string): Promise<ApiResponse<Tender>> {
  return fetchWrapper.get(`tender/${id}`);
}

export async function getTenderByTenderId(
  id: string
): Promise<ApiResponse<Tender>> {
  return fetchWrapper.get(`tender/${id}`);
}

export async function updateTender(
  id: string,
  tender: Tender
): Promise<ApiResponse<Tender>> {
  return fetchWrapper.put(`tender/${id}`, tender);
}
