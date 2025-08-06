import { Paging, Ticket } from "@/types";
import { getTicketPaging } from "../api/actions/ticketActions";
import Link from "next/link";

export default async function TicketList({
  searchParams,
}: {
  searchParams: Promise<{ pageIndex?: string; pageSize?: string }>;
}) {
  const { pageIndex = "1", pageSize = "5" } = await searchParams;

  const response: Paging<Ticket> = await getTicketPaging({
    pageIndex,
    pageSize,
  });

  console.log("response11", response?.records, response?.totalItems);

  return (
    <div className="flex-col ">
      <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
        <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
          <tr>
            <th scope="col" className="px-6 py-3">
              ID
            </th>
            <th scope="col" className="px-6 py-3">
              Title
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
          {response?.records?.map((ticket) => (
            <tr key={ticket.ticketId}>
              <td className="px-6 py-4">{ticket.ticketId}</td>
              <td className="px-6 py-4">{ticket.subject}</td>
              <td className="px-6 py-4">
                {new Date(ticket.createdAt!)?.toISOString().slice(0, 10)}
              </td>
              <td className="px-6 py-4">{ticket.user?.firstName}</td>
              <td>
                <Link
                  className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
                  href={`ticket/${ticket.ticketId}`}
                >
                  Edit
                </Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className="flex justify-center mt-10">
        {/* <TicketPagination totalCount={response?.totalItems} /> */}
      </div>
    </div>
  );
}
