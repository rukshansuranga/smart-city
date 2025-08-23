import { fetchWrapper } from "@/lib/fetchWrapper";
import { Place } from "react-native-google-places-textinput";

export async function getNearLightPosts(place: Place): Promise<[]> {
  return fetchWrapper.get(
    `workpackage/lightPost/near?latitude=${place.details?.location?.latitude}&longitude=${place.details?.location?.longitude}`
  );
}

export async function addLightPostComplainAsync(complain: {
  lightPostNumber: string;
  clientId: string;
  subject: string;
  detail: string;
}): Promise<void> {
  return fetchWrapper.post("workpackage/lightPost", complain);
}

export async function getListPostsByPostNo(postNo: string): Promise<[]> {
  return fetchWrapper.get(`workpackage/lightPost/${postNo}`);
}

export async function getListPostsSpecificCategoryByPostNoAndName(
  postNo: string,
  name: string
): Promise<[]> {
  return fetchWrapper.get(`workpackage/lightPost/${postNo}/${name}`);
}

export async function GetActiveListPostComplainsByMe(
  postNo: string
): Promise<[]> {
  return fetchWrapper.get(`workpackage/lightPost/me/${postNo}`);
}
