"use client";

import React, { useState, useEffect } from "react";
import { Tag, CreateTagRequest } from "@/types";
import {
  getAllTagsClient,
  createTagClient,
  updateTagClient,
  deleteTagClient,
} from "@/app/api/client/tagActions";
import {
  Button,
  Modal,
  TextInput,
  Textarea,
  Badge,
  Spinner,
  Card,
} from "flowbite-react";
import { HiPlus, HiPencil, HiTrash } from "react-icons/hi";
import { toast } from "react-hot-toast";

const TagManager: React.FC = () => {
  const [tags, setTags] = useState<Tag[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [editingTag, setEditingTag] = useState<Tag | null>(null);
  const [formData, setFormData] = useState<CreateTagRequest>({
    name: "",
    description: "",
    color: "#3B82F6",
    isActive: true,
  });

  // Load tags
  const loadTags = async () => {
    setIsLoading(true);
    try {
      const response = await getAllTagsClient();
      if (response.isSuccess) {
        setTags(response.data || []);
      } else {
        toast.error("Failed to load tags");
      }
    } catch (error) {
      console.error("Error loading tags:", error);
      toast.error("Error loading tags");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    loadTags();
  }, []);

  // Handle form submission
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      if (editingTag) {
        // Update existing tag
        const response = await updateTagClient(editingTag.tagId, formData);
        if (response.isSuccess) {
          toast.success("Tag updated successfully!");
          setTags(
            tags.map((tag) =>
              tag.tagId === editingTag.tagId ? response.data : tag
            )
          );
        } else {
          toast.error(response.message || "Failed to update tag");
        }
      } else {
        // Create new tag
        const response = await createTagClient(formData);
        if (response.isSuccess) {
          toast.success("Tag created successfully!");
          setTags([...tags, response.data]);
        } else {
          toast.error(response.message || "Failed to create tag");
        }
      }

      handleCloseModal();
    } catch (error) {
      console.error("Error saving tag:", error);
      toast.error("Error saving tag");
    }
  };

  // Handle delete
  const handleDelete = async (tag: Tag) => {
    if (!confirm(`Are you sure you want to delete the tag "${tag.name}"?`)) {
      return;
    }

    try {
      const response = await deleteTagClient(tag.tagId);
      if (response.isSuccess) {
        toast.success("Tag deleted successfully!");
        setTags(tags.filter((t) => t.tagId !== tag.tagId));
      } else {
        toast.error(response.message || "Failed to delete tag");
      }
    } catch (error) {
      console.error("Error deleting tag:", error);
      toast.error("Error deleting tag");
    }
  };

  // Open modal for creating/editing
  const handleOpenModal = (tag?: Tag) => {
    if (tag) {
      setEditingTag(tag);
      setFormData({
        name: tag.name,
        description: tag.description || "",
        color: tag.color || "#3B82F6",
        isActive: tag.isActive,
      });
    } else {
      setEditingTag(null);
      setFormData({
        name: "",
        description: "",
        color: "#3B82F6",
        isActive: true,
      });
    }
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
    setEditingTag(null);
    setFormData({
      name: "",
      description: "",
      color: "#3B82F6",
      isActive: true,
    });
  };

  const predefinedColors = [
    "#3B82F6", // blue
    "#10B981", // green
    "#F59E0B", // yellow
    "#EF4444", // red
    "#8B5CF6", // purple
    "#06B6D4", // cyan
    "#F97316", // orange
    "#84CC16", // lime
    "#6B7280", // gray
    "#EC4899", // pink
  ];

  return (
    <div className="space-y-6">
      <Card>
        <div className="flex justify-between items-center">
          <h2 className="text-xl font-semibold text-gray-900">
            Tag Management
          </h2>
          <Button
            onClick={() => handleOpenModal()}
            size="sm"
            className="flex items-center gap-2"
          >
            <HiPlus size={16} />
            Create Tag
          </Button>
        </div>

        {isLoading ? (
          <div className="flex justify-center items-center py-8">
            <Spinner size="lg" />
            <span className="ml-2">Loading tags...</span>
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full text-sm text-left text-gray-500">
              <thead className="text-xs text-gray-700 uppercase bg-gray-50">
                <tr>
                  <th className="px-6 py-3">Name</th>
                  <th className="px-6 py-3">Description</th>
                  <th className="px-6 py-3">Color</th>
                  <th className="px-6 py-3">Status</th>
                  <th className="px-6 py-3">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y">
                {tags.map((tag) => (
                  <tr key={tag.tagId} className="bg-white border-b">
                    <td className="px-6 py-4">
                      <Badge
                        color="info"
                        style={{
                          backgroundColor: tag.color || "#3B82F6",
                          color: "white",
                        }}
                      >
                        {tag.name}
                      </Badge>
                    </td>
                    <td className="px-6 py-4">{tag.description || "-"}</td>
                    <td className="px-6 py-4">
                      <div className="flex items-center gap-2">
                        <div
                          className="w-6 h-6 rounded-full border"
                          style={{ backgroundColor: tag.color || "#3B82F6" }}
                        />
                        <span className="text-sm text-gray-600">
                          {tag.color || "#3B82F6"}
                        </span>
                      </div>
                    </td>
                    <td className="px-6 py-4">
                      <Badge color={tag.isActive ? "success" : "failure"}>
                        {tag.isActive ? "Active" : "Inactive"}
                      </Badge>
                    </td>
                    <td className="px-6 py-4">
                      <div className="flex gap-2">
                        <Button
                          size="xs"
                          color="gray"
                          onClick={() => handleOpenModal(tag)}
                        >
                          <HiPencil size={12} />
                        </Button>
                        <Button
                          size="xs"
                          color="failure"
                          onClick={() => handleDelete(tag)}
                        >
                          <HiTrash size={12} />
                        </Button>
                      </div>
                    </td>
                  </tr>
                ))}
                {tags.length === 0 && (
                  <tr>
                    <td colSpan={5} className="text-center py-8">
                      No tags found. Create your first tag!
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}
      </Card>

      {/* Create/Edit Modal */}
      <Modal show={showModal} onClose={handleCloseModal}>
        <div className="p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">
            {editingTag ? "Edit Tag" : "Create New Tag"}
          </h3>

          <form onSubmit={handleSubmit}>
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Tag Name *
                </label>
                <TextInput
                  type="text"
                  placeholder="Enter tag name"
                  value={formData.name}
                  onChange={(e) =>
                    setFormData({ ...formData, name: e.target.value })
                  }
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Description
                </label>
                <Textarea
                  placeholder="Enter tag description (optional)"
                  value={formData.description}
                  onChange={(e) =>
                    setFormData({ ...formData, description: e.target.value })
                  }
                  rows={3}
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Color
                </label>
                <div className="grid grid-cols-5 gap-2 mb-3">
                  {predefinedColors.map((color) => (
                    <button
                      key={color}
                      type="button"
                      className={`w-10 h-10 rounded-full border-2 ${
                        formData.color === color
                          ? "border-gray-800"
                          : "border-gray-300"
                      }`}
                      style={{ backgroundColor: color }}
                      onClick={() => setFormData({ ...formData, color })}
                    />
                  ))}
                </div>
                <TextInput
                  type="color"
                  value={formData.color}
                  onChange={(e) =>
                    setFormData({ ...formData, color: e.target.value })
                  }
                  className="w-full"
                />
              </div>

              <div className="flex items-center">
                <input
                  type="checkbox"
                  id="isActive"
                  checked={formData.isActive}
                  onChange={(e) =>
                    setFormData({ ...formData, isActive: e.target.checked })
                  }
                  className="mr-2"
                />
                <label htmlFor="isActive" className="text-sm text-gray-700">
                  Active
                </label>
              </div>

              {/* Preview */}
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Preview
                </label>
                <Badge
                  color="info"
                  style={{
                    backgroundColor: formData.color,
                    color: "white",
                  }}
                >
                  {formData.name || "Tag Name"}
                </Badge>
              </div>
            </div>

            <div className="flex justify-end gap-3 mt-6">
              <Button color="gray" onClick={handleCloseModal} type="button">
                Cancel
              </Button>
              <Button type="submit">
                {editingTag ? "Update Tag" : "Create Tag"}
              </Button>
            </div>
          </form>
        </div>
      </Modal>
    </div>
  );
};

export default TagManager;
