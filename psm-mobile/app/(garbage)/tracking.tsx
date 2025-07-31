import { Picker } from "@react-native-picker/picker";
import React, { useEffect, useState } from "react";
import { View } from "react-native";
import MapView, { PROVIDER_GOOGLE } from "react-native-maps";
import MapViewDirections from "react-native-maps-directions";
import { Text } from "react-native-paper";
import { SafeAreaProvider } from "react-native-safe-area-context";

const initialRegion = {
  latitude: 7.028372283362622,
  longitude: 80.02045957272463,
  latitudeDelta: 0.043,
  longitudeDelta: 0.034,
};

export default function Garbage() {
  const [inputDate, setInputDate] = useState("2025-07-10");
  const [region, setRegion] = useState(undefined);
  const [coordinates, setCoordinates] = useState([]);

  const [regions, setRegions] = useState([]);

  useEffect(() => {
    fetchRegions();
  }, []);

  useEffect(() => {
    if (region) {
      fetchRideDataPoints();
    }

    // Set up interval for periodic calls
    const intervalId = setInterval(() => {
      if (region) {
        fetchRideDataPoints();
      }
    }, 1000 * 60);

    // Clean up the interval when the component unmounts
    return () => clearInterval(intervalId);
  }, [region]);

  async function fetchRideDataPoints() {
    console.log("select region", region);
    try {
      const data = await fetch(
        `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/route?regionNo=${region}&date=${inputDate}`
      );

      const ride = await data.json();
      console.log("ride points", ride?.ridePoints);
      setCoordinates(ride?.ridePoints);
    } catch (error) {
      console.log("error1", error);
    }
  }

  async function fetchRegions() {
    try {
      const data = await fetch(
        `${process.env.EXPO_PUBLIC_BACKEND_URL}/api/region`
      );

      const regions = await data.json();

      setRegions(regions);
    } catch (error) {
      console.log("error", error);
    }
  }

  return (
    <SafeAreaProvider>
      <View className="flex-1 justify-start items-center gap-5 p-2">
        <View className="w-full">
          <Picker
            selectedValue={region}
            onValueChange={(itemValue, itemIndex) => setRegion(itemValue)}
          >
            {regions?.map((region) => (
              <Picker.Item
                key={region.regionNo}
                label={region.name}
                value={region.regionNo}
              />
            ))}
          </Picker>
        </View>

        <View className="flex-1 w-full">
          <MapView
            style={{ flex: 1 }}
            initialRegion={{
              latitude: 7.028372283362622,
              longitude: 80.02045957272463,
              latitudeDelta: 0.043,
              longitudeDelta: 0.034,
            }}
            provider={PROVIDER_GOOGLE}
          >
            {coordinates && coordinates.length > 1 && (
              <>
                {/* <Marker coordinate={coordinates[0]} />
                <Marker coordinate={coordinates[coordinates.length - 1]} /> */}
                {/* <Polyline
                  coordinates={coordinates}
                  strokeColor="#000"
                  strokeColors={["#7F0000"]}
                  strokeWidth={6}
                /> */}
                <MapViewDirections
                  origin={coordinates[0]}
                  waypoints={coordinates.slice(1, coordinates.length - 2)}
                  destination={coordinates[coordinates.length - 1]}
                  apikey="AIzaSyBO2E_KAoN9H3bmeXlS9Np20qGmtlg-qbc"
                  strokeWidth={3}
                  strokeColor="hotpink"
                  mode="WALKING"
                />
              </>
            )}
          </MapView>
        </View>
        <Text>Tracking</Text>
      </View>
    </SafeAreaProvider>
  );
}
