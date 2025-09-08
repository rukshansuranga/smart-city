"use server";

import { fetchWrapper } from "@/lib/fetchWrapper";
import { User } from "@/types";

export async function getUsers(): Promise<User[]> {
  return fetchWrapper.get(`user`);
}
