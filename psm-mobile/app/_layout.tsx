// prettier-ignore-start
import "react-native-get-random-values";
// prettier-ignore-end

import "@/global.css";
import { Stack } from "expo-router";

import { SafeAreaProvider } from "react-native-safe-area-context";

export default function RootLayout() {
  return (
    <SafeAreaProvider>
      {/* <Stack /> */}
      <Stack>
        <Stack.Screen name="index" options={{ title: "Home" }} />
        <Stack.Screen name="(complains)" options={{ headerShown: false }} />
        <Stack.Screen name="(garbage)" options={{ headerShown: false }} />
        <Stack.Screen name="(projects)" options={{ headerShown: false }} />
        <Stack.Screen name="screem2" />
      </Stack>
    </SafeAreaProvider>
  );
}
