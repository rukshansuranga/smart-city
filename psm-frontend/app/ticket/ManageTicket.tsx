import { ComplainTicket, Ticket } from "@/types";
import TicketForm from "../components/ticket/TicketForm";
import WorkpackageAssigned from "./WorkpackageAssigned";
import { ComplainType, TicketType } from "@/enums";
import LightPostWorkpackageAssigned from "./LightPostWorkpackageAssigned";

export default async function ManageTicket({ ticket }: { ticket: Ticket }) {
  // Determine the ticket type based on available data
  const ticketType = ticket.type || TicketType.InternalTicket;

  // Type assertion to access properties that might exist on specific ticket types
  let complainType = ComplainType.GeneralComplain;

  if (ticketType === TicketType.ComplainTicket) {
    complainType =
      (ticket as ComplainTicket)?.complainType || ComplainType.GeneralComplain;
  }

  return (
    <div>
      <TicketForm ticket={ticket} ticketType={ticketType} />

      {ticket.type == TicketType.ComplainTicket &&
        (complainType == ComplainType.GeneralComplain ||
          complainType == ComplainType.ProjectComplain) && (
          <WorkpackageAssigned
            ticketId={ticket.ticketId!}
            complainType={complainType}
          />
        )}

      {ticket.type == TicketType.ComplainTicket &&
        complainType == ComplainType.LightPostComplain && (
          <LightPostWorkpackageAssigned ticketId={ticket.ticketId!} />
        )}
    </div>
  );
}
