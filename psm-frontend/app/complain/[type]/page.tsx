"use client";

import LightPost from "@/app/components/complain/lightpost";
import WorkpackageList from "@/app/components/workpackage/WorkpackageList";
import { useParams } from "next/navigation";

export default function ComplainPage() {
  const { type }: { type: string } = useParams();

  console.log("Complain type:", type);

  if (type === "LightPostComplain") {
    return <LightPost />;
  }

  return (
    <div>
      <WorkpackageList type={type} pageSize={8} />
    </div>
  );
}
