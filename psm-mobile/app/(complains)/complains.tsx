import { useRouter } from "expo-router";
import { View } from "react-native";
import { Button, Text } from "react-native-paper";

export default function index() {
  const router = useRouter();
  const title = "your dynamic title from params or computed";

  return (
    <View className="flex-1 justify-center items-center gap-5">
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl ">
        <Button
          onPress={() =>
            router.push("/(complains)/general/GeneralComplainList")
          }
        >
          <Text className="font-bold text-3xl text-slate-50 ">General</Text>
        </Button>
      </View>
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl ">
        <Button
          onPress={() =>
            router.push("/(complains)/lightpost/LightPostComplainList")
          }
        >
          <Text className="font-bold text-3xl text-slate-50 ">Light Post</Text>
        </Button>
      </View>
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl ">
        <Button>
          <Text className="font-bold text-3xl text-slate-50 ">Garbage </Text>
        </Button>
      </View>
      <View className="flex justify-center items-center bg-blue-300 py-6 w-3/4 rounded-xl ">
        <Button
          onPress={() =>
            router.push("/(complains)/project/SelectProjectForComplain")
          }
        >
          <Text className="font-bold text-3xl text-slate-50 ">
            Project Complain
          </Text>
        </Button>
      </View>
    </View>
  );
}
