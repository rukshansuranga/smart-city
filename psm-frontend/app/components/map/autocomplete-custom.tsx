import React, { FormEvent, useCallback, useState } from "react";
import { useMapsLibrary } from "@vis.gl/react-google-maps";
import { useAutocompleteSuggestions } from "@/app/hooks/map/use-autocomplete-suggestions";

interface Props {
  onPlaceSelect: (lat: number, lng: number, city: string) => void;
}

// Extended interface for Google Places with runtime properties
interface ExtendedPlace extends google.maps.places.Place {
  Dg?: {
    location?: {
      lat: number;
      lng: number;
    };
  };
}

export const AutocompleteCustom = ({ onPlaceSelect }: Props) => {
  const places = useMapsLibrary("places");

  const [inputValue, setInputValue] = useState<string>("");
  const { suggestions, resetSession } = useAutocompleteSuggestions(inputValue);

  const handleInput = useCallback((event: FormEvent<HTMLInputElement>) => {
    setInputValue((event.target as HTMLInputElement).value);
  }, []);

  const handleSuggestionClick = useCallback(
    async (suggestion: google.maps.places.AutocompleteSuggestion) => {
      if (!places) return;
      if (!suggestion.placePrediction) return;

      const place = suggestion.placePrediction.toPlace();

      await place.fetchFields({
        fields: [
          "viewport",
          "location",
          "svgIconMaskURI",
          "iconBackgroundColor",
        ],
      });

      setInputValue("");

      // calling fetchFields invalidates the session-token, so we now have to call
      // resetSession() so a new one gets created for further search
      resetSession();

      // Type assertion to access runtime properties that aren't in the type definitions
      const placeWithLocation = place as ExtendedPlace;

      onPlaceSelect(
        placeWithLocation?.Dg?.location?.lat || 0,
        placeWithLocation?.Dg?.location?.lng || 0,
        suggestion.placePrediction?.text.text || ""
      );
    },
    [places, onPlaceSelect, resetSession]
  );

  return (
    <div className="autocomplete-container">
      <div className="relative w-full min-w-md max-w-md mx-auto mt-4 ml-4">
        <input
          className="w-full bg-gray-100 border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400 transition"
          value={inputValue}
          onInput={(event) => handleInput(event)}
          placeholder="Search for a place"
        />

        {suggestions.length > 0 && (
          <ul className="absolute left-0 right-0 mt-2 bg-white border border-gray-200 rounded-lg shadow-lg z-10">
            {suggestions.map((suggestion, index) => (
              <li
                key={index}
                className="px-4 py-2 cursor-pointer hover:bg-blue-100 transition text-gray-800"
                onClick={() => handleSuggestionClick(suggestion)}
              >
                {suggestion.placePrediction?.text.text}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};
