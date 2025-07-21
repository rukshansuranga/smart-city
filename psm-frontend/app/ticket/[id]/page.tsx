import { getTicketById } from "@/app/api/actions/ticketActions";
import { Ticket } from "@/types";
import TicketForm from "./TicketForm";
import WorkpackageAssigned from "./WorkpackageAssigned";

// type Props = {
//   params: Promise<{ id: string }>;
// };

export default async function TicketById({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  const ticket: Ticket = await getTicketById(id);

  console.log(45, id, ticket);

  return (
    <div>
      <TicketForm ticket={ticket} />
      <WorkpackageAssigned ticketId={ticket.ticketId!} />
    </div>
  );
}
