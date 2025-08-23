import {
  AdvancedMarker,
  InfoWindow,
  Pin,
  Map,
  APIProvider,
} from "@vis.gl/react-google-maps";
import InfoWindowCard from "./InfoWindowCard";
import { useState, useRef, useEffect } from "react";
import { ActiveLightPostMarker } from "@/types";

export default function LightPostMap({
  activeMarkers,
  manageTicketInclusion,
  openPosition,
  openMarkerId,
  setOpenMarkerId,
  setOpenPosition,
}: {
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
  const inputRef = useRef<HTMLInputElement>(null);
  const autocompleteRef = useRef<google.maps.places.Autocomplete | null>(null);

  useEffect(() => {
    function initAutocomplete() {
      if (window.google && inputRef.current) {
        autocompleteRef.current = new window.google.maps.places.Autocomplete(
          inputRef.current
        );
        autocompleteRef.current.addListener("place_changed", () => {
          const place = autocompleteRef.current?.getPlace();
          if (place?.geometry?.location) {
            setMapCenter({
              lat: place.geometry.location.lat(),
              lng: place.geometry.location.lng(),
            });
          }
        });
      }
    }

    if (!window.google || !window.google.maps || !window.google.maps.places) {
      const script = document.createElement("script");
      script.src = `https://maps.googleapis.com/maps/api/js?key=${
        process.env.NEXT_PUBLIC_GOOGLE_MAPS_API_KEY || ""
      }&libraries=places`;
      script.async = true;
      script.onload = () => {
        initAutocomplete();
      };
      document.body.appendChild(script);
    } else {
      initAutocomplete();
    }
  }, []);

  // Load Google Maps script with Places library
  function initAutocomplete() {
    if (window.google && inputRef.current) {
      const autocomplete = new window.google.maps.places.Autocomplete(
        inputRef.current
      );
      autocomplete.addListener("place_changed", () => {
        const place = autocomplete.getPlace();
        if (place.geometry && place.geometry.location) {
          setMapCenter({
            lat: place.geometry.location.lat(),
            lng: place.geometry.location.lng(),
          });
        }
      });
    }
  }

  if (!window.google) {
    const script = document.createElement("script");
    script.src = `https://maps.googleapis.com/maps/api/js?key=${
      process.env.NEXT_PUBLIC_GOOGLE_MAPS_API_KEY || ""
    }&libraries=places`;
    script.async = true;
    script.onload = () => initAutocomplete();
    document.body.appendChild(script);
  } else {
    initAutocomplete();
  }
  if (activeMarkers?.length === 0) {
    return <div>No active light posts found.</div>;
  }

  console.log("mapcenter", mapCenter, autocompleteRef.current);

  return (
    <APIProvider apiKey={process.env.NEXT_PUBLIC_GOOGLE_MAPS_API_KEY}>
      <div
        style={{
          position: "absolute",
          zIndex: 10,
          width: "300px",
          left: "50%",
          transform: "translateX(-50%)",
          top: 16,
          boxShadow: "0 2px 8px rgba(0,0,0,0.15)",
          background: "#fff",
          padding: "4px",
        }}
      >
        <input
          ref={inputRef}
          type="text"
          placeholder="Search location..."
          style={{
            width: "100%",
            padding: "8px",
            borderRadius: "4px",
            border: "1px solid #ccc",
            fontSize: "16px",
          }}
        />
      </div>
      <Map zoom={15} center={mapCenter} mapId={process.env.NEXT_PUBLIC_MAP_ID}>
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
