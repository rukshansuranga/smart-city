"use client";

import React from "react";
import { useRolePermissions } from "../hooks/useRolePermissions";
import { ProtectedComponent } from "../components/ProtectedComponent";

export function ExampleProtectedPage() {
  const { hasPermission, userRoles, isAdmin } = useRolePermissions();

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Role-Based Access Example</h1>

      {/* Show user info */}
      <div className="mb-6 p-4 bg-gray-100 rounded">
        <h2 className="text-lg font-semibold">User Information</h2>
        <p>Current Roles: {userRoles.join(", ") || "No roles assigned"}</p>
        <p>Is Admin: {isAdmin() ? "Yes" : "No"}</p>
      </div>

      {/* Protected content examples */}
      <div className="space-y-4">
        <ProtectedComponent permission="dashboard">
          <div className="p-4 bg-blue-100 rounded">
            <h3 className="font-semibold">Dashboard Access</h3>
            <p>You can see this because you have dashboard permission.</p>
          </div>
        </ProtectedComponent>

        <ProtectedComponent permission="user">
          <div className="p-4 bg-green-100 rounded">
            <h3 className="font-semibold">User Management</h3>
            <p>You can see this because you have user management permission.</p>
          </div>
        </ProtectedComponent>

        <ProtectedComponent roles={["admin"]}>
          <div className="p-4 bg-red-100 rounded">
            <h3 className="font-semibold">Admin Only</h3>
            <p>You can see this because you are an admin.</p>
          </div>
        </ProtectedComponent>

        <ProtectedComponent
          roles={["manager", "admin"]}
          fallback={
            <div className="p-4 bg-gray-200 rounded">
              <p>Access denied: Manager or Admin role required</p>
            </div>
          }
        >
          <div className="p-4 bg-yellow-100 rounded">
            <h3 className="font-semibold">Manager/Admin Content</h3>
            <p>You can see this because you are a manager or admin.</p>
          </div>
        </ProtectedComponent>

        <ProtectedComponent
          permission="projectprogress"
          fallback={
            <div className="p-4 bg-gray-200 rounded">
              <p>Access denied: Project Progress permission required</p>
            </div>
          }
        >
          <div className="p-4 bg-purple-100 rounded">
            <h3 className="font-semibold">Project Progress</h3>
            <p>
              You can see this because you have project progress permission.
            </p>
          </div>
        </ProtectedComponent>
      </div>

      {/* Role-specific messages */}
      <div className="mt-8 space-y-2">
        <h2 className="text-lg font-semibold">Available Permissions:</h2>
        <ul className="list-disc list-inside space-y-1">
          {hasPermission("dashboard") && <li>Dashboard</li>}
          {hasPermission("board") && <li>Board</li>}
          {hasPermission("complain") && <li>Complaints</li>}
          {hasPermission("ticket") && <li>Tickets</li>}
          {hasPermission("projects") && <li>Projects</li>}
          {hasPermission("user") && <li>User Management</li>}
          {hasPermission("projectprogress") && <li>Project Progress</li>}
        </ul>
      </div>
    </div>
  );
}
