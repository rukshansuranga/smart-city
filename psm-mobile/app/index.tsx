import { useRouter } from "expo-router";
import { View } from "react-native";
import { Button, Text } from "react-native-paper";

export default function Index() {
  const router = useRouter();

  return (
    <View className="flex-1 justify-center items-center gap-5">
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl ">
        <Button onPress={() => router.navigate("/complains")}>
          <Text className="font-bold text-3xl text-slate-50 ">Complains</Text>
        </Button>
      </View>
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl">
        <Button onPress={() => router.navigate("/garbage")}>
          <Text className="font-bold text-3xl">Garbage</Text>
        </Button>
      </View>
      <View className="flex justify-center items-center bg-blue-300 py-6  w-3/4 rounded-xl">
        <Button onPress={() => router.navigate("/(projects)/road")}>
          <Text className="font-bold text-3xl">Projects</Text>
        </Button>
      </View>

      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl">
        <Button>
          <Text className="font-bold text-3xl">Payments</Text>
        </Button>
      </View>
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl">
        <Button>
          <Text className="font-bold text-3xl">Notifications</Text>
        </Button>
      </View>
    </View>
  );
}
