import { fetchWrapper } from "@/lib/fetchWrapper";
import { GeneralComplain, Notification } from "@/types";

export async function addGeneralComplain(
  complain: Partial<GeneralComplain>
): Promise<GeneralComplain> {
  return fetchWrapper.post("workpackage/general", complain);
}

export async function GetGeneralComplainPaging(
  page,
  isPrivate,
  pageSize = 10
): Promise<GeneralComplain[]> {
  return fetchWrapper.get(
    `workpackage/general?pageNumber=${page}&isPrivate=${isPrivate}&pageSize=${pageSize}`
  );
}

export async function deleteGeneralComplain(
  workpackageId: number
): Promise<void> {
  return fetchWrapper.del(`workpackage/general/${workpackageId}`);
}
