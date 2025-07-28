"use client";

import Input from "@/app/components/forms/Input";
import { useForm } from "react-hook-form";
import * as z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

import { Ticket, Option } from "@/types";
import { useEffect, useState } from "react";

import { getUsers } from "@/app/api/actions/userActions";
import { Button } from "flowbite-react";
import {
  createInternalTicket,
  createTicket,
  updateTicket,
} from "@/app/api/actions/ticketActions";
import { useRouter } from "next/navigation";

import { ticketStatusList } from "@/utility/Constants";
import SelectField from "../components/forms/Select";

const schema = z.object({
  title: z.string().min(1, "Title is required"),
  detail: z.string().optional(),
  note: z.string().optional(),
  userId: z.string().min(1, "User is required"),
  status: z.string(),
});

export type FormData = {
  ticketId?: number;
  title: string;
  detail?: string;
  note?: string;
  userId: string;
  status: string;
};

export default function TicketForm({
  ticket,
  isInternal,
  workpackageIdList,
  handleClose,
}: {
  ticket?: Ticket;
  isInternal: boolean;
  workpackageIdList?: string[];
  handleClose?: () => void;
}) {
  const [allUsers, setAllUsers] = useState<Option[]>([]);

  const router = useRouter();

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    mode: "onChange",
    values: {
      detail: ticket?.detail ?? "",
      note: ticket?.note ?? "",
      ticketId: ticket?.ticketId,
      title: ticket?.title ?? "",
      userId: ticket?.userId?.toString() ?? "",
      status: ticket?.status ?? "1", // Default status if not provided
    },
  });

  useEffect(() => {
    async function getAllUsers() {
      const data = await getUsers();
      const options = data.map((user) => ({
        value: user.userId.toString(),
        text: user.name,
      }));

      options.unshift({ value: "", text: "Select User" }); // Add a default option

      setAllUsers(options);
    }

    getAllUsers();
  }, []);

  // useEffect(() => {}, [allUsers]);

  const onSubmit = async (data: FormData) => {
    console.log("submit", data, ticket);

    const type = isInternal ? "Internal" : "External";

    try {
      if (ticket?.ticketId) {
        console.log("updating ticket", data);
        await updateTicket({ ...data, ticketId: ticket.ticketId, type: type });
      } else {
        if (isInternal) {
          await createInternalTicket({ ...data, type: type });
        } else {
          await createTicket({ ...data, type: type, workpackageIdList });
        }
      }
      if (handleClose) {
        handleClose();
      }

      router.push("/ticket");
    } catch (error) {
      console.log("error", error);
    }
  };

  console.log("errors", errors);

  return (
    <div>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="flex-col">
          <div>
            <Input
              showlabel
              label="Title"
              type="title"
              placeholder="Enter Title"
              name="title"
              control={control}
            />
          </div>

          <div>
            <Input
              showlabel
              label="Detail"
              type="detail"
              placeholder="Enter Detail"
              name="detail"
              control={control}
            />
          </div>

          <div className="w-full">
            <Input
              showlabel
              label="Note"
              type="note"
              placeholder="Any additional notes"
              name="note"
              control={control}
            />
          </div>
          <div className="flex gap-4">
            <div className="w-1/2">
              <SelectField
                showlabel
                label="User"
                name="userId"
                control={control}
                options={allUsers}
              />
            </div>

            <div className="w-1/2">
              <SelectField
                showlabel
                label="Status"
                name="status"
                control={control}
                options={ticketStatusList}
              />
            </div>
          </div>

          <div className="flex justify-end my-4">
            <Button type="submit">Submit</Button>
          </div>
        </div>
      </form>
    </div>
  );
}
