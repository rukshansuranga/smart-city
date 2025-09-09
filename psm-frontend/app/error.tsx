"use client"; // Error boundaries must be Client Components

import { useEffect } from "react";

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    // Log the error to an error reporting service
    console.error("Error occurred:", error);
  }, [error]);
  return (
    <div className="flex flex-col items-center justify-center h-screen bg-gray-100">
      <h1 className="text-3xl font-bold text-red-600 mb-4">
        Something went wrong
      </h1>
      <p className="mb-6 text-gray-700">
        An unexpected error has occurred. Please try again later or contact
        support.
      </p>
      <button
        className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
        onClick={() => reset()}
      >
        Try again
      </button>
    </div>
  );
}
