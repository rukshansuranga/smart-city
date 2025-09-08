"use client";
import React, { useState, useEffect, useCallback } from "react";
import {
  Button,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeadCell,
  TableRow,
  Spinner,
  Badge,
} from "flowbite-react";
import { HiPencilAlt, HiTrash, HiPlus } from "react-icons/hi";
import { Coordinator, ProjectCoordinatorType } from "@/types";
import {
  getCoordinatorsByProjectId,
  deleteCoordinator,
} from "../../api/client/cordinatorActions";
import toast from "react-hot-toast";
import CoordinatorModal from "./CoordinatorModal";

interface CoordinatorListProps {
  projectId: string | number;
  initialData?: Coordinator[];
  onDataChange?: () => void;
}

export default function CoordinatorList({
  projectId,
  initialData = [],
  onDataChange,
}: CoordinatorListProps) {
  const [coordinators, setCoordinators] = useState<Coordinator[]>(initialData);
  const [isLoading, setIsLoading] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCoordinator, setSelectedCoordinator] =
    useState<Coordinator | null>(null);

  // Load coordinators data
  const loadCoordinators = useCallback(async () => {
    if (!projectId) return;

    setIsLoading(true);
    try {
      const response = await getCoordinatorsByProjectId(projectId);

      if (response.isSuccess) {
        setCoordinators(response.data);
      } else {
        toast.error(response.message || "Failed to load coordinators");
        setCoordinators([]);
      }
    } catch (error) {
      console.error("Error loading coordinators:", error);
      toast.error("Failed to load coordinators");
      setCoordinators([]);
    } finally {
      setIsLoading(false);
    }
  }, [projectId]);

  // Load data on mount if no initial data provided
  useEffect(() => {
    if (initialData.length === 0) {
      loadCoordinators();
    }
  }, [projectId, initialData.length, loadCoordinators]);

  // Handle coordinator deletion
  const handleDelete = async (projectCoordinatorId: number) => {
    if (!window.confirm("Are you sure you want to remove this coordinator?")) {
      return;
    }

    try {
      const response = await deleteCoordinator(projectCoordinatorId);

      if (response.isSuccess) {
        toast.success("Coordinator removed successfully");
        setCoordinators((prev) =>
          prev.filter((c) => c.projectCoordinatorId !== projectCoordinatorId)
        );
        onDataChange?.();
      } else {
        toast.error(response.message || "Failed to remove coordinator");
      }
    } catch (error) {
      console.error("Error deleting coordinator:", error);
      toast.error("Failed to remove coordinator");
    }
  };

  // Handle add coordinator
  const handleAddCoordinator = () => {
    setSelectedCoordinator(null);
    setIsModalOpen(true);
  };

  // Handle edit coordinator
  const handleEditCoordinator = (coordinator: Coordinator) => {
    setSelectedCoordinator(coordinator);
    setIsModalOpen(true);
  };

  // Handle modal close and refresh
  const handleModalClose = (shouldRefresh?: boolean) => {
    setIsModalOpen(false);
    setSelectedCoordinator(null);

    if (shouldRefresh) {
      loadCoordinators();
      onDataChange?.();
    }
  };

  // Get coordinator type label
  const getCoordinatorTypeLabel = (type: ProjectCoordinatorType) => {
    switch (type) {
      case ProjectCoordinatorType.Coordinator:
        return "Coordinator";
      case ProjectCoordinatorType.Supporter:
        return "Supporter";
      default:
        return "Unknown";
    }
  };

  // Get coordinator type color
  const getCoordinatorTypeColor = (type: ProjectCoordinatorType) => {
    switch (type) {
      case ProjectCoordinatorType.Coordinator:
        return "blue";
      case ProjectCoordinatorType.Supporter:
        return "green";
      default:
        return "gray";
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-8">
        <Spinner aria-label="Loading coordinators" size="lg" />
      </div>
    );
  }

  return (
    <div className="space-y-4">
      {/* Header with Add Button */}
      <div className="flex justify-between items-center">
        <div>
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
            Project Coordinators
          </h3>
          <p className="text-sm text-gray-600 dark:text-gray-400">
            Manage project coordinators and supporters
          </p>
        </div>
        <Button
          onClick={handleAddCoordinator}
          size="sm"
          className="flex items-center"
        >
          <HiPlus className="w-4 h-4 mr-2" />
          Add Coordinator
        </Button>
      </div>

      {/* Coordinators Table */}
      {coordinators.length === 0 ? (
        <div className="text-center py-8">
          <div className="text-gray-500 dark:text-gray-400 mb-4">
            No coordinators assigned to this project
          </div>
          <Button onClick={handleAddCoordinator} size="sm" outline>
            <HiPlus className="w-4 h-4 mr-2" />
            Add First Coordinator
          </Button>
        </div>
      ) : (
        <div className="overflow-x-auto">
          <Table hoverable>
            <TableHead>
              <TableHeadCell>Name</TableHeadCell>
              <TableHeadCell>Contact</TableHeadCell>
              <TableHeadCell>Type</TableHeadCell>
              <TableHeadCell>Assign Date</TableHeadCell>
              <TableHeadCell>Note</TableHeadCell>
              <TableHeadCell>Actions</TableHeadCell>
            </TableHead>
            <TableBody className="divide-y">
              {coordinators.map((coordinator) => (
                <TableRow
                  key={coordinator.projectCoordinatorId}
                  className="bg-white dark:border-gray-700 dark:bg-gray-800"
                >
                  <TableCell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                    {coordinator.coordinator?.firstName || "N/A"}
                  </TableCell>
                  <TableCell>
                    {coordinator.coordinator?.mobile || "N/A"}
                  </TableCell>
                  <TableCell>
                    <Badge
                      color={getCoordinatorTypeColor(
                        coordinator.coordinatorType
                      )}
                    >
                      {getCoordinatorTypeLabel(coordinator.coordinatorType)}
                    </Badge>
                  </TableCell>
                  <TableCell>
                    {new Date(coordinator.assignDate).toLocaleDateString()}
                  </TableCell>
                  <TableCell>{coordinator.note || "No note"}</TableCell>
                  <TableCell>
                    <div className="flex items-center space-x-2">
                      <Button
                        size="xs"
                        color="blue"
                        onClick={() => handleEditCoordinator(coordinator)}
                      >
                        <HiPencilAlt className="w-3 h-3" />
                      </Button>
                      <Button
                        size="xs"
                        color="red"
                        onClick={() =>
                          handleDelete(coordinator.projectCoordinatorId!)
                        }
                      >
                        <HiTrash className="w-3 h-3" />
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </div>
      )}

      {/* Coordinator Modal */}
      <CoordinatorModal
        isOpen={isModalOpen}
        onClose={handleModalClose}
        coordinator={selectedCoordinator}
        projectId={projectId}
      />
    </div>
  );
}
