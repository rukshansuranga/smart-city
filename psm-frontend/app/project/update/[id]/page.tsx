"use client";
import { useParams } from "next/navigation";
import ProjectForm from "../../ProjectForm";

export default function UpdateProject() {
  const { id } = useParams<{ id: string }>();
  return (
    <div className="flex flex-col gap-4">
      <ProjectForm projectId={id} />
    </div>
  );
}
