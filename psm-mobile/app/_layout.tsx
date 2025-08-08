// prettier-ignore-start
import "react-native-get-random-values";
// prettier-ignore-end

import "@/global.css";
import { SplashScreen, Stack, useRouter } from "expo-router";

import { useAuthStore } from "@/stores/authStore";
// import { makeRedirectUri, useAuthRequest, useAutoDiscovery } from "expo-auth-session";
import { useEffect } from "react";
import { Text, View } from "react-native";
import { Button } from "react-native-paper";
import { SafeAreaProvider, SafeAreaView } from "react-native-safe-area-context";
// Minimal custom header component

function MinimalHeader({ options, route, logOut, userName }) {
  // Prefer options.title, then options.headerTitle, then route.name
  const title =
    options.title ||
    (typeof options.headerTitle === "string"
      ? options.headerTitle
      : undefined) ||
    (route && route.name ? route.name : "");
  const router = useRouter();
  return (
    <View className="flex-row items-center justify-between bg-blue-400 py-3 px-2">
      <Text className="text-white text-lg font-bold">{title}</Text>
      {userName && (
        <Text className="text-white text-lg font-bold">Hi, {userName}</Text>
      )}
      <Button onPress={logOut}>Logout</Button>
      <Button onPress={() => router.replace("/")}>Home</Button>
    </View>
  );
}

export default function RootLayout() {
  const { isSignedIn, _hasHydrated, accessToken, idToken, userInfo, logOut } =
    useAuthStore();

  // Auth logic is handled in signIn.tsx

  // https://zustand.docs.pmnd.rs/integrations/persisting-store-data#how-can-i-check-if-my-store-has-been-hydrated
  // Hide the splash screen after the store has been hydrated

  useEffect(() => {
    if (_hasHydrated) {
      SplashScreen.hideAsync();
    }
  }, [_hasHydrated]);

  // No useAuthRequest or response handling here

  if (!_hasHydrated) {
    return null;
  }

  // Log app version and EAS buildId
  // console.log("App Version:", Constants.expoConfig?.version);
  // console.log("EAS Build ID:", Constants.eas?.buildId);
  // console.log("EAS Object:", Constants.eas);
  // console.log("auth", accessToken);

  async function handleLogout() {
    //console.log("isSignedIn:", isSignedIn);
    try {
      // const idToken = "authState.idToken";
      //console.log("Logging out with idToken:", idToken);
      await fetch(
        `${process.env.EXPO_PUBLIC_KEYCLOAK_URL}/protocol/openid-connect/logout?id_token_hint=${idToken}`
      );
      // @ts-ignore
      logOut();
    } catch (e) {
      console.warn(e);
    }
  }

  console.log("userinfo:", userInfo);

  return (
    <SafeAreaProvider>
      <SafeAreaView className="flex-1">
        <Stack
          screenOptions={{
            header: (props) => (
              <MinimalHeader
                {...props}
                logOut={handleLogout}
                userName={userInfo?.given_name}
              />
            ), // Use custom header
          }}
        >
          <Stack.Protected guard={!isSignedIn}>
            <Stack.Screen name="signIn" />
          </Stack.Protected>

          <Stack.Protected guard={isSignedIn}>
            <Stack.Screen name="index" options={{ title: "Home" }} />
            <Stack.Screen name="(complains)" options={{ headerShown: false }} />
            <Stack.Screen name="(garbage)" options={{ headerShown: false }} />
            <Stack.Screen name="(projects)" options={{ headerShown: false }} />
          </Stack.Protected>
        </Stack>
      </SafeAreaView>
    </SafeAreaProvider>
  );
}
