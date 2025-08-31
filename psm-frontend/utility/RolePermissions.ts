export type Role = "admin" | "manager" | "staff" | "contractor" | "councillor";

export type Permission =
  | "dashboard"
  | "board"
  | "complain"
  | "ticket"
  | "projects"
  | "user"
  | "projectprogress";

export type NavigationItem = {
  key: Permission;
  label: string;
  href: string;
  icon: string;
  subItems?: {
    label: string;
    href: string;
    icon: string;
  }[];
};

// Define permissions for each role
export const ROLE_PERMISSIONS: Record<Role, Permission[]> = {
  admin: ["dashboard", "board", "complain", "ticket", "projects", "user"],
  manager: ["dashboard", "board", "complain", "ticket", "projects"],
  staff: ["board", "complain", "ticket", "projects"],
  contractor: ["projectprogress"],
  councillor: ["complain", "projects"],
};

// Define navigation structure
export const NAVIGATION_ITEMS: NavigationItem[] = [
  {
    key: "dashboard",
    label: "Dashboard",
    href: "/dashboard",
    icon: "dashboard",
  },
  {
    key: "board",
    label: "Board",
    href: "/board", // Will be dynamically updated with user ID
    icon: "board",
  },
  {
    key: "complain",
    label: "Complains",
    href: "/complain",
    icon: "complains",
    subItems: [
      {
        label: "Light Post",
        href: "/complain/LightPostComplain",
        icon: "streetlamp",
      },
      {
        label: "General",
        href: "/complain/GeneralComplain",
        icon: "general_complain",
      },
      {
        label: "Project",
        href: "/complain/ProjectComplain",
        icon: "project_complain",
      },
      {
        label: "Garbage",
        href: "/complain/GarbageComplain",
        icon: "garbage_complain",
      },
    ],
  },
  {
    key: "ticket",
    label: "Ticket",
    href: "/ticket",
    icon: "ticket",
    subItems: [
      {
        label: "Tickets",
        href: "/ticket",
        icon: "ticket",
      },
      {
        label: "New Ticket",
        href: "/ticket/new",
        icon: "ticket",
      },
      {
        label: "Resolved",
        href: "/ticket/resolved",
        icon: "ticket",
      },
    ],
  },
  {
    key: "projects",
    label: "Projects",
    href: "/project",
    icon: "projects",
    subItems: [
      {
        label: "New Project",
        href: "/project/new",
        icon: "project-add",
      },
      {
        label: "List",
        href: "/project",
        icon: "project-list",
      },
    ],
  },
  {
    key: "user",
    label: "User",
    href: "/user",
    icon: "user",
  },
  {
    key: "projectprogress",
    label: "Project Progress",
    href: "/project-progress",
    icon: "projects", // You might want to add a specific icon for project progress
  },
];

/**
 * Check if a user has a specific permission
 */
export function hasPermission(
  userRoles: string[],
  permission: Permission
): boolean {
  return userRoles.some((role) => {
    const rolePermissions = ROLE_PERMISSIONS[role as Role];
    return rolePermissions?.includes(permission);
  });
}

/**
 * Get all permissions for user roles
 */
export function getUserPermissions(userRoles: string[]): Permission[] {
  const allPermissions = new Set<Permission>();

  userRoles.forEach((role) => {
    const rolePermissions = ROLE_PERMISSIONS[role as Role];
    if (rolePermissions) {
      rolePermissions.forEach((permission) => allPermissions.add(permission));
    }
  });

  return Array.from(allPermissions);
}

/**
 * Get navigation items that the user has permission to access
 */
export function getAuthorizedNavigation(
  userRoles: string[],
  userId?: string
): NavigationItem[] {
  return NAVIGATION_ITEMS.filter((item) => {
    if (!hasPermission(userRoles, item.key)) {
      return false;
    }

    // Update board href with user ID
    if (item.key === "board" && userId) {
      item.href = `/board/${userId}`;
    }

    return true;
  });
}

/**
 * Check if user has any of the specified roles
 */
export function hasAnyRole(userRoles: string[], roles: Role[]): boolean {
  return userRoles.some((role) => roles.includes(role as Role));
}

/**
 * Check if user has all specified roles
 */
export function hasAllRoles(userRoles: string[], roles: Role[]): boolean {
  return roles.every((role) => userRoles.includes(role));
}
