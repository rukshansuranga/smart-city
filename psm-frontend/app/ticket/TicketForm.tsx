"use client";

import Input from "@/app/components/forms/Input";
import { useForm } from "react-hook-form";
import * as z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useSession } from "next-auth/react";

import { Ticket, Option, Complain } from "@/types";
import { useEffect, useMemo, useState } from "react";

import { getUsers } from "@/app/api/actions/userActions";
import { Button } from "flowbite-react";
import {
  createInternalTicket,
  createTicket,
  updateTicket,
} from "@/app/api/actions/ticketActions";
import { useRouter } from "next/navigation";

import SelectField from "../components/forms/Select";
import {
  TicketPriority,
  TicketStatus,
  TicketType,
  TicketWorkpackageType,
} from "@/enums";
import DateField from "../components/forms/DateField";

const schema = z.object({
  subject: z.string().min(1, "Title is required"),
  detail: z.string().optional(),
  note: z.string().optional(),
  userId: z.string().min(1, "User is required"),
  status: z.number().int().min(0, "Status is required"),
  estimate: z.number().int().min(0, "Estimate is required"),
  dueDate: z.union([z.string(), z.date()]).optional(),
  priority: z.coerce.number().int().optional(),
});

export default function TicketForm({
  ticketWorkpackageType,
  ticket,
  isInternal,
  workpackageList,
  initialValue,
  handleClose,
}: {
  ticketWorkpackageType?: TicketWorkpackageType;
  ticket?: Ticket;
  isInternal: boolean;
  workpackageList?: Complain[];
  initialValue?: { subject: string; detail: string; note?: string };
  handleClose?: () => void;
}) {
  const [allUsers, setAllUsers] = useState<Option[]>([]);
  const { data: session } = useSession();

  const router = useRouter();

  const defaultValues = useMemo(
    () => ({
      detail: ticket?.ticketId
        ? ticket.detail
        : workpackageList?.length === 1
        ? workpackageList[0].detail
        : initialValue?.detail,
      note: ticket?.note ?? "",
      ticketId: ticket?.ticketId,
      subject: ticket?.ticketId
        ? ticket.subject
        : workpackageList?.length === 1
        ? workpackageList[0].subject
        : initialValue?.subject,
      userId: ticket?.userId?.toString() ?? "",
      status: ticket?.status ?? TicketStatus.Open,
      estimate: ticket?.estimate ?? 60,
      priority: ticket?.priority ?? 0,
      dueDate: ticket?.dueDate ? new Date(ticket.dueDate) : new Date(),
    }),
    [ticket, workpackageList]
  );

  console.log("defaultValues", defaultValues, initialValue);

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<Ticket>({
    resolver: zodResolver(schema),
    mode: "onChange",
    values: defaultValues,
  });

  useEffect(() => {
    async function getAllUsers() {
      const data = await getUsers();
      const options = data.map((user) => ({
        value: user.userId.toString(),
        text: user.firstName,
      }));

      options.unshift({ value: "", text: "Select User" }); // Add a default option

      setAllUsers(options);
    }

    getAllUsers();
  }, []);

  const onSubmit = async (data: Ticket) => {
    console.log("submit", data, ticket);

    const type = isInternal ? TicketType.Internal : TicketType.External;

    const dueDateString =
      data.dueDate instanceof Date
        ? data.dueDate.toISOString().slice(0, 10)
        : data.dueDate;

    const payload = {
      ...data,
      dueDate: dueDateString,
      createdBy: session?.user?.id,
      // priority:
      //   typeof data.priority === "string"
      //     ? Number(data.priority)
      //     : data.priority,
    };

    try {
      if (ticket?.ticketId) {
        console.log("updating ticket", data);
        await updateTicket({
          ...payload,
          ticketId: ticket.ticketId,
          type: type,
        });
      } else {
        if (isInternal) {
          console.log("creating external ticket", data);
          await createInternalTicket({
            ...payload,
            type: type,
          });
        } else {
          console.log("creating external ticket", data);
          await createTicket({
            ...payload,
            type: type,
            WorkpackageIdList: workpackageList.map((pkg) => pkg.complainId!),
            ticketWorkpackageType: ticketWorkpackageType,
          });
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

  console.log(
    "priority options",
    Object.entries(TicketPriority)
      .filter(([, value]) => typeof value === "number")
      .map(([key, value]) => ({
        value,
        text: key.replace(/([A-Z])/g, " $1").trim(),
      }))
  );

  return (
    <div>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="flex-col">
          <div>
            <Input
              showlabel
              label="Subject"
              type="subject"
              placeholder="Enter Subject"
              name="subject"
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
              {ticket?.ticketId ? (
                <SelectField
                  showlabel
                  label="Status"
                  name="status"
                  control={control}
                  options={Object.entries(TicketStatus)
                    .filter(([, value]) => typeof value === "number")
                    .map(([key, value]) => ({
                      value,
                      text: key.replace(/([A-Z])/g, " $1").trim(),
                    }))}
                />
              ) : (
                <Input
                  showlabel
                  label="Status"
                  type="status"
                  name="status"
                  value={TicketStatus[TicketStatus.Open]}
                  control={control}
                />
              )}
            </div>
          </div>

          <div className="flex gap-2">
            <div className="w-1/3">
              <SelectField
                showlabel
                label="Priority"
                name="priority"
                control={control}
                options={Object.entries(TicketPriority)
                  .filter(([, value]) => typeof value === "number")
                  .map(([key, value]) => ({
                    value,
                    text: key.replace(/([A-Z])/g, " $1").trim(),
                  }))}
              />
            </div>
            <div className="w-1/3">
              {" "}
              <Input
                showlabel
                label="Estimate (Min)"
                type="number"
                placeholder="Enter estimate"
                name="estimate"
                control={control}
              />
            </div>
            <div className="w-1/3">
              <DateField
                showlabel
                label="Due Date"
                placeholder="Select due date"
                name="dueDate"
                control={control}
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
