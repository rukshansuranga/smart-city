import { fetchWrapperClient } from "@/lib/fetchWrapperClient";

export async function uploadFile(
  data: Record<string, unknown>
): Promise<string> {
  try {
    const result = await fetchWrapperClient.postFormData("project", data);
    return result.fileUrl || result; // assuming the API returns { fileUrl: string } or just the URL
  } catch (error) {
    console.error("Error uploading file:", error);
    throw error;
  }
}
