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
            <View className="py-2 bg-blue-300 rounded-xl elevation-sm">
              <Text className="text-center text-2xl font-medium">
                {item.name}
              </Text>
            </View>
          </Pressable>
        </View>
      ))}
    </>
  );
}
