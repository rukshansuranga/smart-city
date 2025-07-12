"use client";

import Logo from "./Logo";

export default function NavBar() {
  return (
    <header
      className="sticky top-0 z-50 flex justify-between 
            bg-white p-5 items-center text-gray-800 shadow-md"
    >
      <Logo />
    </header>
  );
}
