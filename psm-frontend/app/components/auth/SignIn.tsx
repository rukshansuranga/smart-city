"use client";
import { signIn } from "@/app/components/auth";

export default function SignIn() {
  return (
    <form action={signIn}>
      <button
        type="submit"
        className="bg-[#2A9D8F] hover:bg-[#E9C46A] hover:text-[#264653] text-white px-6 py-2 rounded-lg font-medium transition-all duration-200 shadow-sm"
      >
        Sign in
      </button>
    </form>
  );
}
