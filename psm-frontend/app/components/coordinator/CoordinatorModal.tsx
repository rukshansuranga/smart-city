"use client";
import React, { useState, useEffect } from "react";
import {
  Modal,
  Label,
  TextInput,
  Select,
  Button,
  Spinner,
} from "flowbite-react";
import { Coordinator, ProjectCoordinatorType, User } from "@/types";
import {
  createCoordinator,
  updateCoordinator,
} from "../../api/client/cordinatorActions";
import { getUsersByUserType } from "../../api/client/userActions";
import toast from "react-hot-toast";
import { AuthType } from "@/enums";

interface CoordinatorModalProps {
  isOpen: boolean;
  onClose: (shouldRefresh?: boolean) => void;
  coordinator?: Coordinator | null;
  projectId: string | number;
}

export default function CoordinatorModal({
  isOpen,
  onClose,
  coordinator,
  projectId,
}: CoordinatorModalProps) {
  const [formData, setFormData] = useState<Partial<Coordinator>>({
    projectId: Number(projectId),
    coordinatorId: "",
    coordinatorType: ProjectCoordinatorType.Coordinator,
    assignDate: new Date().toISOString().split("T")[0],
    note: "",
  });

  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingUsers, setIsLoadingUsers] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});

  const isEditing = !!coordinator;

  // Load users for dropdown
  const loadUsers = async () => {
    setIsLoadingUsers(true);
    try {
      const response = await getUsersByUserType([
        AuthType.Staff,
        AuthType.Contractor,
      ]);

      if (response.isSuccess) {
        setUsers(response.data || []);
      } else {
        toast.error("Failed to load users");
        setUsers([]);
      }
    } catch (error) {
      console.error("Error loading users:", error);
      toast.error("Failed to load users");
      setUsers([]);
    } finally {
      setIsLoadingUsers(false);
    }
  };

  // Initialize form data when modal opens
  useEffect(() => {
    if (isOpen) {
      if (coordinator) {
        // Editing existing coordinator
        setFormData({
          projectCoordinatorId: coordinator.projectCoordinatorId,
          projectId: coordinator.projectId,
          coordinatorId: coordinator.coordinatorId,
          coordinatorType: coordinator.coordinatorType,
          assignDate: new Date(coordinator.assignDate)
            .toISOString()
            .split("T")[0],
          note: coordinator.note || "",
        });
      } else {
        // Adding new coordinator
        setFormData({
          projectId: Number(projectId),
          coordinatorId: "",
          coordinatorType: ProjectCoordinatorType.Coordinator,
          assignDate: new Date().toISOString().split("T")[0],
          note: "",
        });
      }

      setErrors({});
      loadUsers();
    }
  }, [isOpen, coordinator, projectId]);

  // Handle input changes
  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value, type } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]:
        type === "checkbox"
          ? (e.target as HTMLInputElement).checked
          : name === "coordinatorType"
          ? (parseInt(value) as ProjectCoordinatorType)
          : value,
    }));

    // Clear error for this field
    if (errors[name]) {
      setErrors((prev) => ({
        ...prev,
        [name]: "",
      }));
    }
  };

  // Validate form
  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};

    if (!formData.coordinatorId) {
      newErrors.coordinatorId = "Please select a user";
    }

    if (!formData.assignDate) {
      newErrors.assignDate = "Please select an assign date";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // Handle form submission
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    setIsLoading(true);

    try {
      let response;

      if (isEditing && coordinator?.projectCoordinatorId) {
        response = await updateCoordinator(
          coordinator.projectCoordinatorId,
          formData
        );
      } else {
        response = await createCoordinator(formData);
      }

      if (response.isSuccess) {
        toast.success(
          isEditing
            ? "Coordinator updated successfully"
            : "Coordinator added successfully"
        );
        onClose(true);
      } else {
        toast.error(response.message || "Operation failed");
      }
    } catch (error) {
      console.error("Error saving coordinator:", error);
      toast.error("Failed to save coordinator");
    } finally {
      setIsLoading(false);
    }
  };

  // Handle modal close
  const handleClose = () => {
    if (!isLoading) {
      onClose(false);
    }
  };

  return (
    <Modal show={isOpen} onClose={handleClose} size="md">
      <div className="p-6">
        <div className="flex items-center justify-between pb-4 mb-4 border-b border-gray-200 dark:border-gray-600">
          <h3 className="text-xl font-semibold text-gray-900 dark:text-white">
            {isEditing ? "Edit Coordinator" : "Add Coordinator"}
          </h3>
          <button
            onClick={handleClose}
            disabled={isLoading}
            className="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5 ml-auto inline-flex items-center dark:hover:bg-gray-600 dark:hover:text-white"
          >
            <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
              <path
                fillRule="evenodd"
                d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                clipRule="evenodd"
              />
            </svg>
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          {/* User Selection */}
          <div>
            <Label htmlFor="coordinatorId">Select User *</Label>
            <Select
              id="coordinatorId"
              name="coordinatorId"
              value={formData.coordinatorId}
              onChange={handleInputChange}
              required
              disabled={isLoadingUsers}
            >
              <option value="">
                {isLoadingUsers ? "Loading users..." : "Select a user"}
              </option>
              {users.map((user) => (
                <option key={user.userId} value={user.userId}>
                  {user.firstName} {user.mobile ? `(${user.mobile})` : ""}
                </option>
              ))}
            </Select>
            {errors.coordinatorId && (
              <p className="text-red-500 text-sm mt-1">
                {errors.coordinatorId}
              </p>
            )}
          </div>

          {/* Coordinator Type */}
          <div>
            <Label htmlFor="coordinatorType">Coordinator Type *</Label>
            <Select
              id="coordinatorType"
              name="coordinatorType"
              value={formData.coordinatorType}
              onChange={handleInputChange}
              required
            >
              <option value={ProjectCoordinatorType.Coordinator}>
                Coordinator
              </option>
              <option value={ProjectCoordinatorType.Supporter}>
                Supporter
              </option>
            </Select>
          </div>

          {/* Assign Date */}
          <div>
            <Label htmlFor="assignDate">Assign Date *</Label>
            <TextInput
              id="assignDate"
              name="assignDate"
              type="date"
              value={
                typeof formData.assignDate === "string"
                  ? formData.assignDate
                  : formData.assignDate?.toISOString().split("T")[0] || ""
              }
              onChange={handleInputChange}
              required
            />
            {errors.assignDate && (
              <p className="text-red-500 text-sm mt-1">{errors.assignDate}</p>
            )}
          </div>

          {/* Note */}
          <div>
            <Label htmlFor="note">Note</Label>
            <TextInput
              id="note"
              name="note"
              type="text"
              value={formData.note || ""}
              onChange={handleInputChange}
              placeholder="Optional note"
            />
          </div>

          {/* Form Actions */}
          <div className="flex justify-end space-x-2 pt-4">
            <Button
              color="gray"
              onClick={handleClose}
              disabled={isLoading}
              type="button"
            >
              Cancel
            </Button>
            <Button type="submit" disabled={isLoading}>
              {isLoading ? (
                <>
                  <Spinner aria-label="Saving" size="sm" className="mr-2" />
                  {isEditing ? "Updating..." : "Adding..."}
                </>
              ) : isEditing ? (
                "Update Coordinator"
              ) : (
                "Add Coordinator"
              )}
            </Button>
          </div>
        </form>
      </div>
    </Modal>
  );
}
