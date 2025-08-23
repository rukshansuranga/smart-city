import nextAuth from "next-auth";
import Keycloak from "next-auth/providers/keycloak";
import jwt from "jsonwebtoken";

export const { handlers, auth, signIn, signOut } = nextAuth({
  debug: process.env.NODE_ENV === "development",
  providers: [Keycloak],
  callbacks: {
    async jwt({ token, account }) {
      if (account && account.access_token) {
        const decodedToken = jwt.decode(account?.access_token);
        //console.log("decode token", decodedToken);
        token.accessToken = account.access_token;
        if (decodedToken && typeof decodedToken !== "string") {
          const roles = decodedToken?.realm_access;
          console.log("jwtnnnn", decodedToken?.sub);
          token = { ...token, sub: decodedToken?.sub, roles: roles?.roles };
        }
      }

      //console.log("[jwt callback] token " + JSON.stringify(token));
      return token;
    },
    session: async ({ session, token }) => {
      if (session?.user) {
        // console.log("Token sub:", token.sub);
        // console.log("Tokenxxxx:", token);
        session.user.id = token.sub ?? "";
        session.roles = token.roles || [];
      }
      //console.log("[session callback] token " + JSON.stringify(session));
      session.accessToken = token.accessToken;
      return session;
    },
  },
});
