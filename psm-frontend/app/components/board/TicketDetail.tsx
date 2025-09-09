import { TicketPriority, TicketStatus } from "@/enums";
import { Ticket } from "@/types";

interface TicketDetailProps {
  ticket: Ticket | null;
}

const getStatusText = (status?: TicketStatus): string => {
  if (status === undefined) return "Unknown";
  switch (status) {
    case TicketStatus.Open:
      return "Open";
    case TicketStatus.InProgress:
      return "In Progress";
    case TicketStatus.Resolved:
      return "Resolved";
    case TicketStatus.Closed:
      return "Closed";
    default:
      return "Unknown";
  }
};

const getPriorityText = (priority?: TicketPriority): string => {
  if (priority === undefined) return "Not Set";
  switch (priority) {
    case TicketPriority.Low:
      return "Low";
    case TicketPriority.Medium:
      return "Medium";
    case TicketPriority.High:
      return "High";
    default:
      return "Unknown";
  }
};

const formatDate = (date?: Date | string): string => {
  if (!date) return "Not Set";
  const dateObj = typeof date === "string" ? new Date(date) : date;
  return dateObj.toLocaleDateString();
};

export default function TicketDetail({ ticket }: TicketDetailProps) {
  if (!ticket) return null;

  return (
    <div className="bg-white p-6 rounded-lg shadow-md">
      <h2 className="text-2xl font-bold mb-6 text-gray-800">Ticket Details</h2>

      <div className="space-y-4">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-600 mb-1">
              Subject
            </label>
            <p className="text-gray-900 font-medium">{ticket.subject}</p>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-600 mb-1">
              Status
            </label>
            <span
              className={`inline-flex px-3 py-1 rounded-full text-sm font-medium ${
                ticket.status === TicketStatus.Open
                  ? "bg-blue-100 text-blue-800"
                  : ticket.status === TicketStatus.InProgress
                  ? "bg-yellow-100 text-yellow-800"
                  : ticket.status === TicketStatus.Resolved
                  ? "bg-green-100 text-green-800"
                  : ticket.status === TicketStatus.Closed
                  ? "bg-gray-100 text-gray-800"
                  : "bg-gray-100 text-gray-800"
              }`}
            >
              {getStatusText(ticket.status)}
            </span>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-600 mb-1">
              Estimation (hours)
            </label>
            <p className="text-gray-900">{ticket.estimation}</p>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-600 mb-1">
              Priority
            </label>
            <span
              className={`inline-flex px-3 py-1 rounded-full text-sm font-medium ${
                ticket.priority === TicketPriority.High
                  ? "bg-red-100 text-red-800"
                  : ticket.priority === TicketPriority.Medium
                  ? "bg-orange-100 text-orange-800"
                  : ticket.priority === TicketPriority.Low
                  ? "bg-green-100 text-green-800"
                  : "bg-gray-100 text-gray-800"
              }`}
            >
              {getPriorityText(ticket.priority)}
            </span>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-600 mb-1">
              Due Date
            </label>
            <p className="text-gray-900">{formatDate(ticket.dueDate)}</p>
          </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-600 mb-1">
            Detail
          </label>
          <div className="bg-gray-50 p-4 rounded-md">
            <p className="text-gray-900 whitespace-pre-wrap">
              {ticket.detail || "No details provided"}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
