import { useState } from "react";
import { View } from "react-native";
import { Button, Checkbox, Text, TextInput } from "react-native-paper";

export default function AddGeneralComplain() {
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
            name: title,
            detail: description,
            isPrivate: isPrivate,
          }),
        }
      );
      const data = await res.json();

      console.log("res general complain", data);
    } catch (error) {
      console.log("fetch active complains", error.message);
    }
  }

  return (
    <View className="flex-1 justify-center mx-4 gap-2">
      <View className="w-full">
        <Text>Title</Text>
      </View>
      <View className="w-full">
        <TextInput placeholder="Enter Title" onChangeText={setTitle} />
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
  );
}
