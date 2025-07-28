import { Ticket } from "@/types";
import TicketForm from "./TicketForm";
import WorkpackageAssigned from "./WorkpackageAssigned";

export default async function ManageTicket({ ticket }: { ticket: Ticket }) {
  return (
    <div>
      <TicketForm ticket={ticket} isInternal={ticket.type === "Internal"} />
      {ticket.type == "External" && (
        <WorkpackageAssigned ticketId={ticket.ticketId!} />
      )}
    </div>
  );
}
