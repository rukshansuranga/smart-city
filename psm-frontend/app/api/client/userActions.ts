import { fetchWrapperClient } from "@/lib/fetchWrapperClient";
import {
  ApiResponse,
  User,
  Paging,
  CreateUserRequest,
  Council,
  PagingRequest,
} from "@/types";
import { AuthType } from "@/enums";

// Get all councils
export async function getAllCouncils(): Promise<ApiResponse<Council[]>> {
  return fetchWrapperClient.get("misc/councils");
}

// Get council by name
export async function getCouncilByName(
  name: string
): Promise<ApiResponse<Council>> {
  return fetchWrapperClient.get(`misc/councils/${name}`);
}

// Get all users with pagination
export async function getAllUsersPaging(
  pagingRequest?: PagingRequest
): Promise<ApiResponse<Paging<User>>> {
  const params = new URLSearchParams();
  if (pagingRequest) {
    params.append("pageNumber", pagingRequest.pageNumber.toString());
    params.append("pageSize", pagingRequest.pageSize.toString());
  } else {
    params.append("pageNumber", "1");
    params.append("pageSize", "10");
  }
  return fetchWrapperClient.get(`user?${params.toString()}`);
}

// Get user by ID
export async function getUserById(id: string): Promise<ApiResponse<User>> {
  return fetchWrapperClient.get(`user/${id}`);
}

// Get users by user type
export async function getUsersByUserType(
  authTypes: AuthType[]
): Promise<ApiResponse<User[]>> {
  const params = new URLSearchParams();
  authTypes.forEach((authType) => {
    params.append("authTypes", authType.toString());
  });

  return fetchWrapperClient.get(`user/by-user-type?${params.toString()}`);
}

// Create user
export async function createUser(
  userData: CreateUserRequest
): Promise<ApiResponse<User>> {
  return fetchWrapperClient.post("user", userData);
}

// Update user
export async function updateUser(
  id: string,
  userData: Partial<CreateUserRequest>
): Promise<ApiResponse<User>> {
  return fetchWrapperClient.put(`user/${id}`, userData);
}

// Delete user
export async function deleteUser(id: string): Promise<ApiResponse<null>> {
  return fetchWrapperClient.del(`user/${id}`);
}
