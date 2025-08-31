//import { Session } from "next-auth";
// eslint-disable-next-line @typescript-eslint/no-unused-vars
import { JWT } from "next-auth/jwt";

declare module "next-auth" {
  interface Session {
    id: string;
    roles: string[];
    user: User;
    accessToken?: string;
    council?: string;
  }

  interface User {
    id: string;
    role: number;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    id: string;
    roles: string[];
    accessToken: string;
    council?: string;
  }
}
