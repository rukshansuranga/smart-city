import {
  AdvancedMarker,
  APIProvider,
  ControlPosition,
  Map,
  MapMouseEvent,
} from "@vis.gl/react-google-maps";
import AutocompleteControl from "./autocomplete-control";
import { useState } from "react";
import { Button, Modal, ModalBody, ModalHeader } from "flowbite-react";

const API_KEY: string = (process.env.NEXT_PUBLIC_GOOGLE_MAPS_API_KEY ??
  process.env.NEXT_PUBLIC_GOOGLE_MAPS_API_KEY) as string;

interface Position {
  lat: number;
  lng: number;
}

interface AutoCompleteMapProps {
  handleSelectPlace: (lat: number, lng: number, city: string) => void;
  position: Position;
}

export default function AutoCompleteMap({
  handleSelectPlace,
  position,
}: AutoCompleteMapProps) {
  //const [position, setPosition] = useState<Position>(initialPosition);
  const [openModal, setOpenModal] = useState(false);

  const handleMapClick = (event: MapMouseEvent) => {
    console.log("Map clicked at:", event);
    const lat = event.detail.latLng?.lat;
    const lng = event.detail.latLng?.lng;

    if (lat !== undefined && lng !== undefined) {
      handleSelectPlace(lat, lng, "");
    }
  };

  const handleMarkerDrag = (e: google.maps.MapMouseEvent) => {
    const lat = e.latLng?.lat();
    const lng = e.latLng?.lng();

    if (lat !== undefined && lng !== undefined) {
      handleSelectPlace(lat, lng, "");
    }
  };

  return (
    <>
      <Button onClick={() => setOpenModal(true)}>Map</Button>
      <Modal
        dismissible
        show={openModal}
        onClose={() => setOpenModal(false)}
        size="7xl"
        className="w-full max-w-[95%]"
      >
        <ModalHeader>Create Ticket</ModalHeader>
        <ModalBody className="p-0">
          {position?.lat && position?.lng && (
            <APIProvider apiKey={API_KEY}>
              <div className="h-[80vh] w-full">
                <Map
                  zoom={15}
                  center={position}
                  mapId={process.env.NEXT_PUBLIC_MAP_ID}
                  onClick={handleMapClick}
                  gestureHandling={"greedy"}
                >
                  <AutocompleteControl
                    controlPosition={ControlPosition.TOP_LEFT}
                    onPlaceSelect={handleSelectPlace}
                  />
                  <AdvancedMarker
                    position={position}
                    draggable={true}
                    onDragEnd={handleMarkerDrag}
                  />
                </Map>
              </div>
            </APIProvider>
          )}
        </ModalBody>
      </Modal>
    </>
  );
}
