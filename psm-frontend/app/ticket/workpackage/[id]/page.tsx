import { getTicketListByWorkpackageId } from "@/app/api/actions/ticketActions";
import { Button } from "flowbite-react";
import Link from "next/link";
import ManageTicket from "../../ManageTicket";

export default async function TicketsByWorkpackagePage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  const ticketList = await getTicketListByWorkpackageId(id);

  //console.log("ticketList", ticketList);

  if (ticketList.length === 1) {
    return <ManageTicket ticket={ticketList[0]} />;
  }

  return (
    <div>
      <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
        <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
          <tr>
            <th scope="col" className="px-6 py-3">
              ID
            </th>
            <th scope="col" className="px-6 py-3">
              Name
            </th>
            <th scope="col" className="px-6 py-3">
              Created Date
            </th>
            <th scope="col" className="px-6 py-3">
              User
            </th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {ticketList.map((ticket) => (
            <tr
              key={ticket.ticketId}
              className="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 border-gray-200"
            >
              <th
                scope="row"
                className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
              >
                {ticket.ticketId}
              </th>
              <td className="px-6 py-4">{ticket.subject}</td>
              <td className="px-6 py-4">
                {new Date(ticket.createdAt!).toISOString().slice(0, 10)}
              </td>
              <td className="px-6 py-4">{ticket.user?.firstName}</td>
              <td>
                <Button>
                  <Link
                    href={`/ticket/${ticket.ticketId}`}
                    className="text-white"
                  >
                    View Ticket
                  </Link>
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
