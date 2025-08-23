import { fetchWrapper } from "@/lib/fetchWrapper";

export async function addComment(comment: Partial<Comment>): Promise<Comment> {
  return fetchWrapper.post("comment", comment);
}
