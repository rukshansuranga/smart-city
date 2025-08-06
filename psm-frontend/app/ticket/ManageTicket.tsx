import { Ticket } from "@/types";
import TicketForm from "./TicketForm";
import WorkpackageAssigned from "./WorkpackageAssigned";
import { TicketType } from "@/enums";

export default async function ManageTicket({ ticket }: { ticket: Ticket }) {
  return (
    <div>
      <TicketForm
        ticket={ticket}
        isInternal={ticket.type === TicketType.Internal}
      />
      {ticket.type == TicketType.External && (
        <WorkpackageAssigned ticketId={ticket.ticketId!} />
      )}
    </div>
  );
}
