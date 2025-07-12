// prettier-ignore-start
import "react-native-get-random-values";
// prettier-ignore-end

import "@/global.css";
import { Stack } from "expo-router";

import { SafeAreaProvider } from "react-native-safe-area-context";

export default function ComplainLayout() {
  return (
    <SafeAreaProvider>
      {/* <Stack /> */}
      <Stack screenOptions={{ title: "Complains" }}>
        <Stack.Screen name="complains" />
        <Stack.Screen
          name="general/GeneralComplainList"
          options={{ title: "General Complains" }}
        />
        <Stack.Screen
          name="general/AddGeneralComplain"
          options={{ title: "Add General Complains" }}
        />

        <Stack.Screen
          name="lightpost/LightPostComplainList"
          options={{ title: "Light Post Complains" }}
        />
      </Stack>
    </SafeAreaProvider>
  );
}
