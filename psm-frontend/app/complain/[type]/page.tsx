"use client";

import ComplainList from "@/app/components/complain/ComplainList";
import LightPostComplainList from "@/app/components/complain/lightpost";

import { useParams } from "next/navigation";

export default function ComplainPage() {
  const { type }: { type: string } = useParams();

  console.log("Complain type:", type);

  if (type === "LightPostComplain") {
    return <LightPostComplainList />;
  }

  return (
    <div>
      <ComplainList type={type} pageSize={8} />
    </div>
  );
}
