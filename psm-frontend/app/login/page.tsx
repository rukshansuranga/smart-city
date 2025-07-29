"use client";
import { Button } from "flowbite-react";
import { signIn } from "next-auth/react";

export default function LoginPage() {
  return (
    <div className="flex flex-col items-center justify-center h-screen">
      <Button
        onClick={() => signIn("keycloak", { callbackUrl: "/" })}
        style={{ padding: "10px 20px", fontSize: "16px", cursor: "pointer" }}
      >
        Sign In with Keycloak
      </Button>
    </div>
  );
}
