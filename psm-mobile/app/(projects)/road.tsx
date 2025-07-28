import {
  BottomSheetModal,
  BottomSheetModalProvider,
  BottomSheetTextInput,
  BottomSheetView,
} from "@gorhom/bottom-sheet";
import { useRouter } from "expo-router";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { FlatList, View } from "react-native";
import { GestureHandlerRootView } from "react-native-gesture-handler";
import { Button, Card, Text } from "react-native-paper";
import { SafeAreaProvider, SafeAreaView } from "react-native-safe-area-context";

export default function Road() {
  const router = useRouter();
  const [projects, setProjects] = useState([]);
  const [textSearch, setTextSearch] = useState("testtest");

  const [citySearch, setCitySearch] = useState("");

  useEffect(() => {
    fetchProjects(null, null, null, null, true);
  }, []);

  const fetchProjects = async (type, status, name, city, isRecent) => {
    console.log("Fetching projects with filters:", {
      type,
      status,
      name,
      city,
      isRecent,
    });
    const response = await fetch(
      `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/project/filter`,
      {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          type: type,
          status: status,
          name: name,
          city: city,
          isRecent: isRecent,
        }),
      }
    );
    const data = await response.json();
    //console.log("Fetched projects:", data);
    setProjects(data);
  };

  const bottomSheetModalRef = useRef<BottomSheetModal>(null);
  const snapPoints = useMemo(() => ["50%", "75%"], []);

  // callbacks
  const handlePresentModalPress = useCallback(() => {
    bottomSheetModalRef.current?.present();
  }, []);
  const handleSheetChanges = useCallback((index: number) => {
    console.log("handleSheetChanges", index);
  }, []);

  function navigateToDetail(item) {
    router.navigate({
      pathname: "/projectDetail",
      params: { projectId: item.id },
    });
  }

  function itemRender({ item }) {
    return (
      <View className="mx-3 mt-2">
        <Card className="flex-1 w-full bg-red-200">
          <View className="flex items-stretch justify-center p-4 gap-2">
            <View className="flex-row gap-6">
              <Text>{item.name}</Text>
            </View>
            <View className="flex-row justify-between">
              <View className="bg-blue-400 rounded-lg py-1 px-2 elevation-md">
                <Text className="font-bold text-lg">{item?.status}</Text>
              </View>
              <Text>{item.endDate}</Text>
            </View>
            <View className="flex-row justify-between items-center">
              <View>
                <Text>{item.city}</Text>
              </View>
              <View>
                <Button onPress={() => navigateToDetail(item)} mode="contained">
                  View
                </Button>
              </View>
            </View>
          </View>
        </Card>
      </View>
    );
  }

  function searchProjects() {
    fetchProjects(null, null, textSearch, citySearch, false);
    bottomSheetModalRef.current?.dismiss();
  }

  // const handleTextChange = (e) => {
  //   setTextSearch(text);
  // };

  // const handleCityChange = (text) => {
  //   setCitySearch(text);
  // };

  return (
    <GestureHandlerRootView>
      <BottomSheetModalProvider>
        <SafeAreaProvider>
          <SafeAreaView className="flex-1">
            <View>
              <Button onPress={handlePresentModalPress}>Search</Button>
            </View>
            <View className="mb-5">
              <FlatList
                data={projects}
                renderItem={itemRender} // Replace with your item rendering
                keyExtractor={(item, index) => String(index)} // Use a unique key
              />
            </View>
          </SafeAreaView>
        </SafeAreaProvider>
        <BottomSheetModal
          ref={bottomSheetModalRef}
          onChange={handleSheetChanges}
          snapPoints={snapPoints}
        >
          <BottomSheetView className="flex-1 min-h-[30vh]">
            <View className="flex items-center justify-center h-full">
              <View className="w-full px-4 mt-5">
                <View>
                  <Text>Search By</Text>
                  <BottomSheetTextInput
                    onChangeText={(text) => setTextSearch(text)}
                    //value={textSearch}
                    className="mt-1  rounded-lg text-lg bg-blue-100 p-2"
                  />
                </View>
              </View>
              <View className="w-full px-4 mt-4">
                <Text>City</Text>

                <BottomSheetTextInput
                  onChangeText={() => setCitySearch}
                  //value={citySearch}
                  className="mt-1 mb-5 rounded-lg text-lg bg-blue-100 p-2"
                />
              </View>
              <View>
                <Button onPress={searchProjects} mode="contained">
                  Search
                </Button>
              </View>
            </View>
          </BottomSheetView>
        </BottomSheetModal>
      </BottomSheetModalProvider>
    </GestureHandlerRootView>
  );
}
