import { WorkpackageStatus } from "@/enums/enum"; // Adjust the import path as necessary
import { useRouter } from "expo-router";
import { useState } from "react";
import {
  KeyboardAvoidingView,
  Platform,
  SafeAreaView,
  View,
} from "react-native";
import { Button, Checkbox, Text, TextInput } from "react-native-paper";
import { SafeAreaProvider } from "react-native-safe-area-context";

export default function AddGeneralComplain() {
  const router = useRouter();

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [isPrivate, setIsPrivate] = useState(true);

  async function addGeneralComplainHandler() {
    try {
      const res = await fetch(
        `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/workpackage/general`,
        {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
          },

          body: JSON.stringify({
            clientId: 1,
            subject: title,
            detail: description,
            isPrivate: isPrivate,
            status: WorkpackageStatus.New,
          }),
        }
      );

      const data = await res.json();

      console.log("res general complain", data);

      router.push({
        pathname: "/(complains)/general/GeneralComplainList",
        params: { isPrivate: isPrivate.toString() },
      });
    } catch (error) {
      console.log("fetch active complains", error.message);
    }
  }

  return (
    <SafeAreaProvider>
      <SafeAreaView className="flex-1">
        <KeyboardAvoidingView
          behavior={Platform.OS === "ios" ? "padding" : "height"}
          style={{ flex: 1 }}
        >
          <View className="flex-1 justify-center mx-4 gap-2">
            <View className="w-full">
              <Text>Comment Text</Text>
            </View>
            <View className="w-full">
              <TextInput
                placeholder="Enter your complain"
                onChangeText={setTitle}
              />
            </View>
            <View className="w-full">
              <Text>Description</Text>
            </View>
            <View className="w-full">
              <TextInput
                placeholder="Enter Description"
                onChangeText={setDescription}
              />
            </View>
            <View className="flex-row items-center">
              <View>
                <Checkbox
                  status={isPrivate ? "checked" : "unchecked"}
                  onPress={() => {
                    setIsPrivate(!isPrivate);
                  }}
                />
              </View>
              <View className="w-full">
                <Text>Is Private</Text>
              </View>
            </View>

            <View>
              <Button
                onPress={addGeneralComplainHandler}
                mode="contained"
                className="mt-5"
              >
                Add Complain
              </Button>
            </View>
          </View>
        </KeyboardAvoidingView>
      </SafeAreaView>
    </SafeAreaProvider>
  );
}
