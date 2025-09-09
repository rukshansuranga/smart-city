"use client";
import React, { useState } from "react";
import { APIProvider, ControlPosition, Map } from "@vis.gl/react-google-maps";
import AutocompleteControl from "./autocomplete-control";
import AutocompleteResult from "./autocomplete-result";

// @ts-expect-error - globalThis.GOOGLE_MAPS_API_KEY may not exist on globalThis
const API_KEY: string = (globalThis.GOOGLE_MAPS_API_KEY ??
  process.env.NEXT_PUBLIC_GOOGLE_MAPS_API_KEY) as string;

export type AutocompleteMode = { id: string; label: string };

const implementations: Array<AutocompleteMode> = [
  { id: "custom", label: "Minimal Custom Build" },
  { id: "custom-hybrid", label: "Custom w/ UI Library" },
  { id: "webcomponent", label: "<gmp-place-autocomplete> component (beta)" },
];

export default function GoogleMapPage() {
  const [selectedImplementation] = useState<AutocompleteMode>(
    implementations[0]
  );

  const incompatibleVersionLoaded = Boolean(
    globalThis &&
      globalThis.google?.maps?.version &&
      !(
        globalThis.google?.maps?.version.endsWith("-alpha") ||
        globalThis.google?.maps?.version.endsWith("-beta")
      )
  );

  const [selectedPlace, setSelectedPlace] =
    useState<google.maps.places.Place | null>(null);

  if (incompatibleVersionLoaded) {
    location.reload();
    return;
  }

  return (
    <div className="h-6/6">
      <APIProvider apiKey={API_KEY} version={"beta"}>
        <Map
          mapId={"49ae42fed52588c3"}
          defaultZoom={3}
          defaultCenter={{ lat: 22.54992, lng: 0 }}
          gestureHandling={"greedy"}
          disableDefaultUI={true}
        >
          <AutocompleteControl
            controlPosition={ControlPosition.TOP_LEFT}
            selectedImplementation={selectedImplementation}
            onPlaceSelect={setSelectedPlace}
          />

          <AutocompleteResult place={selectedPlace} />
        </Map>
      </APIProvider>
    </div>
  );
}
