"use client";

import React from "react";
import { useRouter } from "next/navigation";
import { useRolePermissions } from "../hooks/useRolePermissions";

export default function UnauthorizedPage() {
  const router = useRouter();
  const { userRoles, session } = useRolePermissions();

  const handleGoBack = () => {
    router.back();
  };

  const handleGoHome = () => {
    router.push("/dashboard");
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="max-w-md w-full bg-white shadow-lg rounded-lg p-8 text-center">
        <div className="mb-6">
          <svg
            className="mx-auto h-16 w-16 text-red-500"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z"
            />
          </svg>
        </div>

        <h1 className="text-2xl font-bold text-gray-900 mb-4">Access Denied</h1>

        <p className="text-gray-600 mb-6">
          You don&apos;t have the necessary permissions to access this page.
        </p>

        {session && (
          <div className="mb-6 p-4 bg-gray-100 rounded-lg">
            <h2 className="font-semibold text-sm text-gray-700 mb-2">
              Your Current Roles:
            </h2>
            <div className="flex flex-wrap gap-2 justify-center">
              {userRoles.length > 0 ? (
                userRoles.map((role) => (
                  <span
                    key={role}
                    className="px-3 py-1 bg-blue-100 text-blue-800 text-sm rounded-full"
                  >
                    {role}
                  </span>
                ))
              ) : (
                <span className="text-gray-500 text-sm">No roles assigned</span>
              )}
            </div>
          </div>
        )}

        <div className="space-y-3">
          <button
            onClick={handleGoBack}
            className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-lg transition duration-200"
          >
            Go Back
          </button>

          <button
            onClick={handleGoHome}
            className="w-full bg-gray-600 hover:bg-gray-700 text-white font-medium py-2 px-4 rounded-lg transition duration-200"
          >
            Go to Dashboard
          </button>
        </div>

        <p className="mt-6 text-sm text-gray-500">
          If you believe this is an error, please contact your administrator.
        </p>
      </div>
    </div>
  );
}
