import { getSession } from "next-auth/react";

const baseUrl = process.env.NEXT_PUBLIC_API_URL;

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

async function patch(url: string, body: unknown) {
  const requestOptions = {
    method: "PATCH",
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

// Helper function to convert object to FormData
function objectToFormData(obj: Record<string, unknown>): FormData {
  const formData = new FormData();

  Object.keys(obj).forEach((key) => {
    const value = obj[key];

    if (value instanceof File) {
      // Handle File objects
      formData.append(key, value);
    } else if (value instanceof Date) {
      // Handle Date objects - convert to ISO string
      formData.append(key, value.toISOString());
    } else if (Array.isArray(value)) {
      // Handle arrays - append each item with array notation
      value.forEach((item, index) => {
        if (item instanceof File) {
          formData.append(`${key}[${index}]`, item);
        } else if (item !== null && item !== undefined) {
          formData.append(`${key}[${index}]`, String(item));
        }
      });
    } else if (value !== null && value !== undefined) {
      // Handle other values (strings, numbers, booleans)
      formData.append(key, String(value));
    }
    // Skip null/undefined values
  });

  return formData;
}

// Generic method to post FormData
async function postFormData(
  url: string,
  data: FormData | Record<string, unknown>
) {
  const formData = data instanceof FormData ? data : objectToFormData(data);

  const requestOptions = {
    method: "POST",
    headers: await getFormDataHeaders(),
    body: formData,
  };
  const response = await fetch(baseUrl + url, requestOptions);
  return handleResponse(response);
}

// Generic method to put FormData
async function putFormData(
  url: string,
  data: FormData | Record<string, unknown>
) {
  const formData = data instanceof FormData ? data : objectToFormData(data);

  const requestOptions = {
    method: "PUT",
    headers: await getFormDataHeaders(),
    body: formData,
  };
  const response = await fetch(baseUrl + url, requestOptions);
  return handleResponse(response);
}

// Generic method to patch FormData
async function patchFormData(
  url: string,
  data: FormData | Record<string, unknown>
) {
  const formData = data instanceof FormData ? data : objectToFormData(data);

  const requestOptions = {
    method: "PATCH",
    headers: await getFormDataHeaders(),
    body: formData,
  };
  const response = await fetch(baseUrl + url, requestOptions);
  return handleResponse(response);
}

async function handleResponse(response: Response) {
  if (response.status === 401) {
    if (typeof window !== "undefined") {
      window.location.href = "/login";
    }
    return {
      isSuccess: false,
      message: "Unauthorized",
      data: null,
      errors: ["Unauthorized access"],
    };
  }

  if (response.status === 404) {
    return {
      isSuccess: false,
      message: "Not Found",
      data: null,
      errors: ["Resource not found"],
    };
  }

  const text = await response.text();
  let data;

  try {
    data = text ? JSON.parse(text) : null;
  } catch {
    // If parsing fails, treat as error response
    return {
      isSuccess: false,
      message: "Invalid response format",
      data: null,
      errors: [text || "Invalid JSON response"],
    };
  }

  if (response.ok) {
    // For successful responses, return the parsed data as is
    // (it should already be in ApiResponse format from the backend)
    return (
      data || {
        isSuccess: true,
        message: "Success",
        data: null,
        errors: [],
      }
    );
  } else {
    // For error responses, try to extract ApiResponse format
    // If data is already ApiResponse format, return it
    if (data && typeof data === "object" && "isSuccess" in data) {
      return data;
    }

    // Otherwise, wrap in ApiResponse format
    return {
      isSuccess: false,
      message: typeof data === "string" ? data : response.statusText,
      data: null,
      errors: [typeof data === "string" ? data : response.statusText],
    };
  }
}

async function del(url: string) {
  const requestOptions = {
    method: "DELETE",
    headers: await getHeaders(),
  };
  const response = await fetch(baseUrl + url, requestOptions);
  return handleResponse(response);
}

async function getHeaders(): Promise<Headers> {
  const session = await getSession();
  const headers = new Headers();
  console.log("Session in fetchWrapperClient:", session);
  headers.set("Content-type", "application/json");
  if (session?.accessToken) {
    headers.set("Authorization", `Bearer ${session.accessToken}`);
  }
  return headers;
}

// Headers for FormData requests (don't set Content-Type, let browser set it with boundary)
async function getFormDataHeaders(): Promise<Headers> {
  const session = await getSession();
  const headers = new Headers();
  console.log("Session in fetchWrapperClient:", session);
  // Don't set Content-Type for FormData - browser will set it automatically with boundary
  if (session?.accessToken) {
    headers.set("Authorization", `Bearer ${session.accessToken}`);
  }
  return headers;
}

export const fetchWrapperClient = {
  get,
  post,
  put,
  patch,
  del,
  postFormData,
  putFormData,
  patchFormData,
  objectToFormData,
};
