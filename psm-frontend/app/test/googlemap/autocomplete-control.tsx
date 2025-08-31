import React from "react";
import { ControlPosition, MapControl } from "@vis.gl/react-google-maps";

import { AutocompleteMode } from "./page";
import { AutocompleteCustom } from "@/app/components/test/googlemap/autocomplete-custom";

type CustomAutocompleteControlProps = {
  controlPosition: ControlPosition;
  selectedImplementation: AutocompleteMode;
  onPlaceSelect: (place: google.maps.places.Place | null) => void;
};

const AutocompleteControl = ({
  controlPosition,
  selectedImplementation,
  onPlaceSelect,
}: CustomAutocompleteControlProps) => {
  const { id } = selectedImplementation;

  return (
    <MapControl position={controlPosition}>
      <div className="autocomplete-control">
        {id === "custom" && (
          <AutocompleteCustom onPlaceSelect={onPlaceSelect} />
        )}
      </div>
    </MapControl>
  );
};

export default React.memo(AutocompleteControl);
