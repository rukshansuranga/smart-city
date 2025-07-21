"use client";

import Input from "@/app/components/forms/Input";
import { Controller, useForm } from "react-hook-form";
import * as z from "zod";

import { Button, Label, Select } from "flowbite-react";
import { useEffect, useState } from "react";
import { getUsers } from "@/app/api/actions/userActions";
import { User, Workpackage } from "@/types";
import { createTicket } from "@/app/api/actions/ticketActions";
import { zodResolver } from "@hookform/resolvers/zod";

const schema = z.object({
  title: z.string().trim().min(1, "Project name is required"),
  detail: z.string().optional(),
  note: z.string().optional(),
  userId: z.string().optional(),
});

export type FormData = {
  title: string;
  detail?: string;
  note?: string;
  userId?: string;
};

export default function CreateTicketForm({
  open,
  packages,
}: {
  open: (open: boolean) => void;
  packages: Workpackage[];
}) {
  const [allUsers, setAllUsers] = useState<User[]>([]);

  const { control, handleSubmit, setValue } = useForm<FormData>({
    resolver: zodResolver(schema),
    mode: "onChange",
  });

  useEffect(() => {
    async function getAllUsers() {
      const data = await getUsers();
      setAllUsers(data);
      setValue("userId", data[0]?.userId || ""); // Set default user if available
    }

    getAllUsers();
  }, []);

  const onSubmit = async (data: FormData) => {
    const request = {
      ...data,
      workpackageIdList: packages.map((wp) => wp.workPackageId!),
    };

    await createTicket(request);

    open(false);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex-col">
        <div className="flex w-full gap-2">
          <div className="w-1/2 ">
            <Input
              showlabel
              label="Title"
              type="title"
              placeholder="Enter Title"
              name="title"
              control={control}
            />
          </div>

          <div className="w-1/2">
            <Input
              showlabel
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
              showlabel
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

        <div className="w-full mt-4">
          <table className="w-full bg-gray-200 text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
            <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
              <tr>
                <th scope="col" className="px-6 py-3">
                  ID
                </th>
                <th scope="col" className="px-6 py-3">
                  Name
                </th>
              </tr>
            </thead>
            <tbody>
              {packages.map((workpackage) => (
                <tr key={workpackage.workPackageId}>
                  <td className="px-6 py-4">{workpackage.workPackageId}</td>
                  <td className="px-6 py-4">{workpackage.name}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className="flex justify-end mt-4">
          <Button type="submit">Submit</Button>
        </div>
      </div>
    </form>
  );
}
