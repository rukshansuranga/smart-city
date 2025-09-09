"use client";

import {
  getActiveAndAssignedLightPost,
  getComplainByTicketId,
} from "@/app/api/client/complainAction";
import LightPostMap from "@/app/components/complain/lightpost/lightPostMap";
import { ActiveLightPostMarker, UpdateTicketPayload } from "@/types";
import { Button, Modal, ModalBody, ModalHeader, Spinner } from "flowbite-react";
import { useEffect, useState } from "react";
import {
  addWorkpackagesToTicket,
  removeWorkpackagesFromTicket,
} from "../api/actions/ticketActions";

export default function LightPostWorkpackageAssigned({
  ticketId,
}: {
  ticketId: number;
}) {
  const [activeMarkers, setActiveMarkers] = useState<ActiveLightPostMarker[]>(
    []
  );
  const [loading, setLoading] = useState(false);

  const [openLightPostModal, setOpenLightPostModal] = useState(false);
  const [openPosition, setOpenPosition] = useState(false);
  const [openMarkerId, setOpenMarkerId] = useState<string | null>(null);

  useEffect(() => {
    fetchMarkersByTicketId();
  }, []);

  async function fetchMarkersByTicketId() {
    setLoading(true);
    const response = await getComplainByTicketId(ticketId);

    if (!response.isSuccess) {
      console.error("Error fetching workpackages:", response.message);
      return;
    }

    const data = response.data;
    const markers = await getActiveAndAssignedLightPost();

    if (!markers.isSuccess) {
      console.error("Error fetching active light posts:", markers.message);
      setLoading(false);
      return;
    }

    const updatedMarkers = markers.data.map((marker) => {
      let isContain = false;

      return {
        lightPostNumber: marker.lightPostNumber,
        //isIncluded: marker.isIncluded,
        complains: marker.complains.map((complain) => {
          if (data.some((w) => w.complainId === complain.complainId)) {
            isContain = true;

            return {
              ...complain,
              isChecked: true,
            };
          } else {
            return {
              ...complain,
              isChecked: false,
            };
          }
        }),
        isIncluded: isContain,
        lightPost: marker.lightPost,
      };
    });

    setActiveMarkers(updatedMarkers);

    setLoading(false);
  }

  async function manageTicketInclusion(
    workpackageIds: number[],
    lightPostNumber: string
  ) {
    const addingComplains: number[] = [];
    const removingComplains: number[] = [];

    setActiveMarkers((prevMarkers) => {
      const updatedMarkers = prevMarkers.map((marker) => {
        if (marker.lightPostNumber === lightPostNumber) {
          let isIncluded = false;
          return {
            ...marker,
            complains: marker.complains.map((complain) => {
              const isContain = workpackageIds.includes(
                parseInt(complain.complainId!)
              );

              console.log("is container", workpackageIds, complain.complainId);

              if (isContain) {
                isIncluded = true;
                addingComplains.push(parseInt(complain.complainId!));

                return { ...complain, isChecked: true };
              } else {
                removingComplains.push(parseInt(complain.complainId!));
                return { ...complain, isChecked: false };
              }
            }),
            isIncluded: isIncluded, // Set the inclusion status based on the checked complains
          };
        } else {
          return marker;
        }
      });

      return updatedMarkers;
    });

    await Promise.all([
      addingComplains.length > 0 && addingComplainsToTicket(addingComplains),
      removingComplains.length > 0 &&
        removingComplainsFromTicket(removingComplains),
    ]);
  }

  async function addingComplainsToTicket(complainIds: number[]) {
    const updateTicketPayload: UpdateTicketPayload = {
      ticketId,
      complainIds,
    };
    await addWorkpackagesToTicket(updateTicketPayload);
  }

  async function removingComplainsFromTicket(complainIds: number[]) {
    const updateTicketPayload: UpdateTicketPayload = {
      ticketId,
      complainIds,
    };
    await removeWorkpackagesFromTicket(updateTicketPayload);
  }

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  console.log("Active Light Post Markers:", activeMarkers);

  return (
    <div className="flex-col justify-between">
      <div>
        <Button onClick={() => setOpenLightPostModal(true)}>
          Manage Light Post Complains
        </Button>
      </div>

      <Modal
        show={openLightPostModal}
        onClose={() => setOpenLightPostModal(false)}
        className="z-10000"
        size="7xl"
      >
        <ModalHeader>Manage Light Post Complains</ModalHeader>
        <ModalBody>
          <div style={{ height: "100vh", width: "100%" }}>
            <LightPostMap
              statusList={[0, 1]}
              // setActiveMarkers={setActiveMarkers}
              activeMarkers={activeMarkers}
              manageTicketInclusion={manageTicketInclusion}
              openPosition={openPosition}
              openMarkerId={openMarkerId}
              setOpenPosition={setOpenPosition}
              setOpenMarkerId={setOpenMarkerId}
            />
          </div>
        </ModalBody>
      </Modal>
    </div>
  );
}
