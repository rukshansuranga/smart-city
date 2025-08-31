"use client";

import React from "react";
import { useRolePermissions } from "../hooks/useRolePermissions";
import { Permission, Role } from "../../utility/RolePermissions";

interface ProtectedComponentProps {
  children: React.ReactNode;
  permission?: Permission;
  roles?: Role[];
  requireAll?: boolean; // If true, user must have ALL specified roles
  fallback?: React.ReactNode;
}

/**
 * Component that conditionally renders children based on user permissions
 */
export function ProtectedComponent({
  children,
  permission,
  roles,
  requireAll = false,
  fallback = null,
}: ProtectedComponentProps) {
  const { hasPermission, hasAnyRole, hasAllRoles } = useRolePermissions();

  // Check permission if specified
  if (permission && !hasPermission(permission)) {
    return <>{fallback}</>;
  }

  // Check roles if specified
  if (roles && roles.length > 0) {
    const hasRequiredRoles = requireAll
      ? hasAllRoles(roles)
      : hasAnyRole(roles);

    if (!hasRequiredRoles) {
      return <>{fallback}</>;
    }
  }

  return <>{children}</>;
}

interface WithPermissionProps {
  permission?: Permission;
  roles?: Role[];
  requireAll?: boolean;
  fallback?: React.ReactNode;
}

/**
 * Higher-order component for protecting components based on permissions
 */
export function withPermission<T extends object>(
  Component: React.ComponentType<T>,
  {
    permission,
    roles,
    requireAll = false,
    fallback = null,
  }: WithPermissionProps
) {
  const ProtectedWrapper = (props: T) => {
    return (
      <ProtectedComponent
        permission={permission}
        roles={roles}
        requireAll={requireAll}
        fallback={fallback}
      >
        <Component {...props} />
      </ProtectedComponent>
    );
  };

  ProtectedWrapper.displayName = `withPermission(${
    Component.displayName || Component.name
  })`;

  return ProtectedWrapper;
}
