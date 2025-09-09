# Role-Based Authorization System

This document describes the role-based authorization system implemented in the PSM Frontend application.

## Overview

The system provides fine-grained access control based on user roles and permissions. It includes:

- Role-based sidebar navigation
- Component-level permission checks
- Page-level protection
- Utility hooks and components

## Roles and Permissions

### Available Roles

- **admin**: Full system access
- **manager**: Management-level access
- **staff**: Operational access
- **contractor**: Project progress access only
- **councillor**: Complain and project viewing access

### Permission Matrix

| Permission      | Admin | Manager | Staff | Contractor | Councillor |
| --------------- | ----- | ------- | ----- | ---------- | ---------- |
| dashboard       | ✅    | ✅      | ❌    | ❌         | ❌         |
| board           | ✅    | ✅      | ✅    | ❌         | ❌         |
| complain        | ✅    | ✅      | ✅    | ❌         | ✅         |
| ticket          | ✅    | ✅      | ✅    | ❌         | ❌         |
| projects        | ✅    | ✅      | ✅    | ❌         | ✅         |
| user            | ✅    | ❌      | ❌    | ❌         | ❌         |
| projectprogress | ❌    | ❌      | ❌    | ✅         | ❌         |

## Core Components

### 1. Role Permissions Utility (`utility/RolePermissions.ts`)

Central configuration for roles, permissions, and navigation items.

```typescript
import { hasPermission, getUserPermissions } from "../utility/RolePermissions";

// Check if user has specific permission
const canAccessDashboard = hasPermission(userRoles, "dashboard");

// Get all user permissions
const permissions = getUserPermissions(userRoles);
```

### 2. Role-Based Sidebar (`app/sidebar/RoleBasedSidebar.tsx`)

Automatically generates navigation based on user permissions.

```tsx
import { RoleBasedSidebar } from "./sidebar/RoleBasedSidebar";

<RoleBasedSidebar session={session} />;
```

### 3. Permission Hook (`app/hooks/useRolePermissions.ts`)

React hook for accessing permission utilities in components.

```tsx
import { useRolePermissions } from "../hooks/useRolePermissions";

function MyComponent() {
  const { hasPermission, isAdmin, isManagerOrHigher, userRoles } =
    useRolePermissions();

  if (!hasPermission("dashboard")) {
    return <div>Access denied</div>;
  }

  return <div>Dashboard content</div>;
}
```

## Component-Level Protection

### 1. Protected Component (`app/components/ProtectedComponent.tsx`)

Conditionally render content based on permissions.

```tsx
import { ProtectedComponent } from '../components/ProtectedComponent';

<ProtectedComponent permission="user">
  <UserManagement />
</ProtectedComponent>

<ProtectedComponent roles={['admin', 'manager']}>
  <AdminPanel />
</ProtectedComponent>
```

### 2. Role Gates (`app/components/RoleGates.tsx`)

Convenient shortcuts for common permission patterns.

```tsx
import { AdminOnly, ManagerOrAdminOnly, RoleBasedContent } from '../components/RoleGates';

<AdminOnly>
  <AdminSettings />
</AdminOnly>

<ManagerOrAdminOnly fallback={<div>Access denied</div>}>
  <ManagementTools />
</ManagerOrAdminOnly>

<RoleBasedContent
  admin={<AdminView />}
  manager={<ManagerView />}
  staff={<StaffView />}
  fallback={<GuestView />}
/>
```

## Page-Level Protection

### Page Protection Component (`app/components/PageProtection.tsx`)

Protect entire pages with automatic redirects.

```tsx
import { PageProtection } from "../components/PageProtection";

export default function AdminPage() {
  return (
    <PageProtection roles={["admin"]} redirectTo="/unauthorized">
      <div>Admin-only content</div>
    </PageProtection>
  );
}
```

### Higher-Order Component

```tsx
import { withPageProtection } from "../components/PageProtection";

const ProtectedPage = withPageProtection(MyPage, {
  permission: "user",
  redirectTo: "/unauthorized",
});
```

## Usage Examples

### 1. Conditional Rendering in Components

```tsx
function Dashboard() {
  const { hasPermission, userRoles } = useRolePermissions();

  return (
    <div>
      <h1>Dashboard</h1>

      {hasPermission("user") && <UserManagementSection />}

      {userRoles.includes("admin") && <AdminControls />}
    </div>
  );
}
```

### 2. Protecting Routes

```tsx
// pages/admin/users.tsx
export default function UsersPage() {
  return (
    <PageProtection permission="user">
      <div>
        <h1>User Management</h1>
        {/* User management content */}
      </div>
    </PageProtection>
  );
}
```

### 3. Dynamic Navigation

```tsx
function CustomNavigation() {
  const { hasPermission } = useRolePermissions();

  return (
    <nav>
      {hasPermission("dashboard") && <Link href="/dashboard">Dashboard</Link>}
      {hasPermission("projects") && <Link href="/projects">Projects</Link>}
    </nav>
  );
}
```

## Session Structure

The system expects the session to have the following structure:

```typescript
interface Session {
  user: {
    id: string;
  };
  roles: string[]; // Array of role strings
  // ... other session properties
}
```

## Best Practices

1. **Use Permission-Based Checks**: Prefer checking permissions over roles when possible
2. **Provide Fallbacks**: Always provide appropriate fallback content for unauthorized users
3. **Centralize Permissions**: Keep all permission definitions in the `RolePermissions.ts` file
4. **Use TypeScript**: Take advantage of type safety for roles and permissions
5. **Test Different Roles**: Ensure your components work correctly for all role combinations

## Customization

### Adding New Roles

1. Update the `Role` type in `utility/RolePermissions.ts`
2. Add the new role to `ROLE_PERMISSIONS` mapping
3. Update any role-specific components

### Adding New Permissions

1. Update the `Permission` type
2. Add the permission to relevant roles in `ROLE_PERMISSIONS`
3. Add navigation item to `NAVIGATION_ITEMS` if needed

### Custom Navigation Items

Update `NAVIGATION_ITEMS` in `utility/RolePermissions.ts`:

```typescript
export const NAVIGATION_ITEMS: NavigationItem[] = [
  {
    key: 'newfeature',
    label: 'New Feature',
    href: '/new-feature',
    icon: 'custom-icon',
    subItems: [...] // Optional sub-navigation
  },
  // ... other items
];
```

## Migration from Old System

To migrate from the old conditional sidebar:

1. Replace `<SideBar>` with `<RoleBasedSidebar>` in your layout
2. Update any hardcoded role checks to use the new permission system
3. Test all role combinations to ensure proper access control
4. Update any custom navigation components to use the new utilities

## Troubleshooting

### Common Issues

1. **Sidebar not showing items**: Check that roles are properly set in the session
2. **Permission denied errors**: Verify the user has the required role/permission
3. **TypeScript errors**: Ensure you're using the correct role/permission strings

### Debugging

Use the `useRolePermissions` hook to inspect user permissions:

```tsx
function DebugPermissions() {
  const { userRoles, getUserPermissions } = useRolePermissions();

  console.log("User Roles:", userRoles);
  console.log("User Permissions:", getUserPermissions());

  return null;
}
```
