import { useRouter } from "expo-router";
import { useEffect, useState } from "react";
import { Alert, FlatList, Modal, View } from "react-native";
import {
  ActivityIndicator,
  Badge,
  Button,
  Card,
  IconButton,
  MD3Colors,
  Text,
  TextInput,
} from "react-native-paper";
import { SafeAreaProvider, SafeAreaView } from "react-native-safe-area-context";

export default function GeneralComplainList() {
  const router = useRouter();

  const [isPrivate, setIsPrivate] = useState(false);

  const [selectedGeneralComplain, setSelectedGeneralComplain] = useState(null);

  const [data, setData] = useState([]);
  const [page, setPage] = useState(1);
  const [isLoading, setIsLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true); // To track if there's more data to load

  const [modalVisible, setModalVisible] = useState(false);

  const [comment, setComment] = useState("");

  useEffect(() => {
    if (!isLoading && hasMore && page === 1) {
      fetchData(hasMore, isLoading, page, isPrivate);
    }
  }, []);

  const fetchData = async (hasMore, isLoading, page, isPrivate) => {
    //console.log(23, data.length, hasMore, isLoading);
    if (!hasMore || isLoading) return; // Prevent multiple simultaneous fetches

    setIsLoading(true);
    try {
      // Replace with your actual API call
      const response = await fetch(
        `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/workpackage/general?pageNumber=${page}&isPrivate=${isPrivate}`
      );
      const newData = await response.json();

      setData((prevData) => [...prevData, ...newData]); // Assuming newData has an 'items' array
      setPage((prevPage) => prevPage + 1);

      if (newData.length === 0) {
        // Check if no more data is available
        setHasMore(false);
      }
    } catch (error) {
      console.error("Error fetching data:", error);
    } finally {
      setIsLoading(false);
    }
  };

  function privateButtonClickHandler() {
    setData([]);
    setIsPrivate(true);
    fetchData(true, false, 1, true);
  }

  function publicButtonClickHandler() {
    setData([]);
    setIsPrivate(false);
    fetchData(true, false, 1, false);
  }

  function commentClickHandle(item) {
    setModalVisible(!modalVisible);
    setSelectedGeneralComplain(item);
  }

  async function addCommentHandler() {
    try {
      const res = await fetch(
        `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/comment`,
        {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            workPackageId: selectedGeneralComplain?.workPackageId,
            clientId: 1,
            comment: comment,
            isPrivate: isPrivate,
            type: "GeneralComplain",
          }),
        }
      );
      const data = await res.json();

      console.log("res general complain", data);
    } catch (error) {
      console.log("fetch active complains", error.message);
    }
  }

  function itemRender({ item }) {
    console.log("23", item.name, item.ticketPackages?.length);

    return isPrivate ? (
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
    ) : (
      <Card className="mt-2">
        <View className="flex mx-5 gap-6 p-4">
          <View className="flex-row justify-between ">
            <Badge size={30} style={{ backgroundColor: "green" }}>
              {item.workPackageId}
            </Badge>
            <Text>{item.name}</Text>
          </View>
          <View className="flex-row justify-between items-center">
            {item?.ticketPackages?.map((ticket) => (
              <Text>{ticket?.ticket?.title}</Text>
            ))}
            <Text>{item?.client?.name}</Text>
            <IconButton
              icon="comment"
              iconColor={MD3Colors.primary40}
              size={20}
              onPress={() => commentClickHandle(item)}
            />
          </View>
        </View>
      </Card>
    );
  }

  return (
    <SafeAreaProvider>
      <SafeAreaView className="flex-1">
        <View className="flex-1 justify-between items-center">
          <View className="w-11/12 flex-1">
            <View className="flex-row justify-between items-center mx-4 mt-4 ">
              <View>
                <Text className="font-extrabold text-xl">Complain List </Text>
              </View>
              <View>
                <Button
                  mode="contained"
                  onPress={() =>
                    router.push("/(complains)/general/AddGeneralComplain")
                  }
                >
                  Add
                </Button>
              </View>
            </View>
            <FlatList
              style={{ flex: 1 }}
              contentContainerStyle={{ paddingTop: 8, paddingBottom: 16 }}
              data={data}
              renderItem={itemRender}
              keyExtractor={(item, index) => String(index)}
              onEndReached={() =>
                fetchData(hasMore, isLoading, page, isPrivate)
              }
              onEndReachedThreshold={0.5}
              ListFooterComponent={
                isLoading && hasMore ? <ActivityIndicator size="large" /> : null
              }
            />
          </View>
          <View className="w-full h-20 justify-center">
            <View className="flex-row justify-center gap-3">
              <Button
                {...(isPrivate
                  ? { mode: "contained" }
                  : { mode: "contained-tonal" })}
                onPress={privateButtonClickHandler}
              >
                Private
              </Button>
              <Button
                {...(isPrivate
                  ? { mode: "contained-tonal" }
                  : { mode: "contained" })}
                onPress={publicButtonClickHandler}
              >
                Public
              </Button>
            </View>
          </View>
        </View>
        <Modal
          animationType="slide"
          transparent={true}
          visible={modalVisible}
          onRequestClose={() => {
            Alert.alert("Modal has been closed.");
            setModalVisible(!modalVisible);
          }}
        >
          {selectedGeneralComplain && (
            <View className="flex-1 justify-center items-center w-full px-5 bg-red-50">
              <View className="w-full p-4 bg-slate-200 rounded-md items-center elevation-sm">
                <View className="flex  w-full gap-2 p">
                  <View>
                    <Text className="font-semibold text-xl">Title</Text>
                    <Text>{selectedGeneralComplain.name}</Text>
                  </View>
                  {selectedGeneralComplain.description ? (
                    <View>
                      <Text className="font-semibold text-xl">description</Text>
                      <Text>{selectedGeneralComplain.description}</Text>
                    </View>
                  ) : null}
                  <View className="flex-row justify-between">
                    <View>
                      <Text className="font-semibold text-xl">Created By</Text>
                      <Text>{selectedGeneralComplain.client.name}</Text>
                    </View>
                    <View>
                      <Text className="font-semibold text-xl">
                        Created Date
                      </Text>
                      <Text>date</Text>
                    </View>
                  </View>
                </View>
                <View className="w-full gap-2 mt-4">
                  {selectedGeneralComplain?.comments.map((comment) => (
                    <Card key={comment.commentId}>
                      <View className="p-2">
                        <Text>{comment.text}</Text>
                        <View className="flex-row justify-end">
                          <Text className="font-semibold text-xl">
                            {comment?.client?.name}
                          </Text>
                        </View>
                      </View>
                    </Card>
                  ))}

                  <View>
                    <Text className="font-semibold text-xl">Comment</Text>
                    <TextInput
                      value={comment}
                      multiline
                      onChangeText={setComment}
                    />
                  </View>

                  <View className="flex-row justify-end">
                    <Button
                      icon="plus"
                      mode="contained"
                      onPress={addCommentHandler}
                    >
                      Add Comment
                    </Button>
                  </View>
                </View>

                <View className="mt-20">
                  <IconButton
                    icon="close"
                    iconColor={MD3Colors.primary10}
                    size={30}
                    mode="contained"
                    onPress={() => setModalVisible(!modalVisible)}
                  />
                </View>
              </View>
            </View>
          )}
        </Modal>
      </SafeAreaView>
    </SafeAreaProvider>
  );
}
