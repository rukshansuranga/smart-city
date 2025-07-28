import { useState } from "react";
import { View } from "react-native";
import { Button, TextInput } from "react-native-paper";

export default function ComplainAddModal({ project }) {
  const [complainText, setComplainText] = useState("");
  const [description, setDescription] = useState("");

  async function addComplainHandler() {
    try {
      await fetch(
        `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/workpackage/projectcomplain`,
        {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            name: complainText,
            detail: description,
            clientId: 1,
            projectId: project.id,
          }),
        }
      );
    } catch (error) {
      console.log("fetch active complains", error?.message);
    }
  }
  return (
    <View className="flex-1 w-full justify-center mx-4 gap-2">
      <View className="w-full">
        <TextInput
          label="complain"
          value={complainText}
          onChangeText={setComplainText}
        />
      </View>
      <View className="w-full h-36 bg-red-400">
        <TextInput
          className="h-full"
          label="description"
          value={description}
          onChangeText={setDescription}
          multiline
          editable
          numberOfLines={4} // Initial height for 4 lines
        />
      </View>
      <View className="mt-4">
        <Button onPress={addComplainHandler} mode="contained">
          Add Complain
        </Button>
      </View>
    </View>
  );
}
