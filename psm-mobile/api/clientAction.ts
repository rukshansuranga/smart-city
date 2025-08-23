import { fetchWrapper } from "@/lib/fetchWrapper";
import { Client } from "@/types";

export async function postClient(client: Client): Promise<Client> {
  console.log("Posting client:", client);
  return fetchWrapper.post("client", client);
}
