"use client";

import React from "react";
import { useRolePermissions } from "../hooks/useRolePermissions";
import { Permission, Role } from "../../utility/RolePermissions";

interface RoleGateProps {
  children: React.ReactNode;
  permission?: Permission;
  roles?: Role[];
  requireAll?: boolean;
  fallback?: React.ReactNode;
  className?: string;
}

/**
 * Simple role gate component for inline permission checking
 */
export function RoleGate({
  children,
  permission,
  roles,
  requireAll = false,
  fallback = null,
  className,
}: RoleGateProps) {
  const { hasPermission, hasAnyRole, hasAllRoles } = useRolePermissions();

  // Check permission if specified
  if (permission && !hasPermission(permission)) {
    return <div className={className}>{fallback}</div>;
  }

  // Check roles if specified
  if (roles && roles.length > 0) {
    const hasRequiredRoles = requireAll
      ? hasAllRoles(roles)
      : hasAnyRole(roles);

    if (!hasRequiredRoles) {
      return <div className={className}>{fallback}</div>;
    }
  }

  return <div className={className}>{children}</div>;
}

interface AdminOnlyProps {
  children: React.ReactNode;
  fallback?: React.ReactNode;
  className?: string;
}

/**
 * Shortcut component for admin-only content
 */
export function AdminOnly({
  children,
  fallback = null,
  className,
}: AdminOnlyProps) {
  return (
    <RoleGate roles={["admin"]} fallback={fallback} className={className}>
      {children}
    </RoleGate>
  );
}

interface ManagerOrAdminOnlyProps {
  children: React.ReactNode;
  fallback?: React.ReactNode;
  className?: string;
}

/**
 * Shortcut component for manager or admin content
 */
export function ManagerOrAdminOnly({
  children,
  fallback = null,
  className,
}: ManagerOrAdminOnlyProps) {
  return (
    <RoleGate
      roles={["manager", "admin"]}
      fallback={fallback}
      className={className}
    >
      {children}
    </RoleGate>
  );
}

interface StaffOrHigherOnlyProps {
  children: React.ReactNode;
  fallback?: React.ReactNode;
  className?: string;
}

/**
 * Shortcut component for staff or higher access
 */
export function StaffOrHigherOnly({
  children,
  fallback = null,
  className,
}: StaffOrHigherOnlyProps) {
  return (
    <RoleGate
      roles={["staff", "manager", "admin"]}
      fallback={fallback}
      className={className}
    >
      {children}
    </RoleGate>
  );
}

/**
 * Component that shows different content based on user role
 */
interface RoleBasedContentProps {
  admin?: React.ReactNode;
  manager?: React.ReactNode;
  staff?: React.ReactNode;
  contractor?: React.ReactNode;
  councillor?: React.ReactNode;
  fallback?: React.ReactNode;
  className?: string;
}

export function RoleBasedContent({
  admin,
  manager,
  staff,
  contractor,
  councillor,
  fallback = null,
  className,
}: RoleBasedContentProps) {
  const { userRoles } = useRolePermissions();

  // Show content based on highest role (priority order)
  if (userRoles.includes("admin") && admin) {
    return <div className={className}>{admin}</div>;
  }

  if (userRoles.includes("manager") && manager) {
    return <div className={className}>{manager}</div>;
  }

  if (userRoles.includes("staff") && staff) {
    return <div className={className}>{staff}</div>;
  }

  if (userRoles.includes("contractor") && contractor) {
    return <div className={className}>{contractor}</div>;
  }

  if (userRoles.includes("councillor") && councillor) {
    return <div className={className}>{councillor}</div>;
  }

  return <div className={className}>{fallback}</div>;
}
