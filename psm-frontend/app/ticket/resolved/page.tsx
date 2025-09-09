import { getResolvedTickets } from "@/app/api/actions/ticketActions";
import ResolvedTicketsClient from "./ResolvedTicketsClient";

export default async function ResolvedTicketsPage() {
  const response = await getResolvedTickets();

  if (!response.isSuccess) {
    return <div>Error: {response.message}</div>;
  }

  const tickets = response.data;

  return <ResolvedTicketsClient tickets={tickets} />;
}
