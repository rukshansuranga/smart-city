import React from "react";
import { ControlPosition, MapControl } from "@vis.gl/react-google-maps";

import { AutocompleteCustom } from "../../map/autocomplete-custom";

type CustomAutocompleteControlProps = {
  controlPosition: ControlPosition;
  onPlaceSelect: (lat: number, lng: number, city: string) => void;
};

const AutocompleteControl = ({
  controlPosition,

  onPlaceSelect,
}: CustomAutocompleteControlProps) => {
  return (
    <MapControl position={controlPosition}>
      <div className="autocomplete-control">
        <AutocompleteCustom onPlaceSelect={onPlaceSelect} />
      </div>
    </MapControl>
  );
};

export default React.memo(AutocompleteControl);
