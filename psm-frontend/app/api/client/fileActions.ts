import { fetchWrapperClient } from "@/lib/fetchWrapperClient";
import { ApiResponse } from "@/types";

export async function uploadFile(
  data: Record<string, unknown>
): Promise<ApiResponse<string>> {
  try {
    const result: ApiResponse<{ fileUrl: string }> =
      await fetchWrapperClient.postFormData("project", data);
    if (result.isSuccess && result.data) {
      return {
        isSuccess: true,
        message: result.message,
        data: result.data.fileUrl || (result.data as unknown as string),
        errors: [],
      };
    }
    return result as ApiResponse<string>;
  } catch (error) {
    console.error("Error uploading file:", error);
    throw error;
  }
}
