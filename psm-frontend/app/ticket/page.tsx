import { Paging, Ticket, ApiResponse } from "@/types";
import { getTicketPaging } from "../api/actions/ticketActions";
import TicketList from "./TicketList";

export default async function TicketPage({
  searchParams,
}: {
  searchParams: Promise<{ pageIndex?: string; pageSize?: string }>;
}) {
  const { pageIndex = "1", pageSize = "5" } = await searchParams;

  const response: ApiResponse<Paging<Ticket>> = await getTicketPaging({
    pageIndex,
    pageSize,
  });

  const ticketData =
    response.isSuccess && response.data
      ? response.data
      : { records: [], totalRecords: 0, currentPage: 1, pageSize: 5 };

  return (
    <div className="flex-col ">
      <TicketList tickets={ticketData?.records || []} />
      <div className="flex justify-center mt-10"></div>
    </div>
  );
}
