import { useEffect, useState } from "react";
import { Pressable, Text, View } from "react-native";
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
      <View className="flex-row justify-center">
        <Text className="font-bold text-2xl">Complain Summary</Text>
      </View>
      {activeComplainSummaryList?.map((complain) => (
        <Pressable
          key={complain.workpackageId}
          onPress={() => selectComplainSummary(complain.name)}
          className="rounded-md p-2 m-2"
        >
          <View
            key={complain.name}
            className="flex-row justify-around items-center py-4 bg-blue-400"
          >
            <View>
              <Text className="text-2xl">
                {complainMap.find((c) => c.id == complain.name)?.name ?? "TEST"}
              </Text>
            </View>
            <View>
              <Text className="text-3xl">{complain.count}</Text>
            </View>
          </View>
        </Pressable>
      ))}
    </>
  );
}
