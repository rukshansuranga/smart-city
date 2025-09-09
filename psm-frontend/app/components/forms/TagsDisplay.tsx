"use client";

import React from "react";
import { Tag } from "@/types";
import { Badge } from "flowbite-react";

interface TagsDisplayProps {
  tags: Tag[];
  maxDisplay?: number;
  size?: "xs" | "sm" | "md" | "lg";
  className?: string;
}

const TagsDisplay: React.FC<TagsDisplayProps> = ({
  tags,
  maxDisplay = 5,
  size = "sm",
  className = "",
}) => {
  if (!tags || tags.length === 0) {
    return null;
  }

  const displayTags = tags.slice(0, maxDisplay);
  const remainingCount = tags.length - maxDisplay;

  return (
    <div className={`flex flex-wrap gap-1 ${className}`}>
      {displayTags.map((tag) => (
        <Badge
          key={tag.tagId}
          color="info"
          size={size}
          style={{
            backgroundColor: tag.color || "#3B82F6",
            color: "white",
          }}
          title={tag.description || tag.name}
        >
          {tag.name}
        </Badge>
      ))}

      {remainingCount > 0 && (
        <Badge color="gray" size={size}>
          +{remainingCount} more
        </Badge>
      )}
    </div>
  );
};

export default TagsDisplay;
