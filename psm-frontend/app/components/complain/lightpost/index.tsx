"use client";
import { getActiveLightPost } from "@/app/api/actions/workpackageAction";
import TicketForm from "../../ticket/TicketForm";
import { ActiveLightPostMarker } from "@/types";

import { Button, Modal, ModalBody, ModalHeader } from "flowbite-react";
import { useEffect, useState, useMemo } from "react";

import LightPostMap from "./lightPostMap";
import { ComplainType, TicketType } from "@/enums";

export default function LightPost() {
  const [openPosition, setOpenPosition] = useState(false);
  const [activeMarkers, setActiveMarkers] = useState<ActiveLightPostMarker[]>(
    []
  );

  const [openModal, setOpenModal] = useState(false);

  const [openMarkerId, setOpenMarkerId] = useState<string | null>(null);

  async function fetchActiveMarkers() {
    const response = await getActiveLightPost();

    if (!response.isSuccess) {
      console.error("Failed to fetch active light posts:", response.message);
      return;
    }

    console.log("Active Light Post Markers:", response.data);
    setActiveMarkers(response.data);
  }

  const allWorkpackages = useMemo(
    () =>
      activeMarkers
        .filter((marker) => marker?.isIncluded)
        .flatMap((marker) => marker.complains.filter((c) => c.isChecked)),
    [activeMarkers]
  );

  useEffect(() => {
    fetchActiveMarkers();
  }, []);

  // Utility to collect all workpackages from all activeMarkers

  console.log("All workpackages:", allWorkpackages);

  function manageTicketInclusion(workpackageIds: number[]) {
    //console.log("Managing ticket inclusion for:", workpackageIds);
    setActiveMarkers((prevMarkers) => {
      return prevMarkers.map((marker) => {
        if (marker.lightPostNumber === openMarkerId) {
          let isIncluded = false;
          return {
            ...marker,
            complains: marker.complains.map((complain) => {
              const isContain = workpackageIds.includes(
                parseInt(complain.complainId ?? "")
              );

              console.log("is container", workpackageIds, complain.complainId);

              if (isContain) {
                isIncluded = true;
                return { ...complain, isChecked: true };
              } else {
                return { ...complain, isChecked: false };
              }
            }),
            isIncluded: isIncluded, // Set the inclusion status based on the checked complains
          };
        } else {
          return marker;
        }
      });

      setOpenPosition(false);
    });
  }

  function handleCloseModal() {
    setOpenModal(false);
  }

  if (activeMarkers.length === 0) {
    return <div>No active light posts found.</div>;
  }

  return (
    <div style={{ height: "100vh", width: "100%", position: "relative" }}>
      {/* Button at top right over the map */}
      <div style={{ position: "absolute", top: 16, right: 80, zIndex: 10 }}>
        <Button size="sm" onClick={() => setOpenModal(true)}>
          Create Ticket
        </Button>
      </div>
      {openModal && (
        <Modal
          show={openModal}
          onClose={() => setOpenModal(false)}
          className="z-10000"
        >
          <ModalHeader>Create Ticket</ModalHeader>
          <ModalBody>
            <TicketForm
              ticketType={TicketType.ComplainTicket}
              ticket={null}
              complainList={allWorkpackages}
              initialValue={{
                subject: "Light Post management",
                detail: "you can see the all the light post in map",
              }}
              handleClose={handleCloseModal}
              complainType={ComplainType.LightPostComplain}
            />
          </ModalBody>
        </Modal>
      )}
      <LightPostMap
        statusList={[0]}
        //setActiveMarkers={setActiveMarkers}
        activeMarkers={activeMarkers}
        manageTicketInclusion={manageTicketInclusion}
        openPosition={openPosition}
        openMarkerId={openMarkerId}
        setOpenPosition={setOpenPosition}
        setOpenMarkerId={setOpenMarkerId}
      />
    </div>
  );
}
