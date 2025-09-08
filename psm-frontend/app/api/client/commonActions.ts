import { fetchWrapperClient } from "@/lib/fetchWrapperClient";
import { Contractor, ApiResponse } from "@/types";

export async function getCompanies(): Promise<ApiResponse<Contractor[]>> {
  return fetchWrapperClient.get(`misc/companies`);
}
