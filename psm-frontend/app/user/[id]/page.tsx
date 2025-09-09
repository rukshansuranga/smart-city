"use client";
import { useEffect, useState } from "react";
import { useParams, useSearchParams } from "next/navigation";
import { Spinner } from "flowbite-react";
import toast from "react-hot-toast";

import UserForm from "@/app/components/forms/UserForm";
import UserDetails from "@/app/components/user/UserDetails";
import { User } from "@/types";
import { getUserById } from "@/app/api/client/userActions";

export default function UserPage() {
  const params = useParams();
  const searchParams = useSearchParams();
  const userId = params.id as string;
  const mode = searchParams.get("mode") || "view"; // "view" or "edit"

  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadUser = async () => {
      try {
        setLoading(true);
        setError(null);

        const response = await getUserById(userId);
        if (response.isSuccess) {
          setUser(response.data);
        } else {
          setError(response.message || "Failed to load user");
          toast.error(response.message || "Failed to load user");
        }
      } catch (error) {
        console.error("Error loading user:", error);
        const errorMessage = "Failed to load user";
        setError(errorMessage);
        toast.error(errorMessage);
      } finally {
        setLoading(false);
      }
    };

    if (userId) {
      loadUser();
    }
  }, [userId]);

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-64">
        <Spinner size="xl" />
        <span className="ml-3 text-lg">Loading user...</span>
      </div>
    );
  }

  if (error || !user) {
    return (
      <div className="container mx-auto px-4 py-6">
        <div className="bg-red-50 border border-red-200 rounded-lg p-6 text-center">
          <div className="text-red-600 text-lg font-medium">
            {error || "User not found"}
          </div>
          <p className="text-red-500 mt-2">
            The user you&apos;re looking for could not be loaded.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-6">
      {mode === "edit" ? (
        <UserForm userId={userId} initialData={user} />
      ) : (
        <UserDetails user={user} />
      )}
    </div>
  );
}
