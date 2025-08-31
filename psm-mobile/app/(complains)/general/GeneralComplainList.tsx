import { addComment } from "@/api/commentAction";
import {
  deleteGeneralComplain,
  GetGeneralComplainPaging,
} from "@/api/complainAction";
import { useAuthStore } from "@/stores/authStore";
import { WorkpackageStatus } from "@/types";
import { useLocalSearchParams, useRouter } from "expo-router";
import { useEffect, useState } from "react";
import { Alert, FlatList, Modal, ScrollView, Text, View } from "react-native";

import {
  ActivityIndicator,
  Badge,
  Button,
  Card,
  IconButton,
  MD2Colors,
  TextInput,
} from "react-native-paper";
import { SafeAreaProvider, SafeAreaView } from "react-native-safe-area-context";

const pageSize = 10;

export default function GeneralComplainList() {
  const router = useRouter();
  const { userInfo } = useAuthStore();
  const params = useLocalSearchParams();

  const [isPrivate, setIsPrivate] = useState(() => {
    if (params.isPrivate !== undefined) {
      return params.isPrivate === "true";
    }
    return false;
  });

  console.log("isPrivate", isPrivate, params.isPrivate);

  const [selectedGeneralComplain, setSelectedGeneralComplain] = useState(null);

  const [data, setData] = useState([]);
  const [page, setPage] = useState(1);
  const [isLoading, setIsLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true); // To track if there's more data to load

  const [modalVisible, setModalVisible] = useState(false);

  const [comment, setComment] = useState("");

  useEffect(() => {
    if (params?.isPrivate) {
      fetchData(params.isPrivate === "true");
    } else {
      fetchData();
    }
  }, [params?.random]);

  // Fetch data when modal becomes visible
  useEffect(() => {
    if (modalVisible) {
      fetchData(isPrivate);
    }
  }, [modalVisible]);

  async function fetchData(isPrivate = false) {
    setIsLoading(true);
    try {
      const res = await GetGeneralComplainPaging(1, isPrivate, pageSize);
      setData(res);
      //setIsLoading(false);
    } catch (error) {
    } finally {
      setIsLoading(false);
    }
  }

  function privateButtonClickHandler() {
    setData([]);
    setIsPrivate(true);
    fetchData(true);
  }

  function publicButtonClickHandler() {
    setData([]);
    setIsPrivate(false);
    fetchData(false);
  }

  function commentClickHandle(item) {
    setModalVisible(!modalVisible);
    setSelectedGeneralComplain(item);
  }

  async function addCommentHandler() {
    try {
      const newComment = {
        complainId: selectedGeneralComplain?.complainId,
        clientId: userInfo.sub, // Replace with actual client ID
        text: comment,
        isPrivate: isPrivate,
        type: "GeneralComplain",
      };

      console.log("newComment", newComment);

      await addComment(newComment);
      fetchData(isPrivate);
      setComment("");

      setModalVisible(!modalVisible);
    } catch (error) {
      console.log("fetch active complains", error.message);
    }
  }

  async function deletePrivateComplainHandler(item) {
    try {
      await deleteGeneralComplain(item.complainId);

      const newData = await GetGeneralComplainPaging(page, isPrivate, pageSize);

      setData(newData);

      console.log("res general complain", newData);
    } catch (error) {
      console.log("fetch active complains", error.message);
    } finally {
    }
  }

  console.log("comment list", selectedGeneralComplain?.comments);

  function itemRender({ item }) {
    // Color scheme
    // #22577a, #38a3a5, #57cc99, #80ed99, #c7f9cc
    if (isPrivate) {
      return (
        <Card className="w-full mt-2 border border-[#57cc99] bg-[#80ed99]">
          <View className="flex-row items-center mx-5 p-4">
            <View className="w-10/12 gap-4">
              <View className="flex-row gap-6 items-center">
                <Badge
                  size={30}
                  style={{ backgroundColor: "#38a3a5", color: "#fff" }}
                >
                  {item.complainId}
                </Badge>
                <Text className="text-[#22577a] font-bold text-base">
                  {item.subject}
                </Text>
              </View>
              <View className="flex-row justify-between items-center mt-2">
                <View className="flex-row gap-2">
                  {item?.ticketPackages?.map((ticket, idx) => (
                    <Text key={idx} className="text-[#22577a] text-xs">
                      {ticket?.ticket?.subject}
                    </Text>
                  ))}
                </View>
                <Text className="text-[#38a3a5] text-xs">2026-1-2</Text>
              </View>
            </View>
            {item?.ticketPackages?.length === 0 ? (
              <View className="flex-row justify-end w-2/12">
                <IconButton
                  icon="delete"
                  iconColor="#22577a"
                  size={24}
                  style={{ backgroundColor: "#57cc99" }}
                  onPress={() => deletePrivateComplainHandler(item)}
                />
              </View>
            ) : null}
          </View>
        </Card>
      );
    }
    // Public
    return (
      <Card className="mt-2 border border-[#38a3a5] bg-[#c7f9cc]">
        <View className="flex mx-5 gap-6 p-4">
          <View className="flex-row justify-between items-center">
            <Badge
              size={30}
              style={{ backgroundColor: "#38a3a5", color: "#fff" }}
            >
              {item.complainId}
            </Badge>
            <Text className="text-[#22577a] font-bold text-base">
              {item.subject}
            </Text>
          </View>
          <View className="flex-row justify-between items-center mt-2">
            <View className="flex-row gap-2">
              {item?.ticketPackages?.map((ticket) => (
                <Text
                  key={ticket?.ticket?.id || ticket?.ticketId}
                  className="text-[#22577a] text-xs"
                >
                  {ticket?.ticket?.subject}
                </Text>
              ))}
            </View>
            <Text className="text-[#38a3a5] font-bold">
              {item?.client?.name}
            </Text>
            <IconButton
              icon="comment"
              iconColor="#38a3a5"
              size={20}
              onPress={() => commentClickHandle(item)}
            />
          </View>
        </View>
      </Card>
    );
  }

  if (isLoading) {
    return (
      <View className="flex-1 justify-center items-center">
        <ActivityIndicator animating={true} color={MD2Colors.blue500} />
      </View>
    );
  }

  return (
    <SafeAreaProvider>
      <SafeAreaView className="flex-1 bg-[#c7f9cc]">
        <View className="flex-1 justify-between items-center">
          <View className="w-11/12 flex-1">
            <View className="flex-row justify-between items-center mx-4 mt-4">
              <View>
                <Text className="font-extrabold text-xl text-[#22577a]">
                  Complain List
                </Text>
              </View>
              <View>
                <Button
                  mode="contained"
                  style={{ backgroundColor: "#38a3a5" }}
                  labelStyle={{ color: "#fff", fontWeight: "bold" }}
                  onPress={() =>
                    router.push("/(complains)/general/AddGeneralComplain")
                  }
                >
                  Add
                </Button>
              </View>
            </View>
            {data.length === 0 && !isLoading ? (
              <Card className="w-full h-1/3 justify-center items-center mt-4 bg-[#80ed99]">
                <Text className="text-[#22577a]">No Complains Found</Text>
              </Card>
            ) : (
              <FlatList
                style={{ flex: 1 }}
                contentContainerStyle={{ paddingTop: 8, paddingBottom: 16 }}
                data={data}
                renderItem={itemRender}
                keyExtractor={(item, index) => item.complainId}
              />
            )}
          </View>
          <View className="w-full h-20 justify-center bg-[#80ed99]">
            <View className="flex-row justify-center gap-3">
              <Button
                {...(isPrivate
                  ? { mode: "contained" }
                  : { mode: "contained-tonal" })}
                style={{
                  backgroundColor: isPrivate ? "#22577a" : "#57cc99",
                }}
                labelStyle={{ color: "#fff", fontWeight: "bold" }}
                onPress={privateButtonClickHandler}
              >
                Private
              </Button>
              <Button
                {...(isPrivate
                  ? { mode: "contained-tonal" }
                  : { mode: "contained" })}
                style={{
                  backgroundColor: !isPrivate ? "#22577a" : "#57cc99",
                }}
                labelStyle={{ color: "#fff", fontWeight: "bold" }}
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
          {selectedGeneralComplain ? (
            <View className="flex-1 justify-center items-center bg-[#57cc99]/80">
              <View className="w-4/5 p-4 bg-[#f6fff8] rounded-2xl items-center elevation-sm gap-4 relative">
                <Text className="font-bold text-2xl self-start text-[#22577a]">
                  Add Comment
                </Text>
                <Card className="flex w-full gap-2 px-2 py-2 mt-12 bg-[#57cc99]">
                  <View>
                    <Text className="font-semibold text-xl text-[#22577a]">
                      Title
                    </Text>
                    <Text className="text-[#22577a]">
                      {selectedGeneralComplain.subject}
                    </Text>
                  </View>
                  {selectedGeneralComplain.detail ? (
                    <View>
                      <Text className="font-semibold text-xl text-[#22577a]">
                        Description
                      </Text>
                      <Text className="text-[#22577a]">
                        {selectedGeneralComplain.detail}
                      </Text>
                    </View>
                  ) : null}
                  <View className="flex-row justify-between">
                    <View>
                      <Text className="font-semibold text-xl text-[#22577a]">
                        Created By
                      </Text>
                      <Text className="text-[#22577a]">
                        {selectedGeneralComplain.client.firstName}
                      </Text>
                    </View>
                    <View className="flex items-center">
                      <Text className="font-semibold text-xl text-[#22577a]">
                        Status
                      </Text>
                      <Text className="text-[#22577a]">
                        {WorkpackageStatus[selectedGeneralComplain.status]}
                      </Text>
                    </View>
                    <View className="flex items-center">
                      <Text className="font-semibold text-xl text-[#22577a]">
                        Created Date
                      </Text>
                      <Text className="text-[#22577a]">
                        {new Date(selectedGeneralComplain.createdAt)
                          .toISOString()
                          .slice(0, 10)}
                      </Text>
                    </View>
                  </View>
                </Card>
                <View
                  className={`w-full mt-2 mb-2 ${
                    selectedGeneralComplain?.comments?.length > 0
                      ? "max-h-96"
                      : ""
                  }`}
                >
                  <ScrollView contentContainerStyle={{ gap: 4 }}>
                    {selectedGeneralComplain?.comments.length > 0 ? (
                      selectedGeneralComplain.comments.map((comment) => (
                        <Card key={comment.commentId} className="bg-[#c7f9cc]">
                          <View className="p-2">
                            <Text className="text-[#22577a]">
                              {comment.text}
                            </Text>
                            <View className="flex-row justify-end">
                              <Text className="font-semibold text-xl text-[#22577a]">
                                {comment?.client?.name}
                              </Text>
                            </View>
                          </View>
                        </Card>
                      ))
                    ) : (
                      <View className="items-center justify-center py-4">
                        <Text className="text-[#22577a]">No comments yet.</Text>
                      </View>
                    )}
                  </ScrollView>
                </View>
                <Card className="w-full pb-2 px-2 bg-[#57cc99]">
                  <View className="w-full ">
                    <Text className="font-semibold text-xl text-[#22577a]">
                      Comment
                    </Text>
                    <TextInput
                      value={comment}
                      multiline
                      onChangeText={setComment}
                      style={{
                        color: "#22577a",
                        backgroundColor: "#f6fff8",
                        borderRadius: 8,
                        borderColor: "#57cc99",
                        borderWidth: 2,
                        paddingHorizontal: 12,
                        paddingVertical: 8,
                        fontWeight: "bold",
                      }}
                    />
                  </View>
                  <Button
                    className="self-end mt-2"
                    icon="plus"
                    mode="contained"
                    style={{ backgroundColor: "#38a3a5" }}
                    labelStyle={{ color: "#fff", fontWeight: "bold" }}
                    onPress={addCommentHandler}
                  >
                    Add Comment
                  </Button>
                </Card>
                <View className="absolute top-1 right-2 z-10">
                  <IconButton
                    icon="close"
                    className="bg-[#57cc99]"
                    size={18}
                    mode="contained"
                    onPress={() => setModalVisible(false)}
                  />
                </View>
              </View>
            </View>
          ) : null}
        </Modal>
      </SafeAreaView>
    </SafeAreaProvider>
  );
}
