"use client";

import { AttachmentUpload, Attachment, ApiResponse } from "@/types";
import { fetchWrapperClient } from "@/lib/fetchWrapperClient";

// Client-side attachment upload function
export async function uploadAttachmentsClient(
  entityType: string,
  entityId: number,
  attachments: AttachmentUpload[]
): Promise<ApiResponse<Attachment[]>> {
  try {
    // Debug logging
    console.log("Attachments received:", attachments);
    console.log("Attachments length:", attachments?.length);
    console.log("Is attachments an array?", Array.isArray(attachments));

    // Check if attachments is valid
    if (
      !attachments ||
      !Array.isArray(attachments) ||
      attachments.length === 0
    ) {
      console.warn("No valid attachments to process");
      console.log("Validation failed - attachments:", attachments);
      console.log("Validation failed - is array:", Array.isArray(attachments));
      console.log("Validation failed - length:", attachments?.length);
      return {
        isSuccess: false,
        message: "No attachments provided",
        data: [],
        errors: ["No attachments to upload"],
      };
    }

    // Prepare FormData for bulk upload
    const formData = new FormData();

    // Add files and their metadata arrays
    const descriptions: string[] = [];
    const categories: string[] = [];
    const attachmentTypes: string[] = [];

    attachments.forEach((attachment, index) => {
      console.log(`Processing attachment ${index}:`, attachment);

      if (!attachment.file) {
        console.error(`Attachment ${index} has no file:`, attachment);
        throw new Error(`Attachment at index ${index} is missing file`);
      }

      // Append files - the backend expects IEnumerable<IFormFile> Files
      formData.append("Files", attachment.file);

      // Collect metadata for arrays
      descriptions.push(attachment.description || "");
      categories.push(attachment.category || "");
      attachmentTypes.push(attachment.attachmentType || "");
    });

    // Append metadata arrays - the backend expects IEnumerable<string> for each
    descriptions.forEach((desc) => {
      formData.append("Descriptions", desc);
    });

    categories.forEach((cat) => {
      formData.append("Categories", cat);
    });

    attachmentTypes.forEach((type) => {
      formData.append("AttachmentTypes", type);
    });

    // Add optional settings
    formData.append("ContinueOnError", "true");

    // Debug FormData contents
    console.log("FormData contents for bulk upload:");
    for (const pair of formData.entries()) {
      console.log(pair[0], pair[1]);
    }

    console.log(
      `Making bulk upload request to: attachments/${entityType}/${entityId}/bulk`
    );

    const response = await fetchWrapperClient.postFormData(
      `attachments/${entityType}/${entityId}/bulk`,
      formData
    );

    console.log("Bulk upload server response:", response);

    // Transform the bulk upload response to match the expected ApiResponse<Attachment[]> format
    if (response.isSuccess && response.data) {
      const bulkResult = response.data;

      return {
        isSuccess: bulkResult.successCount > 0,
        message:
          bulkResult.summary ||
          `Successfully uploaded ${bulkResult.successCount} of ${bulkResult.totalFiles} files`,
        data: bulkResult.successfulUploads || [],
        errors: bulkResult.hasErrors
          ? bulkResult.errors?.map(
              (err: { FileName: string; Error: string }) =>
                `${err.FileName}: ${err.Error}`
            ) || []
          : [],
      };
    } else {
      return {
        isSuccess: false,
        message: response.message || "Failed to upload attachments",
        data: [],
        errors: response.errors || ["Unknown error occurred"],
      };
    }
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

// Client-side get attachments function
export async function getAttachmentsClient(
  entityType: string,
  entityId: number
): Promise<ApiResponse<Attachment[]>> {
  try {
    const response = await fetchWrapperClient.get(
      `attachments/${entityType}/${entityId}`
    );

    return response;
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
