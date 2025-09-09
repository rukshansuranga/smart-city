"use client";

import React, { useState, useCallback, useRef } from "react";
import { Label, Button, Card, HelperText, Badge } from "flowbite-react";
import {
  Control,
  FieldValues,
  UseControllerProps,
  useController,
} from "react-hook-form";
import {
  HiTrash,
  HiDownload,
  HiCloudUpload,
  HiDocumentText,
  HiPhotograph,
  HiVideoCamera,
} from "react-icons/hi";
import { toast } from "react-hot-toast";
import { AttachmentUpload, Attachment } from "@/types";
import { getSession } from "next-auth/react";

type Props<T extends FieldValues> = {
  label?: string;
  showlabel?: boolean;
  className?: string;
  required?: boolean;
  control: Control<T>;
  entityType: string; // "Ticket", "Complain", "Project", etc.
  entityId?: number;
  maxFiles?: number;
  maxFileSize?: number; // in MB
  acceptedFileTypes?: string[];
  allowedCategories?: string[];
  allowedAttachmentTypes?: string[];
  existingAttachments?: Attachment[];
  onAttachmentsChange?: (attachments: AttachmentUpload[]) => void;
} & UseControllerProps<T>;

const MultipleAttachmentUpload = <T extends FieldValues>(props: Props<T>) => {
  const {
    label = "Attachments",
    showlabel = true,
    maxFiles = 10,
    maxFileSize = 10, // 10MB default
    acceptedFileTypes = [
      ".pdf",
      ".doc",
      ".docx",
      ".jpg",
      ".jpeg",
      ".png",
      ".gif",
      ".mp4",
      ".avi",
    ],
    allowedCategories = ["Document", "Image", "Video", "Other"],
    allowedAttachmentTypes = [
      "Evidence",
      "Specification",
      "Progress",
      "Report",
    ],
    existingAttachments = [],
    onAttachmentsChange,
  } = props;

  const { fieldState, field } = useController({
    ...props,
  });

  const [attachments, setAttachments] = useState<AttachmentUpload[]>(
    field.value || []
  );
  const [dragActive, setDragActive] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const getFileIcon = (fileName: string) => {
    const extension = fileName.split(".").pop()?.toLowerCase();
    if (
      ["jpg", "jpeg", "png", "gif", "bmp", "webp"].includes(extension || "")
    ) {
      return <HiPhotograph className="w-6 h-6 text-blue-500" />;
    }
    if (["mp4", "avi", "mov", "wmv", "flv"].includes(extension || "")) {
      return <HiVideoCamera className="w-6 h-6 text-purple-500" />;
    }
    return <HiDocumentText className="w-6 h-6 text-gray-500" />;
  };

  const formatFileSize = (bytes: number) => {
    if (bytes === 0) return "0 Bytes";
    const k = 1024;
    const sizes = ["Bytes", "KB", "MB", "GB"];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];
  };

  const validateFile = useCallback(
    (file: File): string | null => {
      // Check file size
      if (file.size > maxFileSize * 1024 * 1024) {
        return `File size must be less than ${maxFileSize}MB`;
      }

      // Check file type
      const extension = "." + file.name.split(".").pop()?.toLowerCase();
      if (!acceptedFileTypes.includes(extension)) {
        return `File type ${extension} is not allowed`;
      }

      return null;
    },
    [maxFileSize, acceptedFileTypes]
  );

  const handleFiles = useCallback(
    (files: FileList) => {
      const newAttachments: AttachmentUpload[] = [];
      const errors: string[] = [];

      // Check total file count
      if (attachments.length + files.length > maxFiles) {
        toast.error(`Maximum ${maxFiles} files allowed`);
        return;
      }

      Array.from(files).forEach((file) => {
        const error = validateFile(file);
        if (error) {
          errors.push(`${file.name}: ${error}`);
        } else {
          newAttachments.push({
            file,
            description: "",
            attachmentType: "Document",
            category: "Other",
          });
        }
      });

      if (errors.length > 0) {
        toast.error(errors.join("\n"));
      }

      if (newAttachments.length > 0) {
        const updatedAttachments = [...attachments, ...newAttachments];
        setAttachments(updatedAttachments);
        field.onChange(updatedAttachments);
        onAttachmentsChange?.(updatedAttachments);
        toast.success(`${newAttachments.length} file(s) added`);
      }
    },
    [attachments, field, maxFiles, onAttachmentsChange, validateFile]
  );

  const handleDrop = useCallback(
    (e: React.DragEvent) => {
      e.preventDefault();
      e.stopPropagation();
      setDragActive(false);

      if (e.dataTransfer.files && e.dataTransfer.files.length > 0) {
        handleFiles(e.dataTransfer.files);
      }
    },
    [handleFiles]
  );

  const handleDragOver = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(true);
  }, []);

  const handleDragLeave = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(false);
  }, []);

  const removeAttachment = (index: number) => {
    const updatedAttachments = attachments.filter((_, i) => i !== index);
    setAttachments(updatedAttachments);
    field.onChange(updatedAttachments);
    onAttachmentsChange?.(updatedAttachments);
    toast.success("File removed");
  };

  const updateAttachmentDetails = (
    index: number,
    field: keyof AttachmentUpload,
    value: string
  ) => {
    const updatedAttachments = attachments.map((attachment, i) =>
      i === index ? { ...attachment, [field]: value } : attachment
    );
    setAttachments(updatedAttachments);
    props.control._formValues[props.name] = updatedAttachments;
    onAttachmentsChange?.(updatedAttachments);
  };

  const downloadExistingAttachment = async (attachmentId: number) => {
    try {
      const session = await getSession();
      const headers: HeadersInit = {};

      if (session?.accessToken) {
        headers.Authorization = `Bearer ${session.accessToken}`;
      }

      const baseUrl = process.env.NEXT_PUBLIC_API_URL || "";
      const response = await fetch(
        `${baseUrl}/api/attachments/${attachmentId}/download`,
        {
          headers,
        }
      );

      if (response.ok) {
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = `attachment_${attachmentId}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
      } else {
        toast.error("Failed to download file");
      }
    } catch (error) {
      console.error("Error downloading file:", error);
      toast.error("Error downloading file");
    }
  };

  return (
    <div className="w-full">
      {showlabel && (
        <div className="mt-2 mb-2 block">
          <Label htmlFor={props.name}>{label}</Label>
          {props.required && <span className="text-red-600 ml-1">*</span>}
        </div>
      )}

      {/* Drag & Drop Upload Area */}
      <div
        className={`border-2 border-dashed rounded-lg p-6 text-center transition-colors ${
          dragActive
            ? "border-blue-500 bg-blue-50"
            : fieldState.error
            ? "border-red-300 bg-red-50"
            : "border-gray-300 hover:border-gray-400"
        }`}
        onDrop={handleDrop}
        onDragOver={handleDragOver}
        onDragLeave={handleDragLeave}
      >
        <HiCloudUpload className="mx-auto h-12 w-12 text-gray-400 mb-4" />
        <p className="mb-2 text-sm text-gray-500">
          <span className="font-semibold">Click to upload</span> or drag and
          drop
        </p>
        <p className="text-xs text-gray-500 mb-4">
          {acceptedFileTypes.join(", ")} (max {maxFileSize}MB each, {maxFiles}{" "}
          files max)
        </p>
        <Button
          type="button"
          color="light"
          size="sm"
          onClick={() => fileInputRef.current?.click()}
        >
          Choose Files
        </Button>
        <input
          ref={fileInputRef}
          type="file"
          multiple
          accept={acceptedFileTypes.join(",")}
          onChange={(e) => e.target.files && handleFiles(e.target.files)}
          className="hidden"
        />
      </div>

      {/* Error Message */}
      {fieldState.error && (
        <HelperText color="failure" className="mt-1">
          {fieldState.error.message}
        </HelperText>
      )}

      {/* Existing Attachments */}
      {existingAttachments && existingAttachments.length > 0 && (
        <div className="mt-4">
          <Label className="mb-2 block">Existing Attachments</Label>
          <div className="space-y-2">
            {existingAttachments.map((attachment) => (
              <Card key={attachment.attachmentId} className="p-3">
                <div className="flex items-center justify-between">
                  <div className="flex items-center space-x-3">
                    {getFileIcon(attachment.fileName)}
                    <div className="flex-1 min-w-0">
                      <p className="text-sm font-medium text-gray-900 truncate">
                        {attachment.originalFileName}
                      </p>
                      <div className="flex items-center space-x-2 mt-1">
                        <span className="text-xs text-gray-500">
                          {formatFileSize(attachment.fileSize)}
                        </span>
                        {attachment.attachmentType && (
                          <Badge color="blue" size="xs">
                            {attachment.attachmentType}
                          </Badge>
                        )}
                        {attachment.category && (
                          <Badge color="gray" size="xs">
                            {attachment.category}
                          </Badge>
                        )}
                      </div>
                      {attachment.description && (
                        <p className="text-xs text-gray-600 mt-1">
                          {attachment.description}
                        </p>
                      )}
                    </div>
                  </div>
                  <Button
                    type="button"
                    color="light"
                    size="xs"
                    onClick={() =>
                      downloadExistingAttachment(attachment.attachmentId)
                    }
                  >
                    <HiDownload className="w-4 h-4" />
                  </Button>
                </div>
              </Card>
            ))}
          </div>
        </div>
      )}

      {/* New Attachments */}
      {attachments.length > 0 && (
        <div className="mt-4">
          <Label className="mb-2 block">
            New Attachments ({attachments.length}/{maxFiles})
          </Label>
          <div className="space-y-3">
            {attachments.map((attachment, index) => (
              <Card key={index} className="p-3">
                <div className="space-y-3">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-3">
                      {getFileIcon(attachment.file.name)}
                      <div className="flex-1 min-w-0">
                        <p className="text-sm font-medium text-gray-900 truncate">
                          {attachment.file.name}
                        </p>
                        <p className="text-xs text-gray-500">
                          {formatFileSize(attachment.file.size)}
                        </p>
                      </div>
                    </div>
                    <Button
                      type="button"
                      color="failure"
                      size="xs"
                      onClick={() => removeAttachment(index)}
                    >
                      <HiTrash className="w-4 h-4" />
                    </Button>
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                    <div>
                      <Label
                        htmlFor={`description-${index}`}
                        className="text-xs"
                      >
                        Description
                      </Label>
                      <input
                        id={`description-${index}`}
                        type="text"
                        placeholder="File description"
                        value={attachment.description || ""}
                        onChange={(e) =>
                          updateAttachmentDetails(
                            index,
                            "description",
                            e.target.value
                          )
                        }
                        className="mt-1 block w-full text-sm border-gray-300 rounded-md shadow-sm focus:border-blue-500 focus:ring-blue-500"
                      />
                    </div>

                    <div>
                      <Label htmlFor={`type-${index}`} className="text-xs">
                        Type
                      </Label>
                      <select
                        id={`type-${index}`}
                        value={attachment.attachmentType || ""}
                        onChange={(e) =>
                          updateAttachmentDetails(
                            index,
                            "attachmentType",
                            e.target.value
                          )
                        }
                        className="mt-1 block w-full text-sm border-gray-300 rounded-md shadow-sm focus:border-blue-500 focus:ring-blue-500"
                      >
                        {allowedAttachmentTypes.map((type) => (
                          <option key={type} value={type}>
                            {type}
                          </option>
                        ))}
                      </select>
                    </div>

                    <div>
                      <Label htmlFor={`category-${index}`} className="text-xs">
                        Category
                      </Label>
                      <select
                        id={`category-${index}`}
                        value={attachment.category || ""}
                        onChange={(e) =>
                          updateAttachmentDetails(
                            index,
                            "category",
                            e.target.value
                          )
                        }
                        className="mt-1 block w-full text-sm border-gray-300 rounded-md shadow-sm focus:border-blue-500 focus:ring-blue-500"
                      >
                        {allowedCategories.map((category) => (
                          <option key={category} value={category}>
                            {category}
                          </option>
                        ))}
                      </select>
                    </div>
                  </div>
                </div>
              </Card>
            ))}
          </div>
        </div>
      )}

      {/* Summary */}
      {(attachments.length > 0 || existingAttachments.length > 0) && (
        <div className="mt-3 text-sm text-gray-600">
          Total files: {(existingAttachments?.length || 0) + attachments.length}
        </div>
      )}
    </div>
  );
};

export default MultipleAttachmentUpload;
