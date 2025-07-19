"use client";
import { useParams } from "next/navigation";
import TenderForm from "../../TenderForm";

export default function AddTenderPage() {
  const { projectId } = useParams<{ projectId: string }>();

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Add New Tender</h1>
      <TenderForm selectedProjectId={projectId} />
    </div>
  );
}
