"use client";

import {
  Sidebar,
  SidebarItem,
  SidebarItemGroup,
  SidebarItems,
} from "flowbite-react";
import { HiChartPie, HiInbox, HiUser, HiViewBoards } from "react-icons/hi";

export function SideBar() {
  return (
    <Sidebar aria-label="Default sidebar" className="w-52">
      <SidebarItems>
        <SidebarItemGroup>
          <SidebarItem href="/workpackage" icon={HiChartPie}>
            Complains
          </SidebarItem>
          <SidebarItem
            href="dashboard"
            icon={HiViewBoards}
            label="Pro"
            labelColor="dark"
          >
            Dashboard
          </SidebarItem>
          <SidebarItem href="ticket" icon={HiInbox}>
            New Ticket
          </SidebarItem>
          <SidebarItem href="/ticket" icon={HiUser} label="3">
            Tickets
          </SidebarItem>
          <SidebarItem href="#" icon={HiUser}>
            Admin
          </SidebarItem>
        </SidebarItemGroup>
      </SidebarItems>
    </Sidebar>
  );
}
