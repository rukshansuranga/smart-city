import { Ticket } from "@/types";
import TicketForm from "./TicketForm";
import WorkpackageAssigned from "./WorkpackageAssigned";
import { TicketType, TicketWorkpackageType } from "@/enums";
import LightPostWorkpackageAssigned from "./LightPostWorkpackageAssigned";

export default async function ManageTicket({ ticket }: { ticket: Ticket }) {
  console.log("ticket123", ticket);

  return (
    <div>
      <TicketForm
        ticket={ticket}
        workpackageList={ticket?.ticketPackages?.map((pkg) => pkg.complain!)}
        isInternal={ticket.type === TicketType.Internal}
        ticketWorkpackageType={ticket.ticketWorkpackageType}
      />
      {ticket.type == TicketType.External &&
        (ticket.ticketWorkpackageType ==
          TicketWorkpackageType.GeneralComplain ||
          ticket.ticketWorkpackageType ==
            TicketWorkpackageType.ProjectComplain) && (
          <WorkpackageAssigned
            ticketId={ticket.ticketId!}
            ticketWorkpackageType={ticket.ticketWorkpackageType}
          />
        )}

      {ticket.type == TicketType.External &&
        ticket.ticketWorkpackageType ==
          TicketWorkpackageType.LightPostComplain && (
          <LightPostWorkpackageAssigned ticketId={ticket.ticketId!} />
        )}
    </div>
  );
}
