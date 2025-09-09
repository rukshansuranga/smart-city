"use client";

import Image from "next/image";
export default function Logo() {
  return (
    <div className="cursor-pointer flex items-center gap-4 text-3xl font-semibold ">
      <Image
        src="/images/icon2.png"
        alt="Logo"
        width={50}
        height={50}
        className="object-contain"
      />
      <div className="text-[#264653]">Smart City</div>
    </div>
  );
}
