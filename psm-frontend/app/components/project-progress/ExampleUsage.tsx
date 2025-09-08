// Example usage of ProjectProgressApprovalModal

import React, { useState } from "react";
import { Button } from "flowbite-react";
import ProjectProgressApprovalModal from "@/app/components/project-progress/ProjectProgressApprovalModal";
import { ProjectProgressApprovedStatus } from "@/enums";

export default function ExampleUsage() {
  const [showModal, setShowModal] = useState(false);

  // Example project progress data
  const exampleProjectProgressId = "123e4567-e89b-12d3-a456-426614174000";
  const currentStatus = ProjectProgressApprovedStatus.Pending;
  const currentNote = "Previous approval note";

  const handleOpenModal = () => {
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };

  const handleSuccess = () => {
    console.log("Project progress updated successfully!");
    // You can add any additional logic here, such as:
    // - Refreshing data
    // - Showing a success message
    // - Updating the UI
  };

  return (
    <div className="p-4">
      <Button onClick={handleOpenModal}>
        Update Project Progress Approval
      </Button>

      <ProjectProgressApprovalModal
        isOpen={showModal}
        onClose={handleCloseModal}
        projectProgressId={exampleProjectProgressId}
        currentStatus={currentStatus}
        currentNote={currentNote}
        onSuccess={handleSuccess}
      />
    </div>
  );
}
