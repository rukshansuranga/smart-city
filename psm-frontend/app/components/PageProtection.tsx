"use client";

import React, { useEffect } from "react";
import { useRouter } from "next/navigation";
import { useSession } from "next-auth/react";
import { useRolePermissions } from "../hooks/useRolePermissions";
import { Permission, Role } from "../../utility/RolePermissions";

interface PageProtectionProps {
  children: React.ReactNode;
  permission?: Permission;
  roles?: Role[];
  requireAll?: boolean;
  redirectTo?: string;
  loading?: React.ReactNode;
  unauthorized?: React.ReactNode;
}

/**
 * Component that protects entire pages based on user permissions
 * Redirects unauthorized users or shows fallback content
 */
export function PageProtection({
  children,
  permission,
  roles,
  requireAll = false,
  redirectTo = "/unauthorized",
  loading,
  unauthorized,
}: PageProtectionProps) {
  const { data: session, status } = useSession();
  const { hasPermission, hasAnyRole, hasAllRoles } = useRolePermissions();
  const router = useRouter();

  useEffect(() => {
    // Don't check permissions while session is loading
    if (status === "loading") return;

    // If no session and we need authentication, redirect to login
    if (status === "unauthenticated") {
      router.push("/login");
      return;
    }

    // If we have a session, check permissions
    if (status === "authenticated") {
      let hasAccess = true;

      // Check permission if specified
      if (permission && !hasPermission(permission)) {
        hasAccess = false;
      }

      // Check roles if specified
      if (roles && roles.length > 0) {
        const hasRequiredRoles = requireAll
          ? hasAllRoles(roles)
          : hasAnyRole(roles);

        if (!hasRequiredRoles) {
          hasAccess = false;
        }
      }

      // Redirect if no access
      if (!hasAccess) {
        router.push(redirectTo);
        return;
      }
    }
  }, [
    status,
    session,
    hasPermission,
    hasAnyRole,
    hasAllRoles,
    permission,
    roles,
    requireAll,
    redirectTo,
    router,
  ]);

  // Show loading while session is being fetched
  if (status === "loading") {
    return (
      loading || (
        <div className="flex items-center justify-center min-h-screen">
          <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-gray-900"></div>
        </div>
      )
    );
  }

  // Show unauthorized if not authenticated
  if (status === "unauthenticated") {
    return (
      unauthorized || (
        <div className="flex items-center justify-center min-h-screen">
          <div className="text-center">
            <h1 className="text-2xl font-bold mb-4">Access Denied</h1>
            <p>Please log in to access this page.</p>
          </div>
        </div>
      )
    );
  }

  // Check permissions for authenticated users
  if (status === "authenticated") {
    let hasAccess = true;

    // Check permission if specified
    if (permission && !hasPermission(permission)) {
      hasAccess = false;
    }

    // Check roles if specified
    if (roles && roles.length > 0) {
      const hasRequiredRoles = requireAll
        ? hasAllRoles(roles)
        : hasAnyRole(roles);

      if (!hasRequiredRoles) {
        hasAccess = false;
      }
    }

    if (!hasAccess) {
      return (
        unauthorized || (
          <div className="flex items-center justify-center min-h-screen">
            <div className="text-center">
              <h1 className="text-2xl font-bold mb-4">Access Denied</h1>
              <p>You don&apos;t have permission to access this page.</p>
              <p className="mt-2 text-gray-600">
                Required: {permission && `Permission: ${permission}`}
                {permission && roles && " and "}
                {roles && `Roles: ${roles.join(", ")}`}
              </p>
            </div>
          </div>
        )
      );
    }
  }

  return <>{children}</>;
}

/**
 * Higher-order component for protecting pages
 */
export function withPageProtection<T extends object>(
  Component: React.ComponentType<T>,
  protectionConfig: Omit<PageProtectionProps, "children">
) {
  const ProtectedPage = (props: T) => {
    return (
      <PageProtection {...protectionConfig}>
        <Component {...props} />
      </PageProtection>
    );
  };

  ProtectedPage.displayName = `withPageProtection(${
    Component.displayName || Component.name
  })`;

  return ProtectedPage;
}
