"use server";

import { fetchWrapper } from "@/lib/fetchWrapper";
import { ActiveLightPostMarker, Paging, Complain, ApiResponse } from "@/types";

export async function getComplainPaging(complainPaging: {
  status: string;
  pageSize: string;
  pageIndex: string;
  duration: string;
  complainType?: string;
}): Promise<ApiResponse<Paging<Complain>>> {
  const queryString = new URLSearchParams(complainPaging).toString();

  return fetchWrapper.get(`complain?${queryString}`);
}

export async function getComplainByTicketId(
  ticketId: number
): Promise<ApiResponse<Complain[]>> {
  return fetchWrapper.get(`complain/ticket/${ticketId}`);
}

export async function manageMappingByTicketAndPackage(managePackage: {
  ticketId: number;
  complainId: number;
  action: string;
}): Promise<ApiResponse<null>> {
  return fetchWrapper.post(`complain/ticket`, managePackage);
}

export async function getComplainById(
  complainId: string,
  type: string
): Promise<ApiResponse<Complain>> {
  return fetchWrapper.get(`complain/${type}/${complainId}`);
}

export async function getActiveLightPost(): Promise<
  ApiResponse<ActiveLightPostMarker[]>
> {
  return fetchWrapper.get(`complain/lightpost/active`);
}

export async function getActiveAndAssignedLightPost(): Promise<
  ApiResponse<ActiveLightPostMarker[]>
> {
  return fetchWrapper.get(`complain/lightpost/assigned`);
}

export async function getLightPostByLocation(managePackage: {
  latitude: number;
  longitude: number;
  statuses: number[];
}): Promise<ApiResponse<ActiveLightPostMarker[]>> {
  return fetchWrapper.post(
    `complain/lightpost/getpointincircle`,
    managePackage
  );
}
