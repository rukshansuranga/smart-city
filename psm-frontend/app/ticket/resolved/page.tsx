import { getResolvedTickets } from "@/app/api/actions/ticketActions";
import ResolvedTicketsClient from "./ResolvedTicketsClient";

export default async function ResolvedTicketsPage() {
  const tickets = await getResolvedTickets();
  return <ResolvedTicketsClient tickets={tickets} />;
}
