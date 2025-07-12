import { Pressable, Text, View } from "react-native";

export default function SelectList({ list, onPress }) {
  return (
    <>
      {list.map((item) => (
        <View key={item.id} className="mx-4">
          <Pressable
            onPress={() => onPress(item.id)}
            className="rounded-md bg-blue-300"
          >
            <Text className="text-center text-xl font-medium p-2">
              {item.name}
            </Text>
          </Pressable>
        </View>
      ))}
    </>
  );
}
