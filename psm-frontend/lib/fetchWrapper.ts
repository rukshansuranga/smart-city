import { auth } from "@/auth";

const baseUrl = process.env.API_URL ?? process.env.NEXT_PUBLIC_API_URL;

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
  console.log("fetch options", url, requestOptions);
  return handleResponse(response);
}

async function handleResponse(response: Response) {
  if (response.status === 401) {
    // Redirect to login or show a message
    if (typeof window !== "undefined") {
      window.location.href = "/login"; // or your login route
    }
    // Optionally, return or throw an error
    return { error: { status: 401, message: "Unauthorized" } };
    //throw new Error("Unauthorized");
  }

  if (response.status === 404) {
    return { error: { status: 404, message: "Not Found" } };
  }

  if (response.status === 400) {
    const text = await response.text();
    let data;

    try {
      data = text ? JSON.parse(text) : null;
    } catch {
      data = text;
    }

    console.log("Bad Request:", data);

    return { error: { status: 400, message: "Bad Request", details: data } };
  }

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
  const session = await auth();
  const headers = new Headers();
  headers.set("Content-type", "application/json");
  if (session) {
    headers.set("Authorization", "Bearer " + session.accessToken);
  }

  return headers;
}

export const fetchWrapper = {
  get,
  post,
  put,
  del,
};
