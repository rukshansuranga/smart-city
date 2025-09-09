"use client";

import TagManager from "@/app/components/tag/TagManager";

export default function TagsPage() {
  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-6">
        <h1 className="text-2xl font-bold text-gray-900">Tags Management</h1>
        <p className="text-gray-600 mt-2">
          Manage tags for organizing tickets, complaints, and projects.
        </p>
      </div>

      <TagManager />
    </div>
  );
}
