"use client";

import { AiTwotoneBank } from "react-icons/ai";

export default function Logo() {
  return (
    <div className="cursor-pointer flex items-center gap-2 text-3xl font-semibold ">
      <AiTwotoneBank className="text-green-500" size={34} />
      <div className="text-green-500 ">PSM</div>
    </div>
  );
}
