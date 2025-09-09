"use client";

import { useCouncilStore } from "@/store";
import { Button } from "flowbite-react";

export default function TestStore() {
  const { council, location, setCouncil, setLocation } = useCouncilStore();

  const testStore = () => {
    setCouncil("Test City");
    setLocation({ lat: 6.9271, lng: 79.8612 });
  };

  return (
    <div className="p-4">
      <Button onClick={testStore}>Test Store</Button>
      <pre className="mt-4">
        Current State:
        {JSON.stringify({ council, location }, null, 2)}
      </pre>
    </div>
  );
}
