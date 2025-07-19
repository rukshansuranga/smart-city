"use client";
import { signIn } from "@/app/components/auth";

export default function SignIn() {
  return (
    <form action={signIn}>
      <button type="submit">Sign in</button>
    </form>
  );
}
