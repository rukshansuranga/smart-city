import { useRouter } from "expo-router";
import { View } from "react-native";
import { Button, Text } from "react-native-paper";

export default function Index() {
  const router = useRouter();
  return (
    <View className="flex-1 justify-center items-center gap-5">
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl">
        <Button onPress={() => router.navigate("/road")}>
          <Text className="font-bold text-3xl">Road</Text>
        </Button>
      </View>
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl">
        <Button onPress={() => router.navigate("/irrigation")}>
          <Text className="font-bold text-3xl">Irrigation</Text>
        </Button>
      </View>
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl">
        <Button onPress={() => router.navigate("/construction")}>
          <Text className="font-bold text-3xl">Construction</Text>
        </Button>
      </View>
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl">
        <Button onPress={() => router.navigate("/other")}>
          <Text className="font-bold text-3xl">Other</Text>
        </Button>
      </View>
    </View>
  );
}
