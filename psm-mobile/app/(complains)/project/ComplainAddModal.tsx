import { useRouter } from "expo-router";
import { useState } from "react";
import { View } from "react-native";
import { Button, TextInput } from "react-native-paper";

export default function ComplainAddModal({ project, closeModel }) {
  const router = useRouter();

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
            subject: complainText,
            detail: description,
            clientId: 1,
            projectId: project.id,
          }),
        }
      );

      closeModel(false);
    } catch (error) {
      console.log("fetch active complains", error?.message);
    }
  }
  return (
    <View className="flex-1 w-full justify-center items-center bg-[#c7f9cc] px-4 py-6 rounded-xl gap-4">
      <View className="w-full">
        <TextInput
          label="Complain"
          value={complainText}
          onChangeText={setComplainText}
          style={{
            backgroundColor: "#80ed99",
            color: "#22577a",
            borderRadius: 8,
            fontWeight: "bold",
          }}
          underlineColor="#38a3a5"
          activeUnderlineColor="#38a3a5"
        />
      </View>
      <View className="w-full h-36">
        <TextInput
          className="h-full"
          label="Description"
          value={description}
          onChangeText={setDescription}
          multiline
          editable
          numberOfLines={4}
          style={{
            backgroundColor: "#80ed99",
            color: "#22577a",
            borderRadius: 8,
            fontWeight: "bold",
            height: "100%",
          }}
          underlineColor="#38a3a5"
          activeUnderlineColor="#38a3a5"
        />
      </View>
      <View className="mt-4 w-full">
        <Button
          onPress={addComplainHandler}
          mode="contained"
          style={{ backgroundColor: "#38a3a5", borderRadius: 8 }}
          labelStyle={{ color: "#fff", fontWeight: "bold", fontSize: 16 }}
        >
          Add Complain
        </Button>
      </View>
    </View>
  );
}
