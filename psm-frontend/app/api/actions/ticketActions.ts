"use server";
import { fetchWrapper } from "@/lib/fetchWrapper";
import { Paging, Ticket } from "@/types";

export async function createTicket(
  ticket: Ticket | { workpackageIdList: string[] }
): Promise<Ticket> {
  //console.log("ticket action 52", ticket);
  return fetchWrapper.post(`ticket`, ticket);
}

export async function getTicketPaging(params: {
  pageSize: string;
  pageIndex: string;
}): Promise<Paging<Ticket>> {
  const queryString = new URLSearchParams(params).toString();
  console.log("queryString", queryString, params);
  return fetchWrapper.get(`ticket?${queryString}`);
}

export async function getTicketById(id: string): Promise<Ticket> {
  return fetchWrapper.get(`ticket/${id}`);
}

export async function updateTicket(ticket: Partial<Ticket>): Promise<Ticket> {
  console.log("ticket action 52", ticket);
  return fetchWrapper.put(`ticket/${ticket?.ticketId}`, ticket);
}
