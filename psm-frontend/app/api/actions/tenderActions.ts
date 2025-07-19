import { fetchWrapper } from "@/lib/fetchWrapper";
import { Paging, Tender } from "@/types";

export async function filterTender(query: string): Promise<Paging<Tender>> {
  return fetchWrapper.get(`tender${query}`);
}

// Add this function to post a new project
export async function postTender(tender: Tender): Promise<Tender> {
  return fetchWrapper.post("tender", tender);
}

export async function getTendersByProjectIdId(id: string): Promise<Tender[]> {
  return fetchWrapper.get(`tender/project/${id}`);
}

export async function getTendersId(id: string): Promise<Tender> {
  return fetchWrapper.get(`tender/${id}`);
}

export async function getTenderByTenderId(id: string): Promise<Tender> {
  return fetchWrapper.get(`tender/${id}`);
}

export async function updateTender(
  id: string,
  tender: Tender
): Promise<Tender> {
  return fetchWrapper.put(`tender/${id}`, tender);
}
