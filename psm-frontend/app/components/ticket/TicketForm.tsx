"use client";

import Input from "@/app/components/forms/Input";
import { useForm, Resolver } from "react-hook-form";
import * as z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useSession } from "next-auth/react";
import { toast } from "react-hot-toast";

import { Ticket, Option, Complain, Attachment } from "@/types";
import { useEffect, useMemo, useState } from "react";

import { getUsersByUserType } from "@/app/api/client/userActions";
import { Button } from "flowbite-react";
import {
  createInternalTicket,
  createTicket,
  updateTicket,
} from "@/app/api/actions/ticketActions";
import { useRouter } from "next/navigation";

import SelectField from "../forms/Select";
import {
  AuthType,
  ComplainType,
  TicketPriority,
  TicketStatus,
  TicketType,
} from "@/enums";
import DateField from "../forms/DateField";
import MultipleAttachmentUpload from "../forms/MultipleAttachmentUpload";
import TagsInput from "../forms/TagsInput";
import {
  uploadAttachmentsClient,
  getAttachmentsClient,
} from "@/app/api/client/attachmentActions";
import {
  getEntityTagsClient,
  assignTagsToEntityClient,
} from "@/app/api/client/tagActions";

const tagSchema = z.object({
  tagId: z.number(),
  name: z.string(),
  description: z.string().optional(),
  color: z.string().optional(),
  isActive: z.boolean(),
});

const schema = z.object({
  ticketId: z.number().optional(),
  subject: z.string().min(1, "Title is required"),
  detail: z.string().optional(),
  note: z.string().optional(),
  tags: z.array(tagSchema).default([]),
  status: z.number().int().min(0, "Status is required"),
  estimation: z.number().int().min(0, "Estimate is required"),
  priority: z.coerce.number().int().optional(),
  dueDate: z.union([z.string(), z.date()]).optional(),
  attachments: z.array(z.any()).optional(), // Changed to accommodate AttachmentUpload[]
  userId: z.string().min(1, "User is required"),
  createdAt: z.date().optional(),
});

type TicketFormData = z.infer<typeof schema>;

export default function TicketForm({
  complainType,
  ticketType,
  ticket,
  complainList,
  initialValue,
  handleClose,
}: {
  complainType?: ComplainType;
  ticketType: TicketType;
  ticket?: Ticket | null;
  complainList?: Complain[];
  initialValue?: { subject: string; detail: string; note?: string };
  handleClose?: () => void;
}) {
  const [allUsers, setAllUsers] = useState<Option[]>([]);
  const [existingAttachments, setExistingAttachments] = useState<Attachment[]>(
    []
  );
  const { data: session } = useSession();

  const router = useRouter();

  const defaultValues = useMemo(
    () => ({
      detail: ticket?.ticketId
        ? ticket.detail
        : complainList?.length === 1
        ? complainList[0].detail
        : initialValue?.detail,
      note: ticket?.note ?? "",
      ticketId: ticket?.ticketId,
      subject: ticket?.ticketId
        ? ticket.subject
        : complainList?.length === 1
        ? complainList[0].subject
        : initialValue?.subject ?? "",
      userId: ticket?.userId?.toString() ?? "",
      status: ticket?.status ?? TicketStatus.Open,
      estimation: ticket?.estimation ?? 60,
      priority: ticket?.priority ?? 0,
      dueDate: ticket?.dueDate ? new Date(ticket.dueDate) : new Date(),
      tags: ticket?.tags || [],
      attachments: [], // Initialize as empty array for new attachments
    }),
    [ticket, complainList, initialValue?.detail, initialValue?.subject]
  ) as TicketFormData;

  console.log("defaultValues", defaultValues, initialValue);

  const { control, handleSubmit, setValue } = useForm<TicketFormData>({
    resolver: zodResolver(schema) as Resolver<TicketFormData>,
    mode: "onChange",
    values: defaultValues,
  });

  useEffect(() => {
    async function getAllUsers() {
      const response = await getUsersByUserType([AuthType.Staff]);

      if (!response.isSuccess) {
        toast.error(response.message || "Failed to load users");
        return;
      }

      const options = response.data.map((user) => ({
        value: user.userId ?? "",
        text: user.firstName,
      }));

      options.unshift({ value: "", text: "Select User" }); // Add a default option

      setAllUsers(options);
    }

    getAllUsers();
  }, []);

  // Load existing attachments if editing a ticket
  useEffect(() => {
    async function loadExistingAttachments() {
      if (!ticket?.ticketId) return;

      try {
        const response = await getAttachmentsClient("Ticket", ticket.ticketId);
        if (response.isSuccess) {
          setExistingAttachments(response.data || []);
        } else {
          console.error("Failed to load attachments:", response.message);
        }
      } catch (error) {
        console.error("Error loading attachments:", error);
      }
    }

    loadExistingAttachments();
  }, [ticket?.ticketId]);

  // Load existing tags if editing a ticket
  useEffect(() => {
    async function loadExistingTags() {
      if (!ticket?.ticketId) return;

      try {
        const response = await getEntityTagsClient("Ticket", ticket.ticketId);
        if (response.isSuccess) {
          setValue("tags", response.data || []);
        } else {
          console.error("Failed to load tags:", response.message);
        }
      } catch (error) {
        console.error("Error loading tags:", error);
      }
    }

    loadExistingTags();
  }, [ticket?.ticketId, setValue]);

  const onSubmit = async (data: TicketFormData) => {
    console.log("submit", data, ticket);

    const dueDateString =
      data.dueDate instanceof Date
        ? data.dueDate.toISOString().slice(0, 10)
        : data.dueDate;

    const payload = {
      ...data,
      dueDate: dueDateString,
      createdBy: session?.user?.id,
      // Remove attachments and tags from payload as they are handled separately
      attachments: undefined,
      tags: undefined,
    };

    try {
      let ticketId: number | undefined;

      if (ticket?.ticketId) {
        console.log("updating ticket", data);
        const response = await updateTicket({
          ...payload,
          ticketId: ticket.ticketId,
        });

        if (response.isSuccess) {
          toast.success(response.message || "Ticket updated successfully!");
          ticketId = ticket.ticketId;
        } else {
          toast.error(response.message || "Failed to update ticket");
          return;
        }
      } else {
        let response;
        if (ticketType === TicketType.InternalTicket) {
          console.log("creating internal ticket", data);
          response = await createInternalTicket({
            ...payload,
          });
        } else {
          console.log("creating external ticket", data);

          // Create the payload for external ticket with complains
          const externalPayload = {
            ...payload,
            ComplainIdList: complainList?.map((pkg) => pkg.complainId!) || [],
            ...(complainType && { complainType }),
          };

          response = await createTicket(externalPayload);
        }

        if (response.isSuccess) {
          toast.success(response.message || "Ticket created successfully!");
          ticketId = response.data?.ticketId;
        } else {
          toast.error(response.message || "Failed to create ticket");
          return;
        }
      }

      // Handle tags assignment if we have tags and a ticketId
      if (data.tags && data.tags.length > 0 && ticketId) {
        try {
          const tagIds = data.tags.map((tag) => tag.tagId);
          const tagResponse = await assignTagsToEntityClient(
            "Ticket",
            ticketId,
            tagIds
          );
          if (tagResponse.isSuccess) {
            toast.success(`${data.tags.length} tag(s) assigned successfully!`);
          } else {
            toast.error(`Failed to assign tags: ${tagResponse.message}`);
          }
        } catch (error) {
          console.error("Error assigning tags:", error);
          toast.error("Error assigning tags");
        }
      }

      // Upload attachments if any and we have a ticketId
      console.log(
        "Before attachment check - data.attachments:",
        data.attachments
      );
      console.log("data.attachments type:", typeof data.attachments);
      console.log(
        "data.attachments is array:",
        Array.isArray(data.attachments)
      );
      console.log("data.attachments length:", data.attachments?.length);

      if (data.attachments && data.attachments.length > 0 && ticketId) {
        console.log(
          "About to call uploadAttachmentsClient with:",
          data.attachments
        );
        try {
          const attachmentResponse = await uploadAttachmentsClient(
            "Ticket",
            ticketId,
            data.attachments
          );
          if (attachmentResponse.isSuccess) {
            toast.success(
              `${data.attachments.length} attachment(s) uploaded successfully!`
            );
          } else {
            toast.error(
              `Failed to upload attachments: ${attachmentResponse.message}`
            );
          }
        } catch (error) {
          console.error("Error uploading attachments:", error);
          toast.error("Error uploading attachments");
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

          {/* Tags Input */}
          <div className="w-full">
            <TagsInput
              name="tags"
              control={control}
              label="Tags"
              showlabel={true}
              placeholder="Search or create tags..."
              entityType="Ticket"
              entityId={ticket?.ticketId}
              allowCreateNew={true}
              maxTags={10}
              onTagsChange={(tags) => {
                console.log("Tags changed:", tags);
              }}
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
                name="estimation"
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

          {/* Attachment Upload Section */}
          <div className="mt-4">
            <MultipleAttachmentUpload
              name="attachments"
              control={control}
              entityType="Ticket"
              entityId={ticket?.ticketId}
              label="Attachments"
              showlabel={true}
              maxFiles={5}
              maxFileSize={10}
              acceptedFileTypes={[
                ".pdf",
                ".doc",
                ".docx",
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".txt",
              ]}
              allowedCategories={["Document", "Image", "Evidence", "Other"]}
              allowedAttachmentTypes={[
                "Evidence",
                "Report",
                "Specification",
                "Progress",
                "Other",
              ]}
              existingAttachments={existingAttachments}
              onAttachmentsChange={(attachments) => {
                // Optional: Handle attachment changes if needed
                console.log("Attachments changed:", attachments);
              }}
            />
          </div>

          <div className="flex justify-end my-4">
            <Button type="submit">Submit</Button>
          </div>
        </div>
      </form>
    </div>
  );
}
