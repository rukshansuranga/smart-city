"use server";

import { ApiResponse, Attachment, AttachmentUpload } from "@/types";
import { auth } from "@/auth";

const baseUrl = process.env.API_URL ?? process.env.NEXT_PUBLIC_API_URL;

async function getHeaders(isFormData = false): Promise<Headers> {
  const session = await auth();
  const headers = new Headers();

  if (!isFormData) {
    headers.set("Content-Type", "application/json");
  }

  if (session) {
    headers.set("Authorization", "Bearer " + session.accessToken);
  }

  return headers;
}

// Upload attachments for an entity
export async function uploadAttachments(
  entityType: string,
  entityId: number,
  attachments: AttachmentUpload[]
): Promise<ApiResponse<Attachment[]>> {
  try {
    const formData = new FormData();

    attachments.forEach((attachment, index) => {
      formData.append(`files`, attachment.file);
      formData.append(`descriptions[${index}]`, attachment.description || "");
      formData.append(
        `attachmentTypes[${index}]`,
        attachment.attachmentType || ""
      );
      formData.append(`categories[${index}]`, attachment.category || "");
    });

    const response = await fetch(
      `${baseUrl}/api/attachments/${entityType}/${entityId}`,
      {
        method: "POST",
        headers: await getHeaders(true),
        body: formData,
      }
    );

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Error uploading attachments:", error);
    return {
      isSuccess: false,
      message: "Failed to upload attachments",
      data: [],
      errors: [error instanceof Error ? error.message : "Unknown error"],
    };
  }
}

// Get all attachments for an entity
export async function getAttachments(
  entityType: string,
  entityId: number
): Promise<ApiResponse<Attachment[]>> {
  try {
    const response = await fetch(
      `${baseUrl}/api/attachments/${entityType}/${entityId}`,
      {
        method: "GET",
        headers: await getHeaders(),
      }
    );

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Error fetching attachments:", error);
    return {
      isSuccess: false,
      message: "Failed to fetch attachments",
      data: [],
      errors: [error instanceof Error ? error.message : "Unknown error"],
    };
  }
}

// Download specific attachment
export async function downloadAttachment(
  attachmentId: number
): Promise<Response> {
  try {
    const response = await fetch(
      `${baseUrl}/api/attachments/${attachmentId}/download`,
      {
        method: "GET",
        headers: await getHeaders(),
      }
    );

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return response;
  } catch (error) {
    console.error("Error downloading attachment:", error);
    throw error;
  }
}

// Delete attachment
export async function deleteAttachment(
  attachmentId: number
): Promise<ApiResponse<boolean>> {
  try {
    const response = await fetch(`${baseUrl}/api/attachments/${attachmentId}`, {
      method: "DELETE",
      headers: await getHeaders(),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Error deleting attachment:", error);
    return {
      isSuccess: false,
      message: "Failed to delete attachment",
      data: false,
      errors: [error instanceof Error ? error.message : "Unknown error"],
    };
  }
}
