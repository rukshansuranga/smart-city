"use client";
import {
  getTicketsByUserId,
  resolveTicket,
  startTicket,
  createInternalTicket,
} from "@/app/api/client/ticketActions";

import ComplainDetail from "@/app/components/complain/ComplainDetail";
import {
  TicketCategory,
  TicketStatus,
  TicketType,
  TicketWorkpackageType,
} from "@/enums";
import { Ticket, Complain } from "@/types";
import {
  Badge,
  Button,
  Modal,
  ModalBody,
  ModalHeader,
  Popover,
} from "flowbite-react";
import Link from "next/link";
import { useParams } from "next/navigation";
import { useEffect, useState } from "react";
import ComplainDetailContainer from "../components/ComplainDetailContainer";
import { useSession } from "next-auth/react";
import toast, { Toaster } from "react-hot-toast";
import TicketDetail from "@/app/components/board/TicketDetail";

function GetCategory(category: TicketCategory) {
  switch (category) {
    case TicketCategory.ComplainTicket:
      return "Complain";
    case TicketCategory.ProjectTicket:
      return "Project";
    case TicketCategory.InternalTicket:
      return "Internal";
    default:
      return "Unknown";
  }
}

export default function UserBoard() {
  const { userId } = useParams();
  const { data: session, status } = useSession();
  const [columns, setColumns] = useState({
    [TicketStatus[TicketStatus.Open]]: {
      name: "Open",
      items: [],
    },

    [TicketStatus[TicketStatus.InProgress]]: {
      name: "In Progress",
      items: [],
    },
    [TicketStatus[TicketStatus.Resolved]]: {
      name: "Resolved",
      items: [],
    },
  });

  const [openModal, setOpenModal] = useState<boolean>(false);

  const [newTask, setNewTask] = useState("");
  const [activeColumn, setActiveColumn] = useState(
    TicketStatus[TicketStatus.Open]
  );
  const [draggedItem, setDraggedItem] = useState(null);

  const [selectedWorkpackage, setSelectedWorkpackage] =
    useState<Complain | null>(null);

  const [selectedTicket, setSelectedTicket] = useState<Ticket | null>(null);

  useEffect(() => {
    fetchTicketsByUserId(userId);
  }, [userId]);

  const fetchTicketsByUserId = async (userId) => {
    try {
      const boardTickets = await getTicketsByUserId(userId);

      const updatedColumns = { ...columns };

      if (!boardTickets.isSuccess) {
        toast.error("Failed to fetch tickets", {
          duration: 3000,
          style: {
            background: "#FFFFFF",
            color: "#264653",
            border: "2px solid #E76F51",
            borderRadius: "12px",
            fontWeight: "500",
          },
        });
        return;
      }

      // Clear existing items
      Object.keys(updatedColumns).forEach((key) => {
        updatedColumns[key].items = [];
      });

      boardTickets.data.complainTickets.forEach((ticket) => {
        const columnId = TicketStatus[ticket.status];
        if (updatedColumns[columnId]) {
          updatedColumns[columnId].items.push({
            ...ticket,
            category: TicketCategory.ComplainTicket,
          });
        }
      });

      boardTickets.data.projectTickets.forEach((ticket) => {
        const columnId = TicketStatus[ticket.status];
        if (updatedColumns[columnId]) {
          updatedColumns[columnId].items.push({
            ...ticket,
            category: TicketCategory.ProjectTicket,
          });
        }
      });

      boardTickets.data.internalTickets.forEach((ticket) => {
        const columnId = TicketStatus[ticket.status];
        if (updatedColumns[columnId]) {
          updatedColumns[columnId].items.push({
            ...ticket,
            category: TicketCategory.InternalTicket,
          });
        }
      });

      console.log("Fetched tickets:", updatedColumns);
      setColumns(updatedColumns);

      toast.success(
        `Successfully loaded ${boardTickets.data.complainTickets.length} tickets`,
        {
          duration: 2000,
          style: {
            background: "#FFFFFF",
            color: "#264653",
            border: "2px solid #2A9D8F",
            borderRadius: "12px",
            fontWeight: "500",
          },
        }
      );
    } catch (error) {
      console.error("Error fetching tickets:", error);
      toast.error("An error occurred while fetching tickets", {
        duration: 3000,
        style: {
          background: "#FFFFFF",
          color: "#264653",
          border: "2px solid #E76F51",
          borderRadius: "12px",
          fontWeight: "500",
        },
      });
    }
  };

  const addNewTask = async () => {
    if (newTask.trim() === "") {
      toast.error("Please enter a task name", {
        duration: 2000,
        style: {
          background: "#FFFFFF",
          color: "#264653",
          border: "2px solid #E9C46A",
          borderRadius: "12px",
          fontWeight: "500",
        },
      });
      return;
    }

    const response = await createInternalTicket({
      subject: newTask,
      userId: session?.user?.id,
      type: TicketType.Internal,
      status: TicketStatus.Open,
    });

    if (!response.isSuccess) {
      toast.error("Failed to create task", {
        duration: 3000,
        style: {
          background: "#FFFFFF",
          color: "#264653",
          border: "2px solid #E76F51",
          borderRadius: "12px",
          fontWeight: "500",
        },
      });
      return;
    }

    const updatedColumns = { ...columns };
    // const newTaskItem = {
    //   ticketId: response.data.ticketId,
    //   subject: newTask,
    // };

    updatedColumns[activeColumn].items.push({
      ...response.data,
      category: TicketCategory.InternalTicket,
    });
    setColumns(updatedColumns);
    setNewTask("");

    toast.success(`Task "${newTask}" added to ${columns[activeColumn].name}`, {
      duration: 2000,
      style: {
        background: "#FFFFFF",
        color: "#264653",
        border: "2px solid #2A9D8F",
        borderRadius: "12px",
        fontWeight: "500",
      },
    });
  };

  const removeTask = (columnId, itemId) => {
    const updatedColumns = { ...columns };
    const taskToRemove = updatedColumns[columnId].items.find(
      (item) => item.ticketId === itemId
    );

    updatedColumns[columnId].items = updatedColumns[columnId].items.filter(
      (item) => item.ticketId !== itemId
    );
    setColumns(updatedColumns);

    if (taskToRemove) {
      toast(`Task "${taskToRemove.subject}" has been removed`, {
        duration: 2000,
        icon: "🗑️",
        style: {
          background: "#FFFFFF",
          color: "#264653",
          border: "2px solid #F4A261",
          borderRadius: "12px",
          fontWeight: "500",
        },
      });
    }
  };

  const handleDragStart = (columnId, item) => {
    setDraggedItem({ columnId, item });
  };

  const handleDragOver = (e) => {
    e.preventDefault();
  };

  const handleDrop = async (e, columnId) => {
    e.preventDefault();
    if (!draggedItem) return;
    const { columnId: sourceColumnId, item } = draggedItem;

    if (sourceColumnId === columnId) return;

    const updatedColumns = { ...columns };

    updatedColumns[sourceColumnId].items = updatedColumns[
      sourceColumnId
    ].items.filter((i) => i.ticketId !== item?.ticketId);

    updatedColumns[columnId].items.push(item);

    try {
      if (columnId === TicketStatus[TicketStatus.InProgress]) {
        // If the item is dropped in the "In Progress" column, start the ticket
        await startTicket(item?.ticketId);
        toast(`Ticket "${item?.subject}" moved to In Progress`, {
          duration: 2000,
          icon: "🔄",
          style: {
            background: "#FFFFFF",
            color: "#264653",
            border: "2px solid #E9C46A",
            borderRadius: "12px",
            fontWeight: "500",
          },
        });
      }

      if (columnId === TicketStatus[TicketStatus.Resolved]) {
        // If the item is dropped in the "Resolved" column, resolve the ticket
        await resolveTicket(item?.ticketId);
        toast.success(`Ticket "${item?.subject}" has been resolved!`, {
          duration: 2000,
          style: {
            background: "#FFFFFF",
            color: "#264653",
            border: "2px solid #2A9D8F",
            borderRadius: "12px",
            fontWeight: "500",
          },
        });
      }

      setColumns(updatedColumns);
      setDraggedItem(null);
    } catch (error) {
      console.error("Error updating ticket status:", error);
      toast.error("Failed to update ticket status", {
        duration: 3000,
        style: {
          background: "#FFFFFF",
          color: "#264653",
          border: "2px solid #E76F51",
          borderRadius: "12px",
          fontWeight: "500",
        },
      });

      // Revert the changes on error
      updatedColumns[sourceColumnId].items.push(item);
      updatedColumns[columnId].items = updatedColumns[columnId].items.filter(
        (i) => i.ticketId !== item?.ticketId
      );
      setColumns(updatedColumns);
      setDraggedItem(null);
    }
  };

  const columnStyles = {
    [TicketStatus[TicketStatus.Open]]: {
      header: "bg-[#2A9D8F]",
      border: "border-[#2A9D8F]",
    },
    [TicketStatus[TicketStatus.InProgress]]: {
      header: "bg-[#E9C46A]",
      border: "border-[#E9C46A]",
    },
    [TicketStatus[TicketStatus.Resolved]]: {
      header: "bg-[#F4A261]",
      border: "border-[#F4A261]",
    },
    [TicketStatus[TicketStatus.Closed]]: {
      header: "bg-[#E76F51]",
      border: "border-[#E76F51]",
    },
  };

  console.log("column", columns);

  function handleDetailHandler(ticket, complain) {
    console.log("Complain clicked:", complain);
    setSelectedWorkpackage(complain);
    setSelectedTicket(ticket);
    setOpenModal(true);
  }

  console.log("modal status:", openModal);

  return (
    <>
      <div className="p-6 w-full min-h-screen bg-gradient-to-br from-white via-[#F4F4F4] to-[#E9C46A]/20 flex items-center justify-center">
        <div className="flex items-center justify-center flex-col gap-4 w-full">
          <h1 className="text-4xl font-bold mb-4 text-[#264653] drop-shadow-lg">
            Your Tickets
          </h1>

          <div className="mb-8 flex w-full max-w-lg shadow-2xl rounded-xl overflow-hidden backdrop-blur-sm bg-[#2A9D8F]/20 border-2 border-[#2A9D8F]/50">
            <input
              type="text"
              value={newTask}
              onChange={(e) => setNewTask(e.target.value)}
              placeholder="Add a new task..."
              className="flex-grow p-3 bg-white/90 text-[#264653] placeholder-[#264653]/70 border-0 backdrop-blur-sm focus:outline-none focus:ring-2 focus:ring-[#E9C46A]"
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  addNewTask();
                }
              }}
            />

            <select
              value={activeColumn}
              onChange={(e) => setActiveColumn(e.target.value)}
              className="p-3 bg-white/90 text-[#264653] border-0 focus:outline-none focus:ring-2 focus:ring-[#E9C46A] backdrop-blur-sm"
            >
              {Object.keys(columns).map((columnId) => (
                <option
                  key={columnId}
                  value={columnId}
                  className="bg-white text-[#264653]"
                >
                  {columns[columnId].name}
                </option>
              ))}
            </select>
            <button
              onClick={addNewTask}
              className="px-6 bg-[#F4A261] text-white font-medium hover:bg-[#E76F51] transition-all duration-200 cursor-pointer shadow-lg"
            >
              Add
            </button>
          </div>

          <div className="flex gap-6 pb-6 w-full ">
            {Object.keys(columns).map((columnId) => (
              <div
                key={columnId}
                className={`flex-shrink-0 flex-1 bg-white/95 backdrop-blur-md rounded-xl shadow-2xl border-t-4 ${
                  columnStyles[columnId.border]
                }`}
                onDragOver={(e) => handleDragOver(e, columnId)}
                onDrop={(e) => handleDrop(e, columnId)}
              >
                <div
                  className={`flex justify-between p-4 text-white font-bold text-xl rounded-t-lg ${columnStyles[columnId].header} shadow-lg`}
                >
                  <span className="pl-3 drop-shadow-md">
                    {columns[columnId].name}
                  </span>
                  <span className="ml-2 px-2 py-1 bg-white/30 backdrop-blur-sm rounded-full text-sm shadow-inner">
                    {columns[columnId].items.length}
                  </span>
                </div>
                <div className="p-3 min-h-64">
                  {columns[columnId].items.length === 0 ? (
                    <div className="text-center py-10 text-[#264653]/60 italic text-sm">
                      Drop task here
                    </div>
                  ) : (
                    columns[columnId].items.map((item: Ticket) => (
                      <div
                        key={item.ticketId}
                        className="p-4 mb-3 bg-white backdrop-blur-sm text-[#264653] rounded-xl shadow-lg cursor-move flex flex-col items-center justify-between transform transition-all duration-200 hover:shadow-xl hover:bg-[#2A9D8F]/10 hover:scale-105 border border-[#2A9D8F]/20"
                        draggable
                        onDragStart={() => handleDragStart(columnId, item)}
                      >
                        <div className="flex items-center justify-between w-full">
                          <span className="mr-2 font-medium text-[#264653]">
                            {item.subject}
                          </span>
                          <button
                            onClick={() => removeTask(columnId, item.ticketId)}
                            className="text-[#264653]/60 hover:text-[#E76F51] transition-colors duration-200 w-6 h-6 flex items-center justify-center rounded-full hover:bg-[#E76F51]/20"
                          >
                            <span className="text-lg cursor-pointer">×</span>
                          </button>
                        </div>
                        <div className="flex items-center justify-between w-full mt-2">
                          <span className="text-sm text-[#264653]/80">
                            {GetCategory(item.category)}
                          </span>

                          <div className="flex flex-wrap gap-2">
                            {item.category == TicketCategory.ComplainTicket &&
                              item.ticketComplains.length > 0 &&
                              item.ticketComplains.map((pack) => (
                                <Badge
                                  onClick={() =>
                                    handleDetailHandler(null, pack.complain)
                                  }
                                  key={pack?.complainId}
                                  className="bg-[#2A9D8F] text-white hover:bg-[#E9C46A] hover:text-[#264653] cursor-pointer transition-all duration-200 shadow-md"
                                  style={{
                                    borderRadius: "10px",
                                  }}
                                  size="sm"
                                >
                                  {pack?.complainId}
                                </Badge>
                              ))}

                            {item.category == TicketCategory.ProjectTicket && (
                              <Badge
                                onClick={() => handleDetailHandler(item, null)}
                                className="bg-[#2A9D8F] text-white hover:bg-[#E9C46A] hover:text-[#264653] cursor-pointer transition-all duration-200 shadow-md"
                                style={{
                                  borderRadius: "10px",
                                }}
                                size="sm"
                              >
                                View
                              </Badge>
                            )}

                            {item.category == TicketCategory.InternalTicket && (
                              <Badge
                                onClick={() => handleDetailHandler(item, null)}
                                className="bg-[#2A9D8F] text-white hover:bg-[#E9C46A] hover:text-[#264653] cursor-pointer transition-all duration-200 shadow-md"
                                style={{
                                  borderRadius: "10px",
                                }}
                                size="sm"
                              >
                                View
                              </Badge>
                            )}
                          </div>
                        </div>
                      </div>
                    ))
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
      <Modal dismissible show={openModal} onClose={() => setOpenModal(false)}>
        <ModalHeader>
          {selectedWorkpackage && (
            <span className="bg-[#2A9D8F] text-white rounded-2xl px-3 py-1">
              {selectedWorkpackage?.complainId}
            </span>
          )}

          {selectedTicket && (
            <span className="bg-[#2A9D8F] text-white rounded-2xl px-3 py-1">
              Selected Ticket - {selectedTicket?.ticketId}
            </span>
          )}
        </ModalHeader>
        <ModalBody>
          {selectedWorkpackage && (
            <ComplainDetailContainer complain={selectedWorkpackage} />
          )}

          {selectedTicket && <TicketDetail ticket={selectedTicket} />}
        </ModalBody>
      </Modal>

      {/* Toast Container with custom styling */}
      <Toaster
        position="top-right"
        reverseOrder={false}
        gutter={8}
        containerClassName=""
        containerStyle={{}}
        toastOptions={{
          className: "",
          duration: 3000,
          style: {
            background: "#FFFFFF",
            color: "#264653",
            border: "2px solid #2A9D8F",
            borderRadius: "12px",
            fontWeight: "500",
            padding: "16px",
          },
          success: {
            duration: 2000,
            style: {
              background: "#FFFFFF",
              color: "#264653",
              border: "2px solid #2A9D8F",
            },
          },
          error: {
            duration: 3000,
            style: {
              background: "#FFFFFF",
              color: "#264653",
              border: "2px solid #E76F51",
            },
          },
        }}
      />
    </>
  );
}
