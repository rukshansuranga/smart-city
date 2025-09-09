import TicketForm from "../../components/ticket/TicketForm";
import { TicketType } from "@/enums";

export default function AddTicketPage() {
  return (
    <div>
      <TicketForm ticketType={TicketType.InternalTicket} />
    </div>
  );
}
