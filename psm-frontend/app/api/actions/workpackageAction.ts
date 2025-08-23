"use server";

import { TicketWorkpackageType } from "@/enums";
import { fetchWrapper } from "@/lib/fetchWrapper";
import { ActiveLightPostMarker, Paging, Workpackage } from "@/types";

export async function getWorkpackagePaging(complainPaging: {
  status: string;
  pageSize: string;
  pageIndex: string;
  duration: string;
  type?: string;
  ticketWorkpackageType?: string;
}): Promise<Paging<Workpackage>> {
  const queryString = new URLSearchParams(complainPaging).toString();

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

export async function getWorkpackageById(
  workpackageId: string,
  type: string
): Promise<Workpackage> {
  return fetchWrapper.get(`workpackage/${type}/${workpackageId}`);
}

export async function getActiveLightPost(): Promise<ActiveLightPostMarker[]> {
  return fetchWrapper.get(`workpackage/lightpost/active`);
}

export async function getActiveAndAssignedLightPost(): Promise<
  ActiveLightPostMarker[]
> {
  return fetchWrapper.get(`workpackage/lightpost/assigned`);
}
