import { useEffect, useState } from "react";
import { FlatList, View } from "react-native";
import { Badge, Card, IconButton, Text } from "react-native-paper";

export default function ComplainListModel({ project }) {
  const [data, setData] = useState([]);

  useEffect(() => {
    // Fetch data or perform any side effects here
    fetchComplainList();
  }, []);

  async function fetchComplainList() {
    try {
      const response = await fetch(
        `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/workpackage/projectcomplains/${project.id}`
      );

      const result = await response.json();
      setData(result);
    } catch (error) {
      console.log("fetch active complains", error?.message);
    }
  }

  function itemRender({ item }) {
    console.log("23", item.name, item.ticketPackages?.length);

    return (
      <Card className="w-full mt-2">
        <View className="flex-row items-center mx-5 p-4">
          <View className="w-10/12 gap-4">
            <View className="flex-row gap-6">
              <Badge size={30} style={{ backgroundColor: "green" }}>
                {item.workPackageId}
              </Badge>
              <Text>{item.name}</Text>
            </View>
            <View className="flex-row justify-between">
              {item?.ticketPackages?.map((ticket) => (
                <Text>{ticket?.ticket?.title}</Text>
              ))}

              <Text>2026-1-2</Text>
            </View>
          </View>
          {item?.ticketPackages?.length === 0 ? (
            <View className="flex-row justify-end w-2/12">
              <IconButton icon="delete" />
            </View>
          ) : null}
        </View>
      </Card>
    );
  }

  return (
    <View>
      <FlatList
        data={data}
        renderItem={itemRender} // Replace with your item rendering
        keyExtractor={(item, index) => String(index)} // Use a unique key
      />
    </View>
  );
}
