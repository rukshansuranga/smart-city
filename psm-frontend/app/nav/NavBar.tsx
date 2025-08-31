"use client";

import { Navbar, NavbarBrand, Select } from "flowbite-react";
import Logo from "./Logo";
import Link from "next/link";
import { Session } from "next-auth";
import SignOut from "../components/auth/SignOut";
import SignIn from "../components/auth/SignIn";
import { useCouncilStore } from "@/store";
import { useEffect, useMemo } from "react";

export interface HeaderProps {
  session?: Session | null;
}

export default function NavBar({ session }: HeaderProps) {
  const { council, setCouncil, setLocation } = useCouncilStore();

  // Council locations mapping (you can adjust these coordinates as needed)
  const councilLocations = useMemo(
    () => ({
      Mahara: { lat: 6.9913, lng: 79.9395 },
      Kandy: { lat: 7.2906, lng: 80.6337 },
      Galle: { lat: 6.0535, lng: 80.221 },
      // Add more councils and their coordinates as needed
    }),
    []
  );

  // Set council from session when it loads
  useEffect(() => {
    if (session?.council && council !== session.council) {
      console.log("Setting council from session:", session.council);
      setCouncil(session.council);
    }
  }, [session, council, setCouncil]);

  // Update location when council changes
  useEffect(() => {
    if (council && councilLocations[council]) {
      setLocation(councilLocations[council]);
    }
  }, [council, setLocation, councilLocations]);

  console.log("Current council in NavBar:", council);

  return (
    <Navbar
      rounded
      className="sticky top-0 z-50 flex justify-between 
            bg-white p-5 items-center text-gray-800 shadow-md"
    >
      <NavbarBrand as={Link} href="https://flowbite-react.com">
        <Logo />
      </NavbarBrand>

      {session?.roles && (
        <div className="flex space-x-4">
          {session.roles
            .filter(
              (role) =>
                role !== "offline_access" &&
                role !== "default-roles-smartcity" &&
                role !== "uma_authorization"
            )
            .map((role) => (
              <span key={role} className="text-sm text-gray-600">
                {role}
              </span>
            ))}
        </div>
      )}

      {session?.user ? (
        <div className="flex flex-row justify-between items-center">
          <div className="mr-4">{session.user.name}</div>
          <SignOut />
        </div>
      ) : (
        <SignIn />
      )}
    </Navbar>
  );
}
