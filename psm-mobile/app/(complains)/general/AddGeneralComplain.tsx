import { addGeneralComplain } from "@/api/complainAction";
import { WorkpackageStatus } from "@/enums/enum"; // Adjust the import path as necessary
import { useAuthStore } from "@/stores/authStore";
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

  const { userInfo } = useAuthStore();

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [isPrivate, setIsPrivate] = useState(true);

  async function addGeneralComplainHandler() {
    try {
      const complain = {
        clientId: userInfo?.sub, // Replace with actual client ID
        subject: title,
        detail: description,
        isPrivate: isPrivate,
        status: WorkpackageStatus.New,
      };

      await addGeneralComplain(complain);

      router.push({
        pathname: "/(complains)/general/GeneralComplainList",
        params: {
          isPrivate: isPrivate.toString(),
          random: Math.random() * 10000,
        },
      });
    } catch (error) {
      console.log("fetch active complains", error.message);
    }
  }

  return (
    <SafeAreaProvider>
      <SafeAreaView className="flex-1 bg-[#c7f9cc]">
        <KeyboardAvoidingView
          behavior={Platform.OS === "ios" ? "padding" : "height"}
          style={{ flex: 1 }}
        >
          <View className="flex-1 justify-center mx-4 gap-4">
            <View className="w-full mb-2">
              <Text className="font-bold text-xl text-[#22577a]">
                Comment Title
              </Text>
              <TextInput
                placeholder="Enter your complain"
                value={title}
                onChangeText={setTitle}
                style={{
                  color: "#22577a",
                  backgroundColor: "#f6fff8",
                  borderRadius: 8,
                  borderColor: "#57cc99",
                  borderWidth: 2,
                  paddingHorizontal: 12,
                  paddingVertical: 8,
                  fontWeight: "bold",
                }}
              />
            </View>
            <View className="w-full mb-2">
              <Text className="font-bold text-xl text-[#22577a]">
                Description
              </Text>
              <TextInput
                placeholder="Enter Description"
                value={description}
                onChangeText={setDescription}
                style={{
                  color: "#22577a",
                  backgroundColor: "#f6fff8",
                  borderRadius: 8,
                  borderColor: "#57cc99",
                  borderWidth: 2,
                  paddingHorizontal: 12,
                  paddingVertical: 8,
                  fontWeight: "bold",
                }}
              />
            </View>
            <View className="flex-row items-center mb-4">
              <Checkbox
                status={isPrivate ? "checked" : "unchecked"}
                onPress={() => setIsPrivate(!isPrivate)}
                color="#38a3a5"
              />
              <Text className="ml-2 text-[#22577a] font-bold">Is Private</Text>
            </View>
            <View>
              <Button
                onPress={addGeneralComplainHandler}
                mode="contained"
                style={{ backgroundColor: "#38a3a5" }}
                labelStyle={{ color: "#fff", fontWeight: "bold" }}
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
