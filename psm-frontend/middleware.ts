import { auth } from "@/auth";
import { NextResponse } from "next/server";

// Define protected routes and required roles
const protectedRoutes: { [key: string]: string[] } = {
  "/dashboard": ["admin"],
  "/project": ["admin", "manager"],
  "/complain": ["staff", "admin", "manager"],
  "/projectprogress": ["contractor"],
  // Add more routes and roles as needed
};

export async function middleware(request: Request) {
  const { pathname } = new URL(request.url);

  // Only check protected routes
  for (const route in protectedRoutes) {
    if (pathname.startsWith(route)) {
      const session = await auth();

      if (!session) {
        return NextResponse.redirect(new URL("/login", request.url));
      }

      const userRoles = session?.roles || [];
      const requiredRoles = protectedRoutes[route];
      const hasRole = requiredRoles.some((role) => userRoles.includes(role));
      if (!hasRole) {
        //return NextResponse.redirect(new URL("/login", request.url));
      }
    }
  }
  return NextResponse.next();
}

export const config = {
  matcher: ["/((?!_next/static|_next/image|favicon.ico|static/).*)"],
};
