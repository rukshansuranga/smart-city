"use client";

import React, { useState, useCallback } from "react";
import { ProjectProgress } from "@/types";
import ProjectProgressList from "./ProjectProgressList";
import ProjectProgressForm from "./ProjectProgressForm";

interface ProjectProgressManagerProps {
  projectId: string;
}

export default function ProjectProgressManager({ projectId }: ProjectProgressManagerProps) {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingProgress, setEditingProgress] = useState<ProjectProgress | null>(null);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  // Handle opening modal for adding new progress
  const handleAddProgress = useCallback(() => {
    setEditingProgress(null);
    setIsModalOpen(true);
  }, []);

  // Handle opening modal for editing existing progress
  const handleEditProgress = useCallback((progress: ProjectProgress) => {
    setEditingProgress(progress);
    setIsModalOpen(true);
  }, []);

  // Handle closing modal
  const handleCloseModal = useCallback(() => {
    setIsModalOpen(false);
    setEditingProgress(null);
  }, []);

  // Handle form submission (both add and edit)
  const handleSubmitProgress = useCallback(async (progressData: Partial<ProjectProgress>) => {
    try {
      // TODO: Replace with actual API calls
      if (editingProgress) {
        // Update existing progress
        console.log('Updating progress:', { ...editingProgress, ...progressData });
        // await updateProjectProgress(editingProgress.projectProgressId, progressData);
      } else {
        // Create new progress
        console.log('Creating new progress:', { ...progressData, projectId });
        // await createProjectProgress({ ...progressData, projectId });
      }
      
      // Close modal and refresh list
      handleCloseModal();
      setRefreshTrigger(prev => prev + 1);
    } catch (error) {
      console.error('Error submitting progress:', error);
      // Handle error (show notification, etc.)
    }
  }, [editingProgress, projectId, handleCloseModal]);

  return (
    <div>
      {/* Progress List with Add and Edit functionality */}
      <ProjectProgressList
        projectId={projectId}
        onAddProgress={handleAddProgress}
        onEditProgress={handleEditProgress}
        refreshTrigger={refreshTrigger}
      />

      {/* Modal with Progress Form */}
      <ProjectProgressForm
        isOpen={isModalOpen}
        onClose={handleCloseModal}
        onSubmit={handleSubmitProgress}
        editingProgress={editingProgress}
        defaultProjectId={projectId}
      />
    </div>
  );
}
