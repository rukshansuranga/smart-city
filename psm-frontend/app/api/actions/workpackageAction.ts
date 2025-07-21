"use server";

import { fetchWrapper } from "@/lib/fetchWrapper";
import { Paging, Workpackage } from "@/types";

export async function getWorkpackagePaging(complainPaging: {
  status: string;
  pageSize: string;
  pageIndex: string;
  duration: string;
}): Promise<Paging<Workpackage>> {
  console.log(34, complainPaging);
  const queryString = new URLSearchParams(complainPaging).toString();
  console.log("getWorkpackagePaging queryString:", queryString);
  return fetchWrapper.get(`workpackage?${queryString}`);
}

export async function getWorkpackageByTicketId(
  ticketId: number
): Promise<Workpackage[]> {
  return fetchWrapper.get(`workpackage/ticket/${ticketId}`);
}

export async function manageMappingByTicketAndPackage(managePackage: {
  ticketId: number;
  workpackageId: number;
  action: string;
}) {
  return fetchWrapper.post(`workpackage/ticket`, managePackage);
}
