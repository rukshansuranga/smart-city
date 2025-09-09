"use client";

export const generateKeycloakLogoutUrl = (
  redirectUrl: string,
  idToken?: string
): string => {
  const CLIENT_ID = process.env.NEXT_PUBLIC_AUTH_KEYCLOAK_ID ?? "";
  const AUTH_KEYCLOAK_ISSUER =
    process.env.NEXT_PUBLIC_AUTH_KEYCLOAK_ISSUER ?? "";
  const urlParams = new URLSearchParams();
  urlParams.append("client_id", CLIENT_ID);
  urlParams.append(
    "post_logout_redirect_uri",
    `${redirectUrl}/api/auth/logout`
  );
  if (idToken) {
    urlParams.append("id_token_hint", idToken);
  }
  return `${AUTH_KEYCLOAK_ISSUER}/protocol/openid-connect/logout?${urlParams.toString()}`;
};
export default function SignOut() {
  return (
    <a
      href={generateKeycloakLogoutUrl(process.env.NEXT_PUBLIC_AUTH_URL ?? "")}
      className="bg-[#E76F51] hover:bg-[#F4A261] text-white px-4 py-2 rounded-lg font-medium transition-all duration-200 shadow-sm"
    >
      Sign out
    </a>
  );
}
