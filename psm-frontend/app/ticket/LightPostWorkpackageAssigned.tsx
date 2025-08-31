"use client";

import {
  getActiveAndAssignedLightPost,
  getActiveLightPost,
  getWorkpackageByTicketId,
} from "@/app/api/actions/workpackageAction";
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
    const data = await getWorkpackageByTicketId(ticketId);
    const markers = await getActiveAndAssignedLightPost();

    const updatedMarkers = markers.map((marker) => {
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
    const addingWorkpackages = [];
    const removingWorkpackages = [];

    setActiveMarkers((prevMarkers) => {
      const updatedMarkers = prevMarkers.map((marker) => {
        if (marker.lightPostNumber === lightPostNumber) {
          let isIncluded = false;
          return {
            ...marker,
            complains: marker.complains.map((complain) => {
              const isContain = workpackageIds.includes(complain.complainId);

              console.log("is container", workpackageIds, complain.complainId);

              if (isContain) {
                isIncluded = true;
                addingWorkpackages.push(complain.complainId);

                return { ...complain, isChecked: true };
              } else {
                removingWorkpackages.push(complain.complainId);
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
      addingWorkpackages.length > 0 &&
        addingWorkpackagesToTicket(addingWorkpackages),
      removingWorkpackages.length > 0 &&
        removingWorkpackagesFromTicket(removingWorkpackages),
    ]);
  }

  async function addingWorkpackagesToTicket(workpackageIds: number[]) {
    const updateTicketPayload: UpdateTicketPayload = {
      ticketId,
      workpackageIds,
    };
    await addWorkpackagesToTicket(updateTicketPayload);
  }

  async function removingWorkpackagesFromTicket(workpackageIds: number[]) {
    const updateTicketPayload: UpdateTicketPayload = {
      ticketId,
      workpackageIds,
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
              setActiveMarkers={setActiveMarkers}
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
