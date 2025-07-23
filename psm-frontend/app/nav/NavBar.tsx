"use client";

import { Navbar, NavbarBrand } from "flowbite-react";
import Logo from "./Logo";
import Link from "next/link";
import { Session } from "next-auth";
import SignOut from "../components/auth/SignOut";
import SignIn from "../components/auth/SignIn";

export interface HeaderProps {
  session?: Session | null;
}

export default function NavBar({ session }: HeaderProps) {
  return (
    <Navbar
      rounded
      className="sticky top-0 z-50 flex justify-between 
            bg-white p-5 items-center text-gray-800 shadow-md"
    >
      <NavbarBrand as={Link} href="https://flowbite-react.com">
        <Logo />
      </NavbarBrand>
      {/* <NavbarToggle />
      <NavbarCollapse>
        <NavbarLink href="#" active>
          Home
        </NavbarLink>
        <NavbarLink as={Link} href="#">
          About
        </NavbarLink>
        <NavbarLink href="#">Services</NavbarLink>
        <NavbarLink href="#">Pricing</NavbarLink>
        <NavbarLink href="#">Contact</NavbarLink>
      </NavbarCollapse> */}
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
