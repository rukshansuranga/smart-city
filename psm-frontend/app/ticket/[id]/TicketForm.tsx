"use client";

import Input from "@/app/components/forms/Input";
import { Controller, useForm } from "react-hook-form";
import * as Yup from "yup";

import { Ticket, User } from "@/types";
import { useEffect, useState } from "react";
import { yupResolver } from "@hookform/resolvers/yup";
import { getUsers } from "@/app/api/actions/userActions";
import { Button, Label, Select } from "flowbite-react";
import { updateTicket } from "@/app/api/actions/ticketActions";
import { useRouter } from "next/navigation";

const schema = Yup.object().shape({
  title: Yup.string().required("Title is required"),
  detail: Yup.string(),
  note: Yup.string(),
  userId: Yup.string(),
});

export type FormData = {
  ticketId?: number;
  title: string;
  detail: string;
  note: string;
  userId?: string;
};

export default function TicketForm({ ticket }: { ticket: Ticket }) {
  const [allUsers, setAllUsers] = useState<User[]>([]);

  const router = useRouter();

  const { control, handleSubmit, setValue } = useForm<FormData>({
    resolver: yupResolver(schema),
    mode: "onChange",
    values: {
      detail: ticket.detail ?? "",
      note: ticket.note ?? "",
      ticketId: ticket.ticketId,
      title: ticket.title,
      userId: ticket.userId,
    },
  });

  useEffect(() => {
    async function getAllUsers() {
      const data = await getUsers();
      setAllUsers(data);
    }

    getAllUsers();
  }, []);

  useEffect(() => {}, [allUsers]);

  const onSubmit = async (data: FormData) => {
    console.log("submit", data);
    try {
      await updateTicket(data);
      router.push("/ticket");
    } catch (error) {
      console.log("error", error);
    }
  };

  return (
    <div>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="flex-col">
          <div className="flex w-full gap-2">
            <div className="w-1/2 ">
              <Input
                showLabel
                label="Title"
                type="title"
                placeholder="Enter Title"
                name="title"
                control={control}
              />
            </div>

            <div className="w-1/2">
              <Input
                showLabel
                label="Detail"
                type="detail"
                placeholder="Enter Detail"
                name="detail"
                control={control}
              />
            </div>
          </div>

          <div className="w-full flex gap-2">
            <div className="w-1/2">
              <Input
                showLabel
                label="Note"
                type="note"
                placeholder="Any additional notes"
                name="note"
                control={control}
              />
            </div>
            <div className="w-1/2">
              <Controller
                name="userId"
                control={control}
                render={({ field: { onChange, value } }) => (
                  <div className="w-full">
                    <div className="mb-2 block">
                      <Label htmlFor="countries">Select User</Label>
                    </div>
                    <Select
                      value={value}
                      onChange={(e) => onChange(e.target.value)}
                    >
                      {allUsers.map((user) => (
                        <option key={user.userId} value={user.userId}>
                          {user.name}
                        </option>
                      ))}
                    </Select>
                  </div>
                )}
              />
            </div>
          </div>

          <div className="flex justify-end mt-4">
            <Button type="submit">Submit</Button>
          </div>
        </div>
      </form>
    </div>
  );
}
