"use client";
import { useParams } from "next/navigation";
import TenderForm from "../../../TenderForm";

export default function UpdateTender() {
  const { projectId, id } = useParams<{ projectId: string; id: string }>();

  console.log("Project ID:", projectId, "Tender ID:", id);

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Update Tender</h1>
      {/* Form for updating tender will go here */}
      <TenderForm selectedProjectId={projectId} tenderId={id} />
    </div>
  );
}
