"use client";

import { useSession } from "next-auth/react";
import {
  hasPermission,
  getUserPermissions,
  hasAnyRole,
  hasAllRoles,
  Permission,
  Role,
} from "../../utility/RolePermissions";

export function useRolePermissions() {
  const { data: session } = useSession();
  const userRoles = session?.roles || [];

  return {
    // Check if user has a specific permission
    hasPermission: (permission: Permission) =>
      hasPermission(userRoles, permission),

    // Get all user permissions
    getUserPermissions: () => getUserPermissions(userRoles),

    // Check if user has any of the specified roles
    hasAnyRole: (roles: Role[]) => hasAnyRole(userRoles, roles),

    // Check if user has all specified roles
    hasAllRoles: (roles: Role[]) => hasAllRoles(userRoles, roles),

    // Get user roles
    userRoles,

    // Get user session
    session,

    // Check if user is admin
    isAdmin: () => userRoles.includes("admin"),

    // Check if user is manager or higher
    isManagerOrHigher: () => hasAnyRole(userRoles, ["admin", "manager"]),

    // Check if user is staff or higher
    isStaffOrHigher: () => hasAnyRole(userRoles, ["admin", "manager", "staff"]),
  };
}
