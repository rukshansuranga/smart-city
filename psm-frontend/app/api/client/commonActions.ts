import { fetchWrapperClient } from "@/lib/fetchWrapperClient";
import { Company } from "@/types";

export async function getCompanies(): Promise<Company[]> {
  return fetchWrapperClient.get(`misc/companies`);
}
