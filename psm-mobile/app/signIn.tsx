import { useAuthStore } from "@/stores/authStore";
import {
  makeRedirectUri,
  useAuthRequest,
  useAutoDiscovery,
} from "expo-auth-session";
import * as WebBrowser from "expo-web-browser";
import { useEffect } from "react";
import { View } from "react-native";
import { Button } from "react-native-paper";

WebBrowser.maybeCompleteAuthSession();

// Endpoint
// const discovery = {
//   authorizationEndpoint:
//     "https://smartcity-identity-ecesd5fya0buajfs.southeastasia-01.azurewebsites.net/realms/smartcity/protocol/openid-connect/auth",
//   tokenEndpoint:
//     "https://smartcity-identity-ecesd5fya0buajfs.southeastasia-01.azurewebsites.net/realms/smartcity/protocol/openid-connect/token",
//   revocationEndpoint:
//     "https://smartcity-identity-ecesd5fya0buajfs.southeastasia-01.azurewebsites.net/realms/smartcity/protocol/openid-connect/revoke",
// };

export default function SignIn() {
  const { logIn } = useAuthStore();
  const redirectUri = makeRedirectUri({
    scheme: "smart-city",
    path: "signIn",
  });

  //console.log("Redirect URI:", redirectUri);

  const discovery = useAutoDiscovery(process.env.EXPO_PUBLIC_KEYCLOAK_URL);

  // Standard login request
  const [request, response, promptAsync] = useAuthRequest(
    {
      clientId: process.env.EXPO_PUBLIC_KEYCLOAK_CLIENT_ID,
      scopes: ["openid", "profile"],
      redirectUri: redirectUri,
    },
    discovery
  );

  // Registration request (kc_action=register)
  const [registerRequest, registerResponse, promptRegisterAsync] =
    useAuthRequest(
      {
        clientId: process.env.EXPO_PUBLIC_KEYCLOAK_CLIENT_ID,
        scopes: ["openid", "profile"],
        redirectUri: redirectUri,
        extraParams: {
          kc_action: "register",
          prompt: "create",
        },
      },
      discovery
    );

  useEffect(() => {
    console.log("Login effect");
    const getToken = async ({ code, codeVerifier, redirectUri }) => {
      console.log("get token", code, codeVerifier, redirectUri);
      try {
        const formData = {
          grant_type: "authorization_code",
          client_id: process.env.EXPO_PUBLIC_KEYCLOAK_CLIENT_ID,
          code: code,
          code_verifier: codeVerifier,
          redirect_uri: redirectUri,
        };
        const formBody = [];
        for (const property in formData) {
          const encodedKey = encodeURIComponent(property);
          const encodedValue = encodeURIComponent(formData[property]);
          formBody.push(encodedKey + "=" + encodedValue);
        }

        const response = await fetch(
          `${process.env.EXPO_PUBLIC_KEYCLOAK_URL}/protocol/openid-connect/token`,
          {
            method: "POST",
            headers: {
              Accept: "application/json",
              "Content-Type": "application/x-www-form-urlencoded",
            },
            body: formBody.join("&"),
          }
        );
        if (response.ok) {
          const payload = await response.json();
          console.log("access tokenxxx:", payload.access_token);
          console.log("user infoxxx:", payload.userinfo);
          const userInfoResponse = await fetch(
            `${process.env.EXPO_PUBLIC_KEYCLOAK_URL}/protocol/openid-connect/userinfo`,
            {
              method: "GET",
              headers: {
                Authorization: `Bearer ${payload.access_token}`,
                Accept: "application/json",
              },
            }
          );

          const userInfo = await userInfoResponse.json();

          console.log("User ID (sub):", userInfo.sub); // <-- This is the user ID

          logIn({
            accessToken: payload.access_token,
            idToken: payload.id_token,
            userInfo: userInfo,
          });
        }
      } catch (e) {
        console.warn(e);
      }
    };
    console.log("Auth response:", response);
    if (response?.type === "success") {
      // Handle successful login here

      const { code } = response.params;
      console.log("Auth code:", code, request?.codeVerifier);
      getToken({
        code,
        codeVerifier: request?.codeVerifier,
        redirectUri,
      });
      console.log("Auth code:", code);
    }
  }, [response]);

  function handleLogin() {
    promptAsync();
  }

  function handleRegister() {
    promptRegisterAsync();
  }

  console.log("redirectUri:", redirectUri);
  // async function handleSignUp() {
  //   // Build the registration URL for Keycloak
  //   const registrationUrl = `${process.env.EXPO_PUBLIC_KEYCLOAK_URL}/protocol/openid-connect/auth?client_id=${encodeURIComponent(process.env.EXPO_PUBLIC_KEYCLOAK_CLIENT_ID ?? "")}&redirect_uri=${encodeURIComponent(redirectUri)}&response_type=code&scope=openid%20profile&kc_action=register&prompt=create`;
  //   await WebBrowser.openAuthSessionAsync(registrationUrl, redirectUri);
  // }

  const getToken = async ({ code, codeVerifier, redirectUri }) => {
    console.log("get token", code, codeVerifier, redirectUri);
    try {
      const formData = {
        grant_type: "authorization_code",
        client_id: process.env.EXPO_PUBLIC_KEYCLOAK_CLIENT_ID,
        code: code,
        code_verifier: codeVerifier,
        redirect_uri: redirectUri,
      };
      const formBody = [];
      for (const property in formData) {
        const encodedKey = encodeURIComponent(property);
        const encodedValue = encodeURIComponent(formData[property]);
        formBody.push(encodedKey + "=" + encodedValue);
      }

      const response = await fetch(
        `${process.env.EXPO_PUBLIC_KEYCLOAK_URL}/protocol/openid-connect/token`,
        {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/x-www-form-urlencoded",
          },
          body: formBody.join("&"),
        }
      );
      if (response.ok) {
        const payload = await response.json();
        console.log("access tokenxxx:", payload.access_token);
        console.log("user infoxxx:", payload.userinfo);
        const userInfoResponse = await fetch(
          `${process.env.EXPO_PUBLIC_KEYCLOAK_URL}/protocol/openid-connect/userinfo`,
          {
            method: "GET",
            headers: {
              Authorization: `Bearer ${payload.access_token}`,
              Accept: "application/json",
            },
          }
        );

        const userInfo = await userInfoResponse.json();

        console.log("User ID (sub):", userInfo.sub); // <-- This is the user ID

        logIn({
          accessToken: payload.access_token,
          idToken: payload.id_token,
          userInfo: userInfo,
        });
      }
    } catch (e) {
      console.warn(e);
    }
  };

  // Handle registration response (same as login)
  useEffect(() => {
    console.log("Auth response:", registerResponse);
    if (registerResponse?.type === "success") {
      // Handle successful login here

      const { code } = registerResponse.params;
      console.log("Auth code:", code, registerRequest?.codeVerifier);
      getToken({
        code,
        codeVerifier: registerRequest?.codeVerifier,
        redirectUri,
      });
      console.log("Auth code:", code);
    }
  }, [registerResponse]);

  console.log("Register Response:", registerResponse);

  return (
    <View className="justify-center flex-1 p-4">
      <View className="flex-row justify-center gap-4 mt-4">
        <Button mode="contained" onPress={handleLogin}>
          Sign In
        </Button>
        <Button mode="contained" onPress={handleRegister}>
          Sign Up
        </Button>
      </View>
    </View>
  );
}
