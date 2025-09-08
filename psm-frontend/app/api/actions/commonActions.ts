import { fetchWrapper } from "@/lib/fetchWrapper";
import { Contractor, ApiResponse } from "@/types";

export async function getCompanies(): Promise<ApiResponse<Contractor[]>> {
  return fetchWrapper.get(`misc/companies`);
}
