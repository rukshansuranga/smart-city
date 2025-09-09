import { getTicketById } from "@/app/api/actions/ticketActions";

import ManageTicket from "../ManageTicket";

export default async function TicketById({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  const response = await getTicketById(id);

  if (!response.isSuccess) {
    return <div>Error: {response.message}</div>;
  }

  const ticket = response.data;

  return (
    <div>
      <ManageTicket ticket={ticket} />
    </div>
  );
}
