import { getTicketById } from "@/app/api/actions/ticketActions";
import { Ticket } from "@/types";
import TicketForm from "./TicketForm";
import WorkpackageAssigned from "./WorkpackageAssigned";

export default async function TicketById({ params }) {
  const { id } = params;

  const ticket: Ticket = await getTicketById(id);

  console.log(45, id, ticket);

  return (
    <div>
      <TicketForm ticket={ticket} />
      <WorkpackageAssigned ticketId={ticket.ticketId!} />
    </div>
  );
}
