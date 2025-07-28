import { useEffect, useState } from "react";
import { Pressable, Text, View } from "react-native";
import { Badge } from "react-native-paper";
import { complainMap } from "../../../Constants";

export default function LightPostSummaryList({
  postNo,
  selectComplainSummary,
}: {
  postNo: string;
  selectComplainSummary: (name: string) => void;
}) {
  const [activeComplainSummaryList, setActiveComplainSummaryList] = useState(
    []
  );

  useEffect(() => {
    async function getComplains() {
      try {
        const res = await fetch(
          `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/Workpackage/lightPost/${postNo}`
        );
        const data = await res.json();
        setActiveComplainSummaryList(data);
      } catch (error) {
        console.log("fetch active complains", error.message);
      }
    }

    getComplains();
  }, []);

  if (activeComplainSummaryList.length == 0) {
    return (
      <View>
        <Text>No Complains</Text>
      </View>
    );
  }

  return (
    <>
      <View className="flex-row justify-center mb-4">
        <Text className="font-bold text-2xl">Complain Summary</Text>
      </View>
      <View className="flex gap-2">
        {activeComplainSummaryList?.map((complain) => (
          <Pressable
            key={complain.workpackageId}
            onPress={() => selectComplainSummary(complain.name)}
          >
            <View
              key={complain.name}
              className="flex-row justify-around items-center py-1 bg-blue-300 rounded-xl elevation-sm"
            >
              <View>
                <Text className="text-2xl font-medium">
                  {complainMap.find((c) => c.id == complain.name)?.name ??
                    "TEST"}
                </Text>
              </View>
              <View>
                <Badge size={35} style={{ backgroundColor: "green" }}>
                  <Text className="text-3xl">{complain.count}</Text>
                </Badge>
              </View>
            </View>
          </Pressable>
        ))}
      </View>
    </>
  );
}
