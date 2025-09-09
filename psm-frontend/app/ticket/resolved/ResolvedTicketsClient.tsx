"use client";
import { useState } from "react";
import { Checkbox, Button } from "flowbite-react";
import {
  closeTicket,
  getResolvedTickets,
} from "@/app/api/actions/ticketActions";
import Link from "next/link";
import { Ticket } from "@/types";
import toast from "react-hot-toast";

// type Ticket = {
//   ticketId: string;
//   subject: string;
// };

interface ResolvedTicketsClientProps {
  tickets: Ticket[];
}

export default function ResolvedTicketsClient({
  tickets,
}: ResolvedTicketsClientProps) {
  const [selected, setSelected] = useState<string[]>([]);
  const [localTickets, setLocalTickets] = useState<Ticket[]>(tickets);

  // Refetch tickets from API
  const fetchTickets = async () => {
    try {
      const tickets = await getResolvedTickets();

      if (!tickets.isSuccess) {
        toast.error(tickets.message || "Failed to fetch tickets");
        return;
      }

      setLocalTickets(tickets.data);
    } catch (error) {
      console.error("Failed to fetch tickets", error);
    }
  };

  // Removed useEffect to avoid resetting localTickets on initial render

  const handleCheck = (ticketId: string, checked: boolean) => {
    setSelected((prev) =>
      checked ? [...prev, ticketId] : prev.filter((id) => id !== ticketId)
    );
  };

  const handleButtonClick = async () => {
    console.log("Selected ticket IDs:", selected);
    for (const ticketId of selected) {
      await closeTicket(ticketId);
    }
    await fetchTickets();
    setSelected([]);
  };

  return (
    <div className="flex flex-col items-center gap-2">
      <div>
        <h1>Resolved Tickets</h1>
      </div>
      <div>
        <ul>
          {localTickets && localTickets.length > 0 ? (
            localTickets.map((ticket) => (
              <li
                key={ticket.ticketId}
                className="flex items-center gap-4 py-1"
              >
                <Checkbox
                  id={`resolved-ticket-${ticket.ticketId}`}
                  checked={selected.includes(ticket.ticketId?.toString() || "")}
                  onChange={(e) =>
                    handleCheck(
                      ticket.ticketId?.toString() || "",
                      e.target.checked
                    )
                  }
                />
                <span>{ticket.subject}</span>
                <Link href={`/ticket/${ticket.ticketId}`}>
                  <Button size="xs">View</Button>
                </Link>
              </li>
            ))
          ) : (
            <li>No resolved tickets found.</li>
          )}
        </ul>
      </div>
      <Button
        className="mt-4"
        onClick={handleButtonClick}
        disabled={selected.length === 0}
      >
        Close Ticket
      </Button>
    </div>
  );
}
