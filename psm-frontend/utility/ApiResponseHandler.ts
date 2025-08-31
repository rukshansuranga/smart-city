import { ApiResponse } from "@/types";
import toast from "react-hot-toast";

/**
 * Handles API responses with the new backend structure
 * Shows toast notifications for errors and returns the data if successful
 */
export function handleApiResponse<T>(response: ApiResponse<T>): T | null {
  if (response.isSuccess) {
    return response.data;
  } else {
    // Show error messages
    if (response.errors && response.errors.length > 0) {
      response.errors.forEach((error: string) => {
        toast.error(error);
      });
    } else {
      toast.error(response.message || "An error occurred");
    }
    return null;
  }
}

/**
 * Handles API responses with the new backend structure for async operations
 * Shows toast notifications and throws errors for catch blocks
 */
export function handleApiResponseWithThrow<T>(response: ApiResponse<T>): T {
  if (response.isSuccess) {
    return response.data;
  } else {
    // Show error messages
    if (response.errors && response.errors.length > 0) {
      response.errors.forEach((error: string) => {
        toast.error(error);
      });
    } else {
      toast.error(response.message || "An error occurred");
    }

    // Throw an error to be caught by the calling function
    const errorMessage =
      response.errors?.join(", ") || response.message || "An error occurred";
    throw new Error(errorMessage);
  }
}
