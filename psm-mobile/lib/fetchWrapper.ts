//import { auth } from "@/auth";

import { useAuthStore } from "@/stores/authStore";

//const baseUrl = process.env.EXPO_PUBLIC_BACKEND_URL;
const baseUrl = "https://d0e910a8d7ed.ngrok-free.app/api/";

async function get(url: string) {
  const requestOptions = {
    method: "GET",
    headers: await getHeaders(),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return handleResponse(response);
}

async function put(url: string, body: unknown) {
  const requestOptions = {
    method: "PUT",
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };
  const response = await fetch(baseUrl + url, requestOptions);
  return handleResponse(response);
}

async function post(url: string, body: unknown) {
  const requestOptions = {
    method: "POST",
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };
  const response = await fetch(baseUrl + url, requestOptions);
  return handleResponse(response);
}

async function del(url: string) {
  const requestOptions = {
    method: "DELETE",
    headers: await getHeaders(),
  };
  const response = await fetch(baseUrl + url, requestOptions);
  return handleResponse(response);
}

async function handleResponse(response: Response) {
  const text = await response.text();
  let data;

  try {
    data = text ? JSON.parse(text) : null;
  } catch {
    data = text;
  }

  if (response.ok) {
    return data || response.statusText;
  } else {
    const error = {
      status: response.status,
      message: typeof data === "string" ? data : response.statusText,
    };
    return { error };
  }
}

async function getHeaders(): Promise<Headers> {
  const { accessToken } = useAuthStore.getState(); // Use getState() for non-component usage
  //const session = await auth();
  const headers = new Headers();
  headers.set("Content-type", "application/json");
  // console.log("get headers accessToken:", accessToken);
  if (accessToken) {
    headers.set("Authorization", "Bearer " + accessToken);
  }
  return headers;
}

export const fetchWrapper = {
  get,
  post,
  put,
  del,
};
