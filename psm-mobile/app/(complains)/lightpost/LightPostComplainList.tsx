import { complainMap } from "@/constants";

import { createRef, useState } from "react";
import { Alert, Modal, StyleSheet, View } from "react-native";
import GooglePlacesTextInput, {
  Place,
} from "react-native-google-places-textinput";
import MapView, { Marker } from "react-native-maps";
import { Button, IconButton, MD3Colors, Text } from "react-native-paper";
import LightPostContainer from "./LightPostContainer";
import SelectList from "./SelectList";

const initialRegion = {
  latitude: 7.028372283362622,
  longitude: 80.02045957272463,
  latitudeDelta: 0.043,
  longitudeDelta: 0.034,
};

export default function LightPostComplainList() {
  const [modalVisible, setModalVisible] = useState(false);
  const [addingComplain, setAddingComplain] = useState(false);

  const [selectedPostNo, setSelectedPostNo] = useState(null);
  const mapRef = createRef();

  const [nearPoles, setNearPoles] = useState([
    {
      latitude: 7.02740345296692,
      longitude: 80.01386152129754,
      lightPostNumber: "LP001",
    },
    {
      latitude: 7.027062550953933,
      longitude: 80.02092091266162,
      lightPostNumber: "LP002",
    },
  ]);

  function markerModelHandler(postNo: string, isModel: boolean) {
    setModalVisible(isModel);
    setSelectedPostNo(postNo);
  }

  async function addComplain(id) {
    console.log("add", id);
    try {
      const res = await fetch(
        `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/workpackage/lightPost`,
        {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            lightPostNumber: selectedPostNo,
            clientId: 1,
            subject: id,
            detail: "",
          }),
        }
      );
      const data = await res.json();

      setAddingComplain(false);
    } catch (error) {
      console.log("fetch active complains", error.message);
    }
  }

  console.log("adding complain", addingComplain);

  // const userAddress = "";

  const handlePlaceSelect = async (place: Place) => {
    try {
      const data = await fetch(
        `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/workpackage/lightPost/near?latitude=${place.details?.location?.latitude}&longitude=${place.details?.location?.longitude}`
      );

      const poleList = await data.json();

      console.log({ poleList });
      setNearPoles(poleList);
      mapRef.current.animateToRegion({
        latitude: place.details?.location?.latitude,
        longitude: place.details?.location?.longitude,
        latitudeDelta: 0.044,
        longitudeDelta: 0.034,
      });
    } catch (error) {
      console.log("error", error);
    }
  };

  console.log("Selected placexx:", process.env.EXPO_PUBLIC_BACKEND_URL);

  return (
    <View className="flex-1 gap-2   p-1">
      <View>
        <GooglePlacesTextInput
          apiKey="AIzaSyBO2E_KAoN9H3bmeXlS9Np20qGmtlg-qbc"
          onPlaceSelect={handlePlaceSelect}
          fetchDetails={true}
        />
      </View>
      <MapView style={styles.map} initialRegion={initialRegion} ref={mapRef}>
        {nearPoles?.map((pole) => {
          return (
            <Marker
              onPress={() => markerModelHandler(pole.lightPostNumber, true)}
              key={pole.lightPostNumber}
              title="Picked Location"
              coordinate={{
                latitude: pole.latitude,
                longitude: pole.longitude,
              }}
            />
          );
        })}
      </MapView>
      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => {
          Alert.alert("Modal has been closed.");
          setModalVisible(!modalVisible);
        }}
      >
        {addingComplain ? (
          <View className="flex-1 justify-center items-center">
            <View className="w-4/5  p-4 bg-slate-200 rounded-2xl items-center elevation-sm gap-4">
              <Text className="font-bold text-2xl">Add List</Text>
              <View className="flex w-full gap-2">
                <SelectList list={complainMap} onPress={addComplain} />
              </View>

              <IconButton
                icon="close"
                iconColor={MD3Colors.primary10}
                size={30}
                mode="contained"
                onPress={() => setAddingComplain((prev) => !prev)}
              />
            </View>
          </View>
        ) : (
          <View className="flex-1 justify-center items-center">
            <View className="w-4/5  p-4 bg-slate-200 rounded-2xl items-center elevation-sm gap-4">
              <View className="w-full">
                {selectedPostNo && (
                  <LightPostContainer postNo={selectedPostNo} />
                )}
              </View>

              <Button
                icon="plus"
                mode="contained"
                onPress={() => setAddingComplain(true)}
              >
                Add Complain
              </Button>

              <IconButton
                icon="close"
                iconColor={MD3Colors.primary10}
                size={30}
                mode="contained"
                onPress={() => setModalVisible(!modalVisible)}
              />
            </View>
          </View>
        )}
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  map: {
    width: "100%",
    height: "100%",
  },
  modalComplainListView: {
    margin: 20,
    width: "80%",
    height: "80%",
    backgroundColor: "white",
    borderRadius: 20,
    alignItems: "center",
    justifyContent: "center",
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 5,
    opacity: 0.8,
  },
  modalAddComplainView: {
    margin: 20,
    width: "80%",
    height: "40%",
    backgroundColor: "white",
    borderRadius: 20,
    alignItems: "center",
    justifyContent: "space-around",
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 5,
    opacity: 0.8,
  },
});
