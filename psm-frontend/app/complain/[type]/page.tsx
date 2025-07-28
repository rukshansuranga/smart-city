"use client";

import WorkpackageList from "@/app/components/workpackage/WorkpackageList";
import { useParams } from "next/navigation";

export default function ComplainPage() {
  const { type }: { type: string } = useParams();

  return (
    <div>
      <WorkpackageList type={type} pageSize={8} />
    </div>
  );
}
