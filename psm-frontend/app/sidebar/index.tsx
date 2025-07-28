"use client";

import {
  Sidebar,
  SidebarCollapse,
  SidebarItem,
  SidebarItemGroup,
  SidebarItems,
} from "flowbite-react";

import { LuTicketPlus } from "react-icons/lu";
import { RiContractLine } from "react-icons/ri";
import Image from "next/image";

// Use a React component for the PNG icon
const TicketIcon = () => (
  <Image src="/images/ticket.png" alt="Ticket" width={30} height={30} />
);

const DashboardIcon = () => (
  <Image src="/images/dashboard.png" alt="Dashboard" width={30} height={30} />
);

const ComplainsIcon = () => (
  <Image src="/images/complains.png" alt="Complains" width={30} height={30} />
);

const StreetlampIcon = () => (
  <Image src="/images/streetlamp.png" alt="Streetlamp" width={30} height={30} />
);

const GeneralComplainIcon = () => (
  <Image
    src="/images/general_complain.png"
    alt="General Complain"
    width={30}
    height={30}
  />
);

const ProjectComplainIcon = () => (
  <Image
    src="/images/project_complain.png"
    alt="Project Complain"
    width={30}
    height={30}
  />
);

const GarbageComplainIcon = () => (
  <Image
    src="/images/garbage_complain.png"
    alt="Garbage Complain"
    width={30}
    height={30}
  />
);

const ProjectsIcon = () => (
  <Image src="/images/projects.png" alt="Projects" width={30} height={30} />
);

export function SideBar() {
  return (
    <Sidebar aria-label="Default sidebar" className="w-52">
      <SidebarItems>
        <SidebarItemGroup>
          <SidebarItem href="dashboard" icon={DashboardIcon} labelColor="dark">
            Dashboard
          </SidebarItem>
          <SidebarCollapse icon={ComplainsIcon} label="Complains">
            <SidebarItem
              icon={StreetlampIcon}
              href="/complain/LightPostComplint"
            >
              Light Post
            </SidebarItem>
            <SidebarItem
              href="/complain/GeneralComplain"
              icon={GeneralComplainIcon}
            >
              General
            </SidebarItem>
            <SidebarItem href="#" icon={ProjectComplainIcon}>
              Project
            </SidebarItem>
            <SidebarItem
              href="/complain/GarbageComplain"
              icon={GarbageComplainIcon}
            >
              Garbage
            </SidebarItem>
          </SidebarCollapse>
          {/* <SidebarItem href="/workpackage" icon={MdOutlineWorkHistory}>
            Workpackages
          </SidebarItem> */}
          <SidebarCollapse icon={TicketIcon} label="Ticket">
            <SidebarItem href="/ticket" icon={LuTicketPlus}>
              Tickets
            </SidebarItem>
            <SidebarItem href="/ticket/new" icon={LuTicketPlus}>
              New Ticket
            </SidebarItem>
          </SidebarCollapse>
          <SidebarItem href="/project" icon={ProjectsIcon}>
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
