"use client";
import { Button, Card, Badge } from "flowbite-react";
import { useRouter } from "next/navigation";
import { HiPencil, HiArrowLeft } from "react-icons/hi2";

import { User } from "@/types";

type UserDetailsProps = {
  user: User;
};

export default function UserDetails({ user }: UserDetailsProps) {
  const router = useRouter();

  const handleEdit = () => {
    router.push(`/user/${user.userId}?mode=edit`);
  };

  const handleBack = () => {
    router.push("/user");
  };

  return (
    <div className="max-w-4xl mx-auto">
      {/* Header */}
      <div className="mb-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <Button
              color="light"
              size="sm"
              onClick={handleBack}
              className="text-gray-600 hover:text-gray-800"
            >
              <HiArrowLeft className="h-4 w-4 mr-1" />
              Back to Users
            </Button>
            <div>
              <h1 className="text-3xl font-bold text-gray-900">
                {`${user.firstName} ${user.lastName}`}
              </h1>
              <p className="text-gray-600 mt-1">User Details</p>
            </div>
          </div>
          <Button
            onClick={handleEdit}
            className="bg-blue-600 hover:bg-blue-700"
          >
            <HiPencil className="mr-2 h-4 w-4" />
            Edit User
          </Button>
        </div>
      </div>

      {/* User Information Cards */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Personal Information */}
        <Card>
          <div className="mb-4">
            <h2 className="text-xl font-semibold text-gray-800">
              Personal Information
            </h2>
          </div>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-600 mb-1">
                Full Name
              </label>
              <p className="text-gray-900 font-medium">
                {`${user.firstName} ${user.lastName}`}
              </p>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-600 mb-1">
                Email
              </label>
              <p className="text-gray-900">{user.email}</p>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-600 mb-1">
                Mobile
              </label>
              <p className="text-gray-900">{user.mobile}</p>
            </div>

            {user.userId && (
              <div>
                <label className="block text-sm font-medium text-gray-600 mb-1">
                  User ID
                </label>
                <p className="text-gray-900 font-mono text-sm">{user.userId}</p>
              </div>
            )}
          </div>
        </Card>

        {/* Address Information */}
        <Card>
          <div className="mb-4">
            <h2 className="text-xl font-semibold text-gray-800">
              Address Information
            </h2>
          </div>
          <div className="space-y-4">
            {user.addressLine1 && (
              <div>
                <label className="block text-sm font-medium text-gray-600 mb-1">
                  Address Line 1
                </label>
                <p className="text-gray-900">{user.addressLine1}</p>
              </div>
            )}

            {user.addressLine2 && (
              <div>
                <label className="block text-sm font-medium text-gray-600 mb-1">
                  Address Line 2
                </label>
                <p className="text-gray-900">{user.addressLine2}</p>
              </div>
            )}

            <div>
              <label className="block text-sm font-medium text-gray-600 mb-1">
                City
              </label>
              <p className="text-gray-900">{user.city}</p>
            </div>
          </div>
        </Card>

        {/* Professional Information */}
        <Card>
          <div className="mb-4">
            <h2 className="text-xl font-semibold text-gray-800">
              Professional Information
            </h2>
          </div>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-600 mb-1">
                Council
              </label>
              <Badge color="info" size="lg">
                {user.council}
              </Badge>
            </div>

            {user.designation && (
              <div>
                <label className="block text-sm font-medium text-gray-600 mb-1">
                  Designation
                </label>
                <p className="text-gray-900">{user.designation}</p>
              </div>
            )}

            {user.authType && (
              <div>
                <label className="block text-sm font-medium text-gray-600 mb-1">
                  Authentication Type
                </label>
                <Badge color="gray" size="sm">
                  {user.authType?.toString() || "N/A"}
                </Badge>
              </div>
            )}
          </div>
        </Card>

        {/* System Information */}
        <Card>
          <div className="mb-4">
            <h2 className="text-xl font-semibold text-gray-800">
              System Information
            </h2>
          </div>
          <div className="space-y-4">
            {user.createdAt && (
              <div>
                <label className="block text-sm font-medium text-gray-600 mb-1">
                  Created At
                </label>
                <p className="text-gray-900">
                  {new Date(user.createdAt).toLocaleString()}
                </p>
              </div>
            )}

            {user.updatedAt && (
              <div>
                <label className="block text-sm font-medium text-gray-600 mb-1">
                  Last Updated
                </label>
                <p className="text-gray-900">
                  {new Date(user.updatedAt).toLocaleString()}
                </p>
              </div>
            )}
          </div>
        </Card>
      </div>
    </div>
  );
}
