"use client";
import {
  getTicketsByUserId,
  resolveTicket,
  startTicket,
} from "@/app/api/actions/ticketActions";
import ComplainDetail from "@/app/components/complain/ComplainDetail";
import { TicketStatus, TicketType } from "@/enums";
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
    // [TicketStatus[TicketStatus.Closed]]: {
    //   name: "Closed",
    //   items: [],
    // },
  });

  const [openModal, setOpenModal] = useState<boolean>(false);

  const [newTask, setNewTask] = useState("");
  const [activeColumn, setActiveColumn] = useState(
    TicketStatus[TicketStatus.Open]
  );
  const [draggedItem, setDraggedItem] = useState(null);

  const [selectedWorkpackage, setSelectedWorkpackage] =
    useState<Complain | null>(null);

  useEffect(() => {
    fetchTicketsByUserId(userId);
  }, [userId]);

  const fetchTicketsByUserId = async (userId) => {
    try {
      const ticketList = await getTicketsByUserId(userId);

      const updatedColumns = { ...columns };
      ticketList.forEach((ticket) => {
        const columnId = TicketStatus[ticket.status];
        if (updatedColumns[columnId]) {
          updatedColumns[columnId].items.push(ticket);
        }
      });
      console.log("Fetched tickets:", updatedColumns);
      setColumns(updatedColumns);
    } catch (error) {
      console.error("Error fetching tickets:", error);
    }
  };

  const addNewTask = () => {
    if (newTask.trim() === "") return; // Prevent adding empty tasks

    const updatedColumns = { ...columns };
    updatedColumns[activeColumn].items.push({
      ticketId: Date.now().toString(),
      subject: newTask,
    });
    setColumns(updatedColumns);
    setNewTask("");
  };

  const removeTask = (columnId, itemId) => {
    const updatedColumns = { ...columns };
    updatedColumns[columnId].items = updatedColumns[columnId].items.filter(
      (item) => item.ticketId !== itemId
    );
    setColumns(updatedColumns);
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

    if (columnId === TicketStatus[TicketStatus.InProgress]) {
      // If the item is dropped in the "In Progress" column, start the ticket
      await startTicket(item?.ticketId);
    }

    if (columnId === TicketStatus[TicketStatus.Resolved]) {
      // If the item is dropped in the "Resolved" column, resolve the ticket
      await resolveTicket(item?.ticketId);
    }

    setColumns(updatedColumns);
    setDraggedItem(null);
  };

  const columnStyles = {
    [TicketStatus[TicketStatus.Open]]: {
      header: "bg-gradient-to-r from-blue-600 to-blue-400",
      border: "border-blue-400",
    },
    [TicketStatus[TicketStatus.InProgress]]: {
      header: "bg-gradient-to-r from-yellow-600 to-yellow-400",
      border: "border-yellow-400",
    },
    [TicketStatus[TicketStatus.Resolved]]: {
      header: "bg-gradient-to-r from-green-600 to-green-400",
      border: "border-green-400",
    },
    [TicketStatus[TicketStatus.Closed]]: {
      header: "bg-gradient-to-r from-gray-600 to-gray-400",
      border: "border-gray-400",
    },
  };

  console.log("column", columns);

  function handleWorkpackage(complain) {
    console.log("Complain clicked:", complain);
    setSelectedWorkpackage(complain);
    setOpenModal(true);
  }

  console.log("modal status:", openModal);

  return (
    <>
      <div className="p-6 w-full min-h-screen bg-gradient-to-b from-zinc-900 to-zinc-800 flex items-center justify-center">
        <div className="flex items-center justify-center flex-col gap-4 w-full">
          <h1 className="text-6xl font-bold mb-8 text-transparent bg-clip-text bg-gradient-to-r from-yellow-400 via-amber-500 to-rose-400">
            User's Tickets
          </h1>

          <div className="mb-8 flex w-full max-w-lg shadow-lg rounded-lg overflow-hidden">
            <input
              type="text"
              value={newTask}
              onChange={(e) => setNewTask(e.target.value)}
              placeholder="Add a new task..."
              className="flex-grow p-3 bg-zinc-700 text-white"
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  addNewTask();
                }
              }}
            />

            <select
              value={activeColumn}
              onChange={(e) => setActiveColumn(e.target.value)}
              className="p-3 bg-zinc-700 text-white border-0 border-1 border-zinc-600"
            >
              {Object.keys(columns).map((columnId) => (
                <option key={columnId} value={columnId}>
                  {columns[columnId].name}
                </option>
              ))}
            </select>
            <button
              onClick={addNewTask}
              className="px-6 bg-gradient-to-r from-yellow-600 bg-amber-500 text-white font-medium hover:from-yellow-500 hover:to-amber-500 transition-all duration-200 cursor-pointer"
            >
              Add
            </button>
          </div>

          <div className="flex gap-6 pb-6 w-full ">
            {Object.keys(columns).map((columnId) => (
              <div
                key={columnId}
                className={`flex-shrink-0 flex-1  bg-zinc-800 rounded-lg shadow-xl border-t-4 ${
                  columnStyles[columnId.border]
                }`}
                onDragOver={(e) => handleDragOver(e, columnId)}
                onDrop={(e) => handleDrop(e, columnId)}
              >
                <div
                  className={`flex justify-between p-4 text-white font-bold text-xl rounded-t-md ${columnStyles[columnId].header}`}
                >
                  <span className="pl-3">{columns[columnId].name}</span>
                  <span className="ml-2 px-2 py-1 bg-zinc-800 bg-opacty-30 rounded-full text-sm">
                    {columns[columnId].items.length}
                  </span>
                </div>
                <div className="p-3 min-h-64">
                  {columns[columnId].items.length === 0 ? (
                    <div className="text-center py-10 text-zinc-500 italic text-sm">
                      Drop task here
                    </div>
                  ) : (
                    columns[columnId].items.map((item: Ticket) => (
                      <div
                        key={item.ticketId}
                        className="p-4 mb-3 bg-zinc-700 text-white rounded-lg shadow-md cursor-move flex flex-col items-center justify-between transform transform-all duration-200  hover:shadow-lg"
                        draggable
                        onDragStart={() => handleDragStart(columnId, item)}
                      >
                        <div className="flex items-center justify-between w-full">
                          <span className="mr-2">{item.subject}</span>
                          <button
                            onClick={() => removeTask(columnId, item.ticketId)}
                            className="text-zinc-400 hover:text-red-400 transition-colors duration-200 w-6 h-6 flex items-center justify-center rounded-full hover:bg-zinc-600"
                          >
                            <span className="text-lg cursor-pointer">x</span>
                          </button>
                        </div>
                        <div className="flex items-center justify-between w-full mt-2">
                          <span className="text-sm text-zinc-400">
                            {TicketType[item.type]}
                          </span>

                          <div className="flex flex-wrap gap-2 roun">
                            {item.ticketPackages.length > 0 &&
                              item.ticketPackages.map((pack) => (
                                <Badge
                                  onClick={() =>
                                    handleWorkpackage(pack.complain)
                                  }
                                  key={pack?.complainId}
                                  style={{
                                    borderRadius: "10px",
                                  }}
                                  size="sm"
                                >
                                  {pack?.complainId}
                                </Badge>
                              ))}
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
          Complain -{" "}
          <span className="bg-green-300 rounded-4xl p-2">
            {selectedWorkpackage?.complainId}
          </span>
        </ModalHeader>
        <ModalBody>
          <ComplainDetailContainer complain={selectedWorkpackage} />
        </ModalBody>
      </Modal>
    </>
  );
}
