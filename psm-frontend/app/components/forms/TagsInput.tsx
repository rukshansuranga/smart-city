"use client";

import React, { useState, useEffect, useCallback } from "react";
import { Controller, Control } from "react-hook-form";
import { Tag } from "@/types";
import { getAllTagsClient, createTagClient } from "@/app/api/client/tagActions";
import { Badge, TextInput, Spinner } from "flowbite-react";
import { HiX, HiPlus } from "react-icons/hi";
import { toast } from "react-hot-toast";

interface TagsInputProps {
  name: string;
  control: Control<any>; // eslint-disable-line @typescript-eslint/no-explicit-any
  label?: string;
  showlabel?: boolean;
  placeholder?: string;
  entityType?: string; // e.g., "Ticket", "Complain", "Project"
  entityId?: number;
  allowCreateNew?: boolean;
  maxTags?: number;
  disabled?: boolean;
  className?: string;
  onTagsChange?: (tags: Tag[]) => void;
}

const TagsInput: React.FC<TagsInputProps> = ({
  name,
  control,
  label = "Tags",
  showlabel = true,
  placeholder = "Type to search or create tags...",
  allowCreateNew = true,
  maxTags = 10,
  disabled = false,
  className = "",
  onTagsChange,
}) => {
  const [availableTags, setAvailableTags] = useState<Tag[]>([]);
  const [filteredTags, setFilteredTags] = useState<Tag[]>([]);
  const [inputValue, setInputValue] = useState("");
  const [showDropdown, setShowDropdown] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isCreatingTag, setIsCreatingTag] = useState(false);

  // Load available tags
  const loadTags = useCallback(async () => {
    setIsLoading(true);
    try {
      const response = await getAllTagsClient();
      if (response.isSuccess) {
        setAvailableTags(response.data || []);
      } else {
        console.error("Failed to load tags:", response.message);
      }
    } catch (error) {
      console.error("Error loading tags:", error);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    loadTags();
  }, [loadTags]);

  // Filter tags based on input
  useEffect(() => {
    if (inputValue.trim()) {
      const filtered = availableTags.filter(
        (tag) =>
          tag.isActive &&
          tag.name.toLowerCase().includes(inputValue.toLowerCase())
      );
      setFilteredTags(filtered);
    } else {
      setFilteredTags(availableTags.filter((tag) => tag.isActive));
    }
  }, [inputValue, availableTags]);

  // Create new tag
  const createNewTag = async (tagName: string) => {
    if (!allowCreateNew || isCreatingTag) return;

    setIsCreatingTag(true);
    try {
      const response = await createTagClient({
        name: tagName.trim(),
        description: `Auto-created tag: ${tagName.trim()}`,
        color: getRandomColor(),
        isActive: true,
      });

      if (response.isSuccess) {
        const newTag = response.data;
        setAvailableTags((prev) => [...prev, newTag]);
        toast.success(`Tag "${tagName}" created successfully!`);
        return newTag;
      } else {
        toast.error(`Failed to create tag: ${response.message}`);
        return null;
      }
    } catch (error) {
      console.error("Error creating tag:", error);
      toast.error("Error creating tag");
      return null;
    } finally {
      setIsCreatingTag(false);
    }
  };

  // Generate random color for new tags
  const getRandomColor = () => {
    const colors = [
      "#3B82F6", // blue
      "#10B981", // green
      "#F59E0B", // yellow
      "#EF4444", // red
      "#8B5CF6", // purple
      "#06B6D4", // cyan
      "#F97316", // orange
      "#84CC16", // lime
    ];
    return colors[Math.floor(Math.random() * colors.length)];
  };

  // Handle tag selection
  const handleTagSelect = (
    tag: Tag,
    selectedTags: Tag[],
    onChange: (tags: Tag[]) => void
  ) => {
    if (selectedTags.find((t) => t.tagId === tag.tagId)) return;
    if (selectedTags.length >= maxTags) {
      toast.error(`Maximum ${maxTags} tags allowed`);
      return;
    }

    const newTags = [...selectedTags, tag];
    onChange(newTags);
    setInputValue("");
    setShowDropdown(false);
    onTagsChange?.(newTags);
  };

  // Handle tag removal
  const handleTagRemove = (
    tagId: number,
    selectedTags: Tag[],
    onChange: (tags: Tag[]) => void
  ) => {
    const newTags = selectedTags.filter((tag) => tag.tagId !== tagId);
    onChange(newTags);
    onTagsChange?.(newTags);
  };

  // Handle input key press
  const handleKeyPress = async (
    e: React.KeyboardEvent,
    selectedTags: Tag[],
    onChange: (tags: Tag[]) => void
  ) => {
    if (e.key === "Enter" && inputValue.trim()) {
      e.preventDefault();

      // Check if tag already exists
      const existingTag = availableTags.find(
        (tag) => tag.name.toLowerCase() === inputValue.trim().toLowerCase()
      );

      if (existingTag) {
        handleTagSelect(existingTag, selectedTags, onChange);
      } else if (allowCreateNew) {
        const newTag = await createNewTag(inputValue.trim());
        if (newTag) {
          handleTagSelect(newTag, selectedTags, onChange);
        }
      }
    }
  };

  return (
    <Controller
      name={name}
      control={control}
      defaultValue={[]}
      render={({ field: { value = [], onChange }, fieldState: { error } }) => (
        <div className={`relative ${className}`}>
          {showlabel && (
            <label className="block text-sm font-medium text-gray-700 mb-2">
              {label}
            </label>
          )}

          {/* Selected Tags Display */}
          <div className="flex flex-wrap gap-2 mb-2">
            {value.map((tag: Tag) => (
              <Badge
                key={tag.tagId}
                color="info"
                size="sm"
                style={{
                  backgroundColor: tag.color || "#3B82F6",
                  color: "white",
                }}
                className="flex items-center gap-1"
              >
                <span>{tag.name}</span>
                {!disabled && (
                  <button
                    type="button"
                    onClick={() => handleTagRemove(tag.tagId, value, onChange)}
                    className="ml-1 hover:bg-black/20 rounded-full p-0.5"
                  >
                    <HiX size={12} />
                  </button>
                )}
              </Badge>
            ))}
          </div>

          {/* Input Field */}
          {!disabled && value.length < maxTags && (
            <div className="relative">
              <TextInput
                type="text"
                placeholder={placeholder}
                value={inputValue}
                onChange={(e) => setInputValue(e.target.value)}
                onFocus={() => setShowDropdown(true)}
                onBlur={() => setTimeout(() => setShowDropdown(false), 200)}
                onKeyPress={(e) => handleKeyPress(e, value, onChange)}
                className="w-full"
                disabled={disabled}
              />

              {/* Dropdown */}
              {showDropdown && (
                <div className="absolute z-10 w-full mt-1 bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-y-auto">
                  {isLoading ? (
                    <div className="p-4 text-center">
                      <Spinner size="sm" />
                      <span className="ml-2">Loading tags...</span>
                    </div>
                  ) : filteredTags.length > 0 ? (
                    <>
                      {filteredTags
                        .filter(
                          (tag) =>
                            !value.find((t: Tag) => t.tagId === tag.tagId)
                        )
                        .map((tag) => (
                          <button
                            key={tag.tagId}
                            type="button"
                            className="w-full text-left px-4 py-2 hover:bg-gray-100 focus:bg-gray-100 flex items-center gap-2"
                            onClick={() =>
                              handleTagSelect(tag, value, onChange)
                            }
                          >
                            <div
                              className="w-3 h-3 rounded-full"
                              style={{
                                backgroundColor: tag.color || "#3B82F6",
                              }}
                            />
                            <span>{tag.name}</span>
                            {tag.description && (
                              <span className="text-sm text-gray-500 ml-auto">
                                {tag.description}
                              </span>
                            )}
                          </button>
                        ))}
                    </>
                  ) : inputValue.trim() && allowCreateNew ? (
                    <button
                      type="button"
                      className="w-full text-left px-4 py-2 hover:bg-gray-100 focus:bg-gray-100 flex items-center gap-2 text-blue-600"
                      onClick={() => createNewTag(inputValue.trim())}
                      disabled={isCreatingTag}
                    >
                      {isCreatingTag ? (
                        <Spinner size="sm" />
                      ) : (
                        <HiPlus size={16} />
                      )}
                      <span>
                        {isCreatingTag
                          ? "Creating..."
                          : `Create "${inputValue.trim()}"`}
                      </span>
                    </button>
                  ) : (
                    <div className="px-4 py-2 text-gray-500 text-center">
                      {inputValue.trim()
                        ? "No matching tags found"
                        : "Type to search tags"}
                    </div>
                  )}
                </div>
              )}
            </div>
          )}

          {/* Error Display */}
          {error && (
            <p className="mt-1 text-sm text-red-600">{error.message}</p>
          )}

          {/* Helper Text */}
          <div className="mt-1 text-xs text-gray-500">
            {value.length}/{maxTags} tags selected
            {allowCreateNew && " • Press Enter to create new tags"}
          </div>
        </div>
      )}
    />
  );
};

export default TagsInput;
