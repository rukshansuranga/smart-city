import {
  AdvancedMarker,
  InfoWindow,
  Pin,
  Map,
  APIProvider,
  ControlPosition,
} from "@vis.gl/react-google-maps";
import InfoWindowCard from "./InfoWindowCard";
import { useState } from "react";
import { ActiveLightPostMarker } from "@/types";
import AutocompleteControl from "./autocomplete-control";
import { getLightPostByLocation } from "@/app/api/actions/workpackageAction";

const API_KEY: string = (process.env.NEXT_PUBLIC_GOOGLE_MAPS_API_KEY ??
  process.env.NEXT_PUBLIC_GOOGLE_MAPS_API_KEY) as string;

export type AutocompleteMode = { id: string; label: string };

export default function LightPostMap({
  statusList,
  setActiveMarkers,
  activeMarkers,
  manageTicketInclusion,
  openPosition,
  openMarkerId,
  setOpenMarkerId,
  setOpenPosition,
}: {
  statusList: number[];
  setActiveMarkers: React.Dispatch<
    React.SetStateAction<ActiveLightPostMarker[]>
  >;
  activeMarkers: ActiveLightPostMarker[];
  manageTicketInclusion: (
    workpackageIds: number[],
    lightPostNumber?: string | null
  ) => void;
  openPosition?: boolean;
  openMarkerId?: string | null;
  setOpenPosition?: (open: boolean) => void;
  setOpenMarkerId?: (id: string | null) => void;
}) {
  const [mapCenter, setMapCenter] = useState(() =>
    activeMarkers?.length > 0
      ? {
          lat: activeMarkers[0]?.lightPost.latitude,
          lng: activeMarkers[0]?.lightPost.longitude,
        }
      : { lat: 0, lng: 0 }
  );

  async function handleSelectPlace(place: google.maps.places.Place | null) {
    //setSelectedPlace(place);
    if (place?.Dg?.location) {
      console.log(
        "selectedPlacexxxxx",
        place?.Dg?.location?.lat,
        place?.Dg?.location?.lng,
        statusList
      );

      await getLightPostByLocation({
        latitude: place?.Dg?.location?.lat,
        longitude: place?.Dg?.location?.lng,
        statuses: statusList,
      });

      setMapCenter({
        lat: place?.Dg?.location?.lat || 0,
        lng: place?.Dg?.location?.lng || 0,
      });
    }
  }

  if (activeMarkers?.length === 0) {
    return <div>No active light posts found.</div>;
  }

  return (
    <APIProvider apiKey={API_KEY}>
      <Map zoom={15} center={mapCenter} mapId={process.env.NEXT_PUBLIC_MAP_ID}>
        <AutocompleteControl
          controlPosition={ControlPosition.TOP_LEFT}
          onPlaceSelect={handleSelectPlace}
        />

        {/* <AutocompleteResult place={selectedPlace} /> */}

        {activeMarkers.map((marker) => (
          <div key={marker.lightPostNumber}>
            <AdvancedMarker
              position={{
                lat: marker.lightPost.latitude,
                lng: marker.lightPost.longitude,
              }}
              onClick={() => {
                setOpenPosition?.(true);
                setOpenMarkerId?.(marker.lightPostNumber);
              }}
            >
              <Pin
                background={marker.isIncluded ? "#22c55e" : "#2563eb"}
                glyph={marker.isIncluded ? "➖" : "➕"}
              />
            </AdvancedMarker>
            {openMarkerId === marker.lightPostNumber && openPosition && (
              <InfoWindow
                position={{
                  lat: marker.lightPost.latitude,
                  lng: marker.lightPost.longitude,
                }}
                onCloseClick={() => {
                  setOpenMarkerId?.(null);
                  setOpenPosition?.(false);
                }}
              >
                <InfoWindowCard
                  marker={marker}
                  onClose={(workpackageIds) => {
                    manageTicketInclusion(
                      workpackageIds,
                      marker.lightPostNumber
                    );
                    setOpenMarkerId?.(null);
                    setOpenPosition?.(false);
                  }}
                />
              </InfoWindow>
            )}
          </div>
        ))}
      </Map>
    </APIProvider>
  );
}
