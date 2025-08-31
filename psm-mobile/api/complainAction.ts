import { fetchWrapper } from "@/lib/fetchWrapper";
import { GeneralComplain } from "@/types";

export async function addGeneralComplain(
  complain: Partial<GeneralComplain>
): Promise<GeneralComplain> {
  return fetchWrapper.post("complain/general", complain);
}

export async function GetGeneralComplainPaging(
  page,
  isPrivate,
  pageSize = 10
): Promise<GeneralComplain[]> {
  return fetchWrapper.get(
    `complain/general?pageNumber=${page}&isPrivate=${isPrivate}&pageSize=${pageSize}`
  );
}

export async function deleteGeneralComplain(complainId: number): Promise<void> {
  return fetchWrapper.del(`complain/general/${complainId}`);
}
