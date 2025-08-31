"use server";

import { fetchWrapper } from "@/lib/fetchWrapper";
import { ActiveLightPostMarker, Paging, Complain } from "@/types";

export async function getWorkpackagePaging(complainPaging: {
  status: string;
  pageSize: string;
  pageIndex: string;
  duration: string;
  type?: string;
  ticketWorkpackageType?: string;
}): Promise<Paging<Complain>> {
  const queryString = new URLSearchParams(complainPaging).toString();

  return fetchWrapper.get(`complain?${queryString}`);
}

export async function getWorkpackageByTicketId(
  ticketId: number
): Promise<Complain[]> {
  return fetchWrapper.get(`complain/ticket/${ticketId}`);
}

export async function manageMappingByTicketAndPackage(managePackage: {
  ticketId: number;
  complainId: number;
  action: string;
}) {
  return fetchWrapper.post(`complain/ticket`, managePackage);
}

export async function getWorkpackageById(
  complainId: string,
  type: string
): Promise<Complain> {
  return fetchWrapper.get(`complain/${type}/${complainId}`);
}

export async function getActiveLightPost(): Promise<ActiveLightPostMarker[]> {
  return fetchWrapper.get(`complain/lightpost/active`);
}

export async function getActiveAndAssignedLightPost(): Promise<
  ActiveLightPostMarker[]
> {
  return fetchWrapper.get(`complain/lightpost/assigned`);
}

export async function getLightPostByLocation(managePackage: {
  latitude: number;
  longitude: number;
  statuses: number[];
}): Promise<ActiveLightPostMarker[]> {
  return fetchWrapper.post(
    `complain/lightpost/getpointincircle`,
    managePackage
  );
}
