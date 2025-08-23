import { useRouter } from "expo-router";
import { Text, View } from "react-native";
import { Button } from "react-native-paper";

export default function Index() {
  const router = useRouter();

  return (
    <View className="flex-1 justify-center items-center gap-6 bg-[#c7f9cc] px-4">
      {[
        // Button data for DRY formatting
        { label: "Complains", route: "/(complains)" },
        { label: "Garbage", route: "/garbage" },
        { label: "Projects", route: "/(projects)/road" },
        { label: "Payments" },
        { label: "Notifications" },
        { label: "Profile", route: "/editUser" },
      ].map(({ label, route }, idx) => (
        <View
          key={label}
          className="flex justify-center items-center w-full rounded-xl shadow-md h-20 px-6"
          style={{ backgroundColor: idx % 2 === 0 ? "#80ed99" : "#57cc99" }}
        >
          <Button
            onPress={route ? () => router.navigate(route) : undefined}
            style={{ width: "100%", height: "100%" }}
            contentStyle={{ height: "100%" }}
          >
            <Text className="font-bold text-3xl tracking-wide text-center w-full text-[#22577a]">
              {label}
            </Text>
          </Button>
        </View>
      ))}
    </View>
  );
}
