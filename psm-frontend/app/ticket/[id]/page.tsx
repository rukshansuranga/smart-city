import { getTicketById } from "@/app/api/actions/ticketActions";
import { Ticket } from "@/types";

import ManageTicket from "../ManageTicket";

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
      <ManageTicket ticket={ticket} />
    </div>
  );
}
