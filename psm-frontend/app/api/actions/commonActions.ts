import { fetchWrapper } from "@/lib/fetchWrapper";
import { Company } from "@/types";

export async function getCompanies(): Promise<Company[]> {
  return fetchWrapper.get(`misc/companies`);
}
