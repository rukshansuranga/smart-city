"use client";

import { CreateTagRequest, UpdateTagRequest, Tag, ApiResponse } from "@/types";
import { fetchWrapperClient } from "@/lib/fetchWrapperClient";

export async function getAllTagsClient(): Promise<ApiResponse<Tag[]>> {
  try {
    const response = await fetchWrapperClient.get("tags");
    return response;
  } catch (error) {
    console.error("Error fetching tags:", error);
    return {
      isSuccess: false,
      message: "Failed to fetch tags",
      data: [],
      errors: [],
    };
  }
}

export async function getTagByIdClient(id: number): Promise<ApiResponse<Tag>> {
  try {
    const response = await fetchWrapperClient.get(`tags/${id}`);
    return response;
  } catch (error) {
    console.error("Error fetching tag:", error);
    return {
      isSuccess: false,
      message: "Failed to fetch tag",
      data: {} as Tag,
      errors: [],
    };
  }
}

export async function createTagClient(
  tagData: CreateTagRequest
): Promise<ApiResponse<Tag>> {
  try {
    const response = await fetchWrapperClient.post("tags", tagData);
    return response;
  } catch (error) {
    console.error("Error creating tag:", error);
    return {
      isSuccess: false,
      message: "Failed to create tag",
      data: {} as Tag,
      errors: [],
    };
  }
}

export async function updateTagClient(
  id: number,
  tagData: UpdateTagRequest
): Promise<ApiResponse<Tag>> {
  try {
    const response = await fetchWrapperClient.put(`tags/${id}`, tagData);
    return response;
  } catch (error) {
    console.error("Error updating tag:", error);
    return {
      isSuccess: false,
      message: "Failed to update tag",
      data: {} as Tag,
      errors: [],
    };
  }
}

export async function deleteTagClient(
  id: number
): Promise<ApiResponse<boolean>> {
  try {
    const response = await fetchWrapperClient.del(`tags/${id}`);
    return response;
  } catch (error) {
    console.error("Error deleting tag:", error);
    return {
      isSuccess: false,
      message: "Failed to delete tag",
      data: false,
      errors: [],
    };
  }
}

export async function searchTagsClient(
  searchTerm: string
): Promise<ApiResponse<Tag[]>> {
  try {
    const response = await fetchWrapperClient.get(
      `tags/search?searchTerm=${encodeURIComponent(searchTerm)}`
    );
    return response;
  } catch (error) {
    console.error("Error searching tags:", error);
    return {
      isSuccess: false,
      message: "Failed to search tags",
      data: [],
      errors: [],
    };
  }
}

export async function getEntityTagsClient(
  entityType: string,
  entityId: number
): Promise<ApiResponse<Tag[]>> {
  try {
    const response = await fetchWrapperClient.get(
      `tags/entity/${entityType}/${entityId}`
    );
    return response;
  } catch (error) {
    console.error("Error fetching entity tags:", error);
    return {
      isSuccess: false,
      message: "Failed to fetch entity tags",
      data: [],
      errors: [],
    };
  }
}

export async function assignTagsToEntityClient(
  entityType: string,
  entityId: number,
  tagIds: number[]
): Promise<ApiResponse<boolean>> {
  try {
    const response = await fetchWrapperClient.post(
      `tags/entity/${entityType}/${entityId}`,
      tagIds
    );
    return response;
  } catch (error) {
    console.error("Error assigning tags to entity:", error);
    return {
      isSuccess: false,
      message: "Failed to assign tags to entity",
      data: false,
      errors: [],
    };
  }
}

export async function removeTagsFromEntityClient(
  entityType: string,
  entityId: number,
  tagIds: number[]
): Promise<ApiResponse<boolean>> {
  try {
    // Using custom fetch since we need to send body with DELETE request
    const response = await fetch(
      `${process.env.NEXT_PUBLIC_API_URL}/api/Tags/entity/${entityType}/${entityId}`,
      {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(tagIds),
      }
    );

    const result = await response.json();
    return result;
  } catch (error) {
    console.error("Error removing tags from entity:", error);
    return {
      isSuccess: false,
      message: "Failed to remove tags from entity",
      data: false,
      errors: [],
    };
  }
}

export async function removeAllTagsFromEntityClient(
  entityType: string,
  entityId: number
): Promise<ApiResponse<boolean>> {
  try {
    const response = await fetchWrapperClient.del(
      `tags/entity/${entityType}/${entityId}/all`
    );
    return response;
  } catch (error) {
    console.error("Error removing all tags from entity:", error);
    return {
      isSuccess: false,
      message: "Failed to remove all tags from entity",
      data: false,
      errors: [],
    };
  }
}
