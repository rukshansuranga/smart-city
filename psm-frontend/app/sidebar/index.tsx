"use client";

import {
  Sidebar,
  SidebarCollapse,
  SidebarItem,
  SidebarItemGroup,
  SidebarItems,
} from "flowbite-react";

import Image from "next/image";
import { Session } from "next-auth";

// Use a React component for the PNG icon
const TicketIcon = () => (
  <Image src="/images/ticket.png" alt="Ticket" width={30} height={30} />
);

const TicketListIcon = () => (
  <Image src="/images/ticket-list.png" alt="Ticket" width={30} height={30} />
);

const TicketAddIcon = () => (
  <Image src="/images/ticket-add.png" alt="Ticket" width={30} height={30} />
);

const TicketAssignedIcon = () => (
  <Image
    src="/images/ticket-assigned.png"
    alt="Ticket"
    width={30}
    height={30}
  />
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
    <Sidebar
      aria-label="Default sidebar"
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
          <SidebarItem
            href="dashboard"
            icon={DashboardIcon}
            className="text-[#264653] hover:bg-[#E9C46A]/20 hover:text-[#264653] transition-all duration-200 rounded-lg mb-1"
          >
            <span className="text-[#264653] font-medium">Dashboard</span>
          </SidebarItem>
          <SidebarItem
            href={`board/${session?.user?.id}`}
            icon={BoardIcon}
            className="text-[#264653] hover:bg-[#E9C46A]/20 hover:text-[#264653] transition-all duration-200 rounded-lg mb-1"
          >
            <span className="text-[#264653] font-medium">Board</span>
          </SidebarItem>

          <SidebarCollapse
            icon={ComplainsIcon}
            label="Complains"
            className="hover:bg-[#E9C46A]/20 transition-all duration-200 rounded-lg mb-1 text-[#264653]"
          >
            <SidebarItem
              icon={StreetlampIcon}
              href="/complain/LightPostComplain"
              className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
            >
              <span className="text-[#264653] font-medium">Light Post</span>
            </SidebarItem>
            <SidebarItem
              href="/complain/GeneralComplain"
              icon={GeneralComplainIcon}
              className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
            >
              <span className="text-[#264653] font-medium">General</span>
            </SidebarItem>
            <SidebarItem
              href="#"
              icon={ProjectComplainIcon}
              className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
            >
              <span className="text-[#264653] font-medium">Project</span>
            </SidebarItem>
            <SidebarItem
              href="/complain/GarbageComplain"
              icon={GarbageComplainIcon}
              className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
            >
              <span className="text-[#264653] font-medium">Garbage</span>
            </SidebarItem>
          </SidebarCollapse>

          <SidebarCollapse
            icon={TicketIcon}
            label="Ticket"
            className="hover:bg-[#E9C46A]/20 transition-all duration-200 rounded-lg mb-1 text-[#264653]"
          >
            <SidebarItem
              href="/ticket"
              icon={TicketListIcon}
              className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
            >
              <span className="text-[#264653] font-medium">Tickets</span>
            </SidebarItem>
            <SidebarItem
              href="/ticket/new"
              icon={TicketAddIcon}
              className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
            >
              <span className="text-[#264653] font-medium">New Ticket</span>
            </SidebarItem>
            <SidebarItem
              href="/ticket/resolved"
              icon={TicketAssignedIcon}
              className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
            >
              <span className="text-[#264653] font-medium">Resolved</span>
            </SidebarItem>
          </SidebarCollapse>
          {session?.roles?.includes("admin") && (
            <SidebarCollapse
              icon={ProjectsIcon}
              label="Projects"
              className="hover:bg-[#E9C46A]/20 transition-all duration-200 rounded-lg mb-1 text-[#264653]"
            >
              <SidebarItem
                href="/project/new"
                icon={AddProjectIcon}
                className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
              >
                <span className="text-[#264653] font-medium">New Project</span>
              </SidebarItem>
              <SidebarItem
                href="/project"
                icon={ListProjectIcon}
                className="text-[#264653] hover:bg-[#F4A261]/20 hover:text-[#264653] transition-all duration-200 rounded-lg ml-4"
              >
                <span className="text-[#264653] font-medium">List</span>
              </SidebarItem>
            </SidebarCollapse>
          )}

          {session?.roles?.includes("admin") && (
            <SidebarItem
              href="/user"
              icon={UserIcon}
              className="text-[#264653] hover:bg-[#E9C46A]/20 hover:text-[#264653] transition-all duration-200 rounded-lg mb-1"
            >
              <span className="text-[#264653] font-medium">User</span>
            </SidebarItem>
          )}
        </SidebarItemGroup>
      </SidebarItems>
    </Sidebar>
  );
}
