"use client";

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

const UserIcon = () => (
  <Image src="/images/user.png" alt="User" width={30} height={30} />
);

const TenderIcon = () => (
  <Image src="/images/bid.png" alt="Tender" width={30} height={30} />
);

const ProjectsIcon = () => (
  <Image src="/images/projects.png" alt="Projects" width={30} height={30} />
);

const AddProjectIcon = () => (
  <Image src="/images/project-add.png" alt="Projects" width={30} height={30} />
);

const ListProjectIcon = () => (
  <Image src="/images/project-list.png" alt="Projects" width={30} height={30} />
);

const BoardIcon = () => (
  <Image src="/images/board.png" alt="Board" width={30} height={30} />
);

export interface HeaderProps {
  session?: Session | null;
}

export function SideBar({ session }: HeaderProps) {
  return (
    <Sidebar aria-label="Default sidebar" className="w-52">
      <SidebarItems>
        <SidebarItemGroup>
          <SidebarItem href="dashboard" icon={DashboardIcon} labelColor="dark">
            Dashboard
          </SidebarItem>
          <SidebarItem
            href={`board/${session?.user?.id}`}
            icon={BoardIcon}
            labelColor="dark"
          >
            Board
          </SidebarItem>

          <SidebarCollapse icon={ComplainsIcon} label="Complains">
            <SidebarItem
              icon={StreetlampIcon}
              href="/complain/LightPostComplain"
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

          <SidebarCollapse icon={TicketIcon} label="Ticket">
            <SidebarItem href="/ticket" icon={LuTicketPlus}>
              Tickets
            </SidebarItem>
            <SidebarItem href="/ticket/new" icon={LuTicketPlus}>
              New Ticket
            </SidebarItem>
            <SidebarItem href="/ticket/resolved" icon={LuTicketPlus}>
              Resolved
            </SidebarItem>
          </SidebarCollapse>
          {session?.roles?.includes("admin") && (
            <SidebarCollapse icon={ProjectsIcon} label="Projects">
              <SidebarItem href="/project/new" icon={AddProjectIcon}>
                New Project
              </SidebarItem>
              <SidebarItem href="/project" icon={ListProjectIcon}>
                List
              </SidebarItem>
            </SidebarCollapse>
          )}

          {session?.roles?.includes("admin") && (
            <SidebarItem href="/user" icon={UserIcon}>
              User
            </SidebarItem>
          )}
        </SidebarItemGroup>
      </SidebarItems>
    </Sidebar>
  );
}
