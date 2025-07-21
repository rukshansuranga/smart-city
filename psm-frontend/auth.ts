import nextAuth from "next-auth";
import Keycloak from "next-auth/providers/keycloak";
import jwt from "jsonwebtoken";

export const { handlers, auth, signIn, signOut } = nextAuth({
  debug: process.env.NODE_ENV === "development",
  providers: [Keycloak],
  callbacks: {
    //async jwt({ token, user, account, profile }) {
    async jwt({ token, account }) {
      //console.log("account "+ JSON.stringify(account))
      // let roles = undefined as
      //   | { [key: string]: { roles: Array<string> } }
      //   | undefined;

      if (account && account.access_token) {
        const decodedToken = jwt.decode(account?.access_token);
        console.log("decode token", decodedToken);
        if (decodedToken && typeof decodedToken !== "string") {
          const roles = decodedToken?.realm_access;
          console.log("roles .....", roles);
          token = { ...token, roles: roles?.roles };
        }
      }

      console.log("[jwt callback] token " + JSON.stringify(token));
      return token;
    },
    //session: async ({ session, token, user }) => {
    session: async ({ session, token }) => {
      if (session?.user) {
        //session.user.id = token.sub;
        session.roles = token.roles || [];
      }
      console.log("[session callback] token " + JSON.stringify(session));

      return session;
    },
  },
});
