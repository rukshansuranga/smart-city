"use client";

import React from "react";
import {
  Sidebar,
  SidebarCollapse,
  SidebarItem,
  SidebarItemGroup,
  SidebarItems,
} from "flowbite-react";
import { LuTicketPlus } from "react-icons/lu";
import Image from "next/image";
import { Session } from "next-auth";
import {
  getAuthorizedNavigation,
  NavigationItem,
} from "../../utility/RolePermissions";

// Icon components mapping
const iconComponents = {
  dashboard: () => (
    <Image src="/images/dashboard.png" alt="Dashboard" width={30} height={30} />
  ),
  board: () => (
    <Image src="/images/board.png" alt="Board" width={30} height={30} />
  ),
  complains: () => (
    <Image src="/images/complains.png" alt="Complains" width={30} height={30} />
  ),
  streetlamp: () => (
    <Image
      src="/images/streetlamp.png"
      alt="Streetlamp"
      width={30}
      height={30}
    />
  ),
  general_complain: () => (
    <Image
      src="/images/general_complain.png"
      alt="General Complain"
      width={30}
      height={30}
    />
  ),
  project_complain: () => (
    <Image
      src="/images/project_complain.png"
      alt="Project Complain"
      width={30}
      height={30}
    />
  ),
  garbage_complain: () => (
    <Image
      src="/images/garbage_complain.png"
      alt="Garbage Complain"
      width={30}
      height={30}
    />
  ),
  ticket: () => (
    <Image src="/images/ticket.png" alt="Ticket" width={30} height={30} />
  ),
  projects: () => (
    <Image src="/images/projects.png" alt="Projects" width={30} height={30} />
  ),
  "project-add": () => (
    <Image
      src="/images/project-add.png"
      alt="Add Project"
      width={30}
      height={30}
    />
  ),
  "project-list": () => (
    <Image
      src="/images/project-list.png"
      alt="List Projects"
      width={30}
      height={30}
    />
  ),
  user: () => (
    <Image src="/images/user.png" alt="User" width={30} height={30} />
  ),
  bid: () => (
    <Image src="/images/bid.png" alt="Tender" width={30} height={30} />
  ),
};

// Fallback for missing icons
const DefaultIcon = () => <LuTicketPlus />;

interface RoleBasedSidebarProps {
  session?: Session | null;
}

export function RoleBasedSidebar({ session }: RoleBasedSidebarProps) {
  // Get user roles, fallback to empty array if no session
  const userRoles = session?.roles || [];
  const userId = session?.user?.id;

  // Get authorized navigation items based on user roles
  const authorizedNavItems = getAuthorizedNavigation(userRoles, userId);

  // Helper function to get icon component
  const getIconComponent = (iconName: string) => {
    const IconComponent =
      iconComponents[iconName as keyof typeof iconComponents];
    return IconComponent || DefaultIcon;
  };

  // Helper function to render navigation item
  const renderNavigationItem = (item: NavigationItem) => {
    const IconComponent = getIconComponent(item.icon);

    // If item has sub-items, render as collapsible
    if (item.subItems && item.subItems.length > 0) {
      return (
        <SidebarCollapse
          key={item.key}
          icon={IconComponent}
          label={item.label}
          className="hover:bg-[#E9C46A]/20 transition-all duration-200 rounded-lg mb-1 text-[#264653]"
        >
          {item.subItems.map(
            (
              subItem: { label: string; href: string; icon: string },
              index: number
            ) => {
              const SubIconComponent = getIconComponent(subItem.icon);
              return (
                <SidebarItem
                  key={`${item.key}-${index}`}
                  href={subItem.href}
                  icon={SubIconComponent}
                  className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
                >
                  <span className="text-[#264653] font-medium">
                    {subItem.label}
                  </span>
                </SidebarItem>
              );
            }
          )}
        </SidebarCollapse>
      );
    }

    // Regular sidebar item
    return (
      <SidebarItem
        key={item.key}
        href={item.href}
        icon={IconComponent}
        className="text-[#264653] hover:bg-[#E9C46A]/20 hover:text-[#264653] transition-all duration-200 rounded-lg mb-1"
      >
        <span className="text-[#264653] font-medium">{item.label}</span>
      </SidebarItem>
    );
  };

  // Don't render sidebar if no authorized items
  if (authorizedNavItems.length === 0) {
    return (
      <Sidebar
        aria-label="No access sidebar"
        className="w-52 bg-white border-r-4 border-[#E76F51] shadow-lg"
        theme={{
          root: {
            base: "h-full bg-white",
            inner:
              "h-full overflow-y-auto overflow-x-hidden rounded bg-white px-3 py-4",
          },
        }}
      >
        <SidebarItems>
          <SidebarItemGroup>
            <SidebarItem
              href="#"
              icon={DefaultIcon}
              className="text-[#E76F51] hover:bg-[#E76F51]/20 transition-all duration-200 rounded-lg"
            >
              <span className="text-[#E76F51] font-medium">No Access</span>
            </SidebarItem>
          </SidebarItemGroup>
        </SidebarItems>
      </Sidebar>
    );
  }

  return (
    <Sidebar
      aria-label="Role-based sidebar"
      className="w-52 bg-white border-r-4 border-[#2A9D8F] shadow-lg"
      theme={{
        root: {
          base: "h-full bg-white",
          inner:
            "h-full overflow-y-auto overflow-x-hidden rounded bg-white px-3 py-4",
        },
        item: {
          base: "flex items-center justify-center rounded-lg p-2 text-base font-medium text-[#264653] hover:bg-[#E9C46A]/20 hover:text-[#264653] transition-all duration-200",
          active: "bg-[#2A9D8F] text-white hover:bg-[#2A9D8F] hover:text-white",
        },
        collapse: {
          button:
            "group flex w-full items-center rounded-lg p-2 text-base font-medium text-[#264653] transition duration-75 hover:bg-[#E9C46A]/20",
          icon: {
            base: "h-6 w-6 text-[#264653] transition duration-75 group-hover:text-[#264653]",
          },
          label: {
            base: "ml-3 flex-1 whitespace-nowrap text-left text-[#264653] group-hover:text-[#264653]",
          },
        },
      }}
    >
      <SidebarItems>
        <SidebarItemGroup>
          {authorizedNavItems.map(renderNavigationItem)}
        </SidebarItemGroup>
      </SidebarItems>
    </Sidebar>
  );
}
