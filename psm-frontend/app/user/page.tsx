"use client";
import { useEffect, useState, useCallback } from "react";
import { Button, Spinner, Badge, Pagination } from "flowbite-react";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";
import { HiPlus, HiPencil, HiTrash, HiEye } from "react-icons/hi2";

import { User, PagingRequest } from "@/types";
import { getAllUsersPaging, deleteUser } from "@/app/api/client/userActions";

export default function UserListPage() {
  const router = useRouter();
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [deleting, setDeleting] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(10);
  const [totalItems, setTotalItems] = useState(0);

  const loadUsers = useCallback(
    async (pageNumber: number = currentPage) => {
      try {
        setLoading(true);
        const pagingRequest: PagingRequest = {
          pageNumber,
          pageSize,
        };
        const response = await getAllUsersPaging(pagingRequest);
        if (response.isSuccess) {
          setUsers(response.data.records);
          setTotalItems(response.data.totalItems);
          setCurrentPage(pageNumber);
        } else {
          toast.error(response.message || "Failed to load users");
        }
      } catch (error) {
        console.error("Error loading users:", error);
        toast.error("Failed to load users");
      } finally {
        setLoading(false);
      }
    },
    [currentPage, pageSize]
  );

  useEffect(() => {
    loadUsers(1);
  }, [loadUsers]);

  const handlePageChange = (page: number) => {
    if (page !== currentPage) {
      loadUsers(page);
    }
  };

  const handleDelete = async (userId: string) => {
    if (!confirm("Are you sure you want to delete this user?")) {
      return;
    }

    try {
      setDeleting(userId);
      const response = await deleteUser(userId);
      if (response.isSuccess) {
        toast.success("User deleted successfully!");
        await loadUsers(currentPage); // Reload the current page
      } else {
        toast.error(response.message || "Failed to delete user");
      }
    } catch (error) {
      console.error("Error deleting user:", error);
      toast.error("Failed to delete user");
    } finally {
      setDeleting(null);
    }
  };

  const handleView = (userId: string) => {
    router.push(`/user/${userId}`);
  };

  const handleEdit = (userId: string) => {
    router.push(`/user/${userId}?mode=edit`);
  };

  const handleCreateNew = () => {
    router.push("/user/new");
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-64">
        <Spinner size="xl" />
        <span className="ml-3 text-lg">Loading users...</span>
      </div>
    );
  }

  return (
    <div className="p-6">
      {/* Header */}
      <div className="mb-6">
        <div className="flex justify-between items-center">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">Users</h1>
            <p className="text-gray-600 mt-1">Manage system users</p>
          </div>
          <Button
            onClick={handleCreateNew}
            className="bg-blue-600 hover:bg-blue-700"
          >
            <HiPlus className="mr-2 h-4 w-4" />
            Add New User
          </Button>
        </div>
      </div>

      {/* Users Table */}
      {users.length === 0 ? (
        <div className="bg-white rounded-lg shadow-md p-8 text-center">
          <div className="text-gray-500 text-lg">No users found</div>
          <p className="text-gray-400 mt-2">
            Get started by creating your first user
          </p>
          <Button
            onClick={handleCreateNew}
            className="mt-4 bg-blue-600 hover:bg-blue-700"
          >
            <HiPlus className="mr-2 h-4 w-4" />
            Create First User
          </Button>
        </div>
      ) : (
        <div className="bg-white rounded-lg shadow-md overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Name
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Email
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Mobile
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  City
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Council
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Designation
                </th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {users.map((user) => (
                <tr key={user.userId} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div>
                      <div className="text-sm font-medium text-gray-900">
                        {`${user.firstName} ${user.lastName}`}
                      </div>
                      {user.userId && (
                        <div className="text-sm text-gray-500">
                          ID: {user.userId}
                        </div>
                      )}
                    </div>
                  </td>

                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-sm text-gray-900">{user.email}</div>
                  </td>

                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-sm text-gray-900">{user.mobile}</div>
                  </td>

                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-sm text-gray-900">{user.city}</div>
                  </td>

                  <td className="px-6 py-4 whitespace-nowrap">
                    <Badge color="info" size="sm">
                      {user.council}
                    </Badge>
                  </td>

                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-sm text-gray-900">
                      {user.designation || "N/A"}
                    </div>
                  </td>

                  <td className="px-6 py-4 whitespace-nowrap text-center">
                    <div className="flex items-center justify-center gap-2">
                      <Button
                        size="sm"
                        color="light"
                        onClick={() => handleView(user.userId!)}
                        className="text-blue-600 hover:text-blue-800"
                      >
                        <HiEye className="h-4 w-4" />
                      </Button>

                      <Button
                        size="sm"
                        color="light"
                        onClick={() => handleEdit(user.userId!)}
                        className="text-yellow-600 hover:text-yellow-800"
                      >
                        <HiPencil className="h-4 w-4" />
                      </Button>

                      <Button
                        size="sm"
                        color="light"
                        onClick={() => handleDelete(user.userId!)}
                        disabled={deleting === user.userId}
                        className="text-red-600 hover:text-red-800"
                      >
                        {deleting === user.userId ? (
                          <Spinner size="sm" />
                        ) : (
                          <HiTrash className="h-4 w-4" />
                        )}
                      </Button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Pagination */}
      {totalItems > pageSize && (
        <div className="flex justify-center mt-6">
          <Pagination
            currentPage={currentPage}
            totalPages={Math.ceil(totalItems / pageSize)}
            onPageChange={handlePageChange}
            showIcons
            theme={{
              pages: {
                base: "xs:mt-0 mt-2 inline-flex items-center -space-x-px",
                showIcon: "inline-flex",
                previous: {
                  base: "ml-0 rounded-l-lg border border-gray-300 bg-white py-2 px-3 leading-tight text-gray-500 hover:bg-gray-100 hover:text-gray-700",
                  icon: "h-5 w-5",
                },
                next: {
                  base: "rounded-r-lg border border-gray-300 bg-white py-2 px-3 leading-tight text-gray-500 hover:bg-gray-100 hover:text-gray-700",
                  icon: "h-5 w-5",
                },
                selector: {
                  base: "w-12 border border-gray-300 bg-white py-2 leading-tight text-gray-500 hover:bg-gray-100 hover:text-gray-700",
                  active:
                    "bg-blue-50 text-blue-600 hover:bg-blue-100 hover:text-blue-700 border-blue-300",
                  disabled: "opacity-50 cursor-normal",
                },
              },
            }}
          />
        </div>
      )}

      {/* Summary */}
      <div className="mt-4 text-sm text-gray-600 flex justify-between items-center">
        <span>
          Showing {Math.min((currentPage - 1) * pageSize + 1, totalItems)} to{" "}
          {Math.min(currentPage * pageSize, totalItems)} of {totalItems} users
        </span>
        <span>
          Page {currentPage} of {Math.ceil(totalItems / pageSize)} | Page Size:{" "}
          {pageSize}
        </span>
      </div>
    </div>
  );
}
