"use client";

import {
  Sidebar,
  SidebarCollapse,
  SidebarItem,
  SidebarItemGroup,
  SidebarItems,
} from "flowbite-react";
import {
  HiChartPie,
  HiInbox,
  HiShoppingBag,
  HiUser,
  HiViewBoards,
} from "react-icons/hi";

import { MdOutlineWorkHistory } from "react-icons/md";
import { LuTicketPlus } from "react-icons/lu";
import { GoProject } from "react-icons/go";
import { RiContractLine } from "react-icons/ri";

export function SideBar() {
  return (
    <Sidebar aria-label="Default sidebar" className="w-52">
      <SidebarItems>
        <SidebarItemGroup>
          <SidebarItem
            href="dashboard"
            icon={HiViewBoards}
            label="Pro"
            labelColor="dark"
          >
            Dashboard
          </SidebarItem>
          <SidebarCollapse icon={HiShoppingBag} label="Complains">
            <SidebarItem href="#">Light Post</SidebarItem>
            <SidebarItem href="#">General</SidebarItem>
            <SidebarItem href="#">Project</SidebarItem>
            <SidebarItem href="#">Garbage</SidebarItem>
          </SidebarCollapse>
          <SidebarItem href="/workpackage" icon={MdOutlineWorkHistory}>
            Workpackages
          </SidebarItem>

          <SidebarItem href="/ticket" icon={LuTicketPlus} label="3">
            Tickets
          </SidebarItem>
          <SidebarItem href="/project" icon={GoProject}>
            Projects
          </SidebarItem>
          <SidebarItem href="/tender" icon={RiContractLine}>
            Tenders
          </SidebarItem>
        </SidebarItemGroup>
      </SidebarItems>
    </Sidebar>
  );
}
