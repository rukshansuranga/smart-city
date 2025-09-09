"use server";
import { fetchWrapper } from "@/lib/fetchWrapper";
import {
  Paging,
  Ticket,
  UpdateTicketPayload,
  ApiResponse,
  BoardTicket,
} from "@/types";

export async function createTicket(
  ticket: Ticket | { ComplainIdList: string[] }
): Promise<ApiResponse<Ticket>> {
  console.log("ticket creation action", ticket);
  return fetchWrapper.post(`ticket/complain`, ticket);
}

export async function createInternalTicket(
  ticket: Ticket
): Promise<ApiResponse<Ticket>> {
  //console.log("ticket action 52", ticket);
  return fetchWrapper.post(`ticket/internal`, ticket);
}

export async function getTicketPaging(params: {
  pageSize: string;
  pageIndex: string;
}): Promise<ApiResponse<Paging<Ticket>>> {
  const queryString = new URLSearchParams(params).toString();
  console.log("queryString", queryString, params);
  return fetchWrapper.get(`ticket?${queryString}`);
}

export async function getTicketListByWorkpackageId(
  id: string
): Promise<ApiResponse<Ticket[]>> {
  return fetchWrapper.get(`ticket/complain/${id}`);
}

export async function getTicketById(id: string): Promise<ApiResponse<Ticket>> {
  return fetchWrapper.get(`ticket/${id}`);
}

export async function getTicketByIdAndType(
  id: string,
  type: string
): Promise<ApiResponse<Ticket>> {
  return fetchWrapper.get(`ticket/${id}/${type}`);
}

export async function updateTicket(
  ticket: Partial<Ticket>
): Promise<ApiResponse<Ticket>> {
  //console.log("ticket action 52", ticket);
  return fetchWrapper.put(`ticket/${ticket?.ticketId}`, ticket);
}

export async function getTicketsByUserId(
  id: string
): Promise<ApiResponse<BoardTicket>> {
  return fetchWrapper.get(`ticket/user/${id}`);
}

export async function startTicket(
  ticketId: string
): Promise<ApiResponse<boolean>> {
  return fetchWrapper.get(`ticket/start/${ticketId}`);
}

export async function resolveTicket(
  ticketId: string
): Promise<ApiResponse<boolean>> {
  return fetchWrapper.get(`ticket/resolve/${ticketId}`);
}

export async function closeTicket(
  ticketId: string
): Promise<ApiResponse<boolean>> {
  return fetchWrapper.get(`ticket/close/${ticketId}`);
}

export async function getResolvedTickets(): Promise<ApiResponse<Ticket[]>> {
  return fetchWrapper.get(`ticket/resolve`);
}

export async function addWorkpackagesToTicket(
  updateTicketPayload: UpdateTicketPayload
): Promise<ApiResponse<null>> {
  //console.log("ticket action 52", ticket);
  return fetchWrapper.post(`ticket/addworkpackages`, updateTicketPayload);
}

export async function removeWorkpackagesFromTicket(
  updateTicketPayload: UpdateTicketPayload
): Promise<ApiResponse<null>> {
  //console.log("ticket action 52", ticket);
  return fetchWrapper.post(`ticket/removeworkpackages`, updateTicketPayload);
}
