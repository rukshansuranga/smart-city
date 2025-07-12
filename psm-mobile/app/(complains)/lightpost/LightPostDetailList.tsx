import { useEffect, useState } from "react";
import { FlatList, Text, View } from "react-native";

export default function LightPostDetailList({
  postNo,
  name,
}: {
  postNo: string;
  name: string;
}) {
  const [activeComplainList, setActiveComplainList] = useState([]);

  useEffect(() => {
    console.log(23);
    async function getComplains() {
      try {
        const res = await fetch(
          `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/Workpackage/lightPost/${postNo}/${name}`
        );
        const data = await res.json();
        setActiveComplainList(data);
      } catch (error) {
        console.log("fetch active complains", error.message);
      }
    }

    getComplains();
  }, []);

  console.log(45, activeComplainList);

  const renderList = ({ item }) => {
    return (
      <View className="flex rounded-md overflow-hidden shadow-lg bg-blue-400 m-2 p-2">
        <View className="flex-row">
          <View className="w-1/3">
            <Text className="font-medium text-xl">{item.clientName}</Text>
          </View>
          <View className="w-1/3">
            <Text className="font-medium text-xl">{item.status}</Text>
          </View>
          <View className="w-1/3">
            <Text className="font-medium text-xl">{item.complainDate}</Text>
          </View>
        </View>
        <View className="flex-row justify-center">
          {item.ticketList.map((ticket) => (
            <View key={ticket.id}>
              <Text>{ticket.title}</Text>
            </View>
          ))}
        </View>
      </View>
    );
  };

  return (
    <View className="flex justify-between items-center py-4">
      <View>
        <Text className="font-bold text-2xl">Complain Details</Text>
      </View>
      <FlatList
        data={activeComplainList}
        renderItem={renderList}
        keyExtractor={(item) => item.workpackageId}
      />
    </View>
  );
}
