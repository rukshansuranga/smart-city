import { ActiveLightPostMarker } from "@/types";
import { Button, Checkbox } from "flowbite-react";
import { useState } from "react";

export default function InfoWindowCard({
  marker,
  onClose,
}: {
  marker: ActiveLightPostMarker;
  onClose: (workpackageIds: number[]) => void;
}) {
  // Track checked status for each complain by id
  const [checkedStatus, setCheckedStatus] = useState(() => {
    const status: Record<number, boolean> = {};
    marker.complains.forEach((c) => {
      status[c.complainId] = c.isChecked || false;
    });
    return status;
  });

  //console.log("Initial checked status:", checkedStatus);

  const handleCheckboxChange = (id: string, checked: boolean) => {
    setCheckedStatus((prev) => ({ ...prev, [id]: checked }));
  };

  function handleClose() {
    // Pass all checked workpackageIds to parent
    const checkedIds = Object.entries(checkedStatus)
      .filter(([_, checked]) => checked)
      .map(([id]) => parseInt(id));
    //console.log("Checked complain IDs:", checkedIds);
    onClose(checkedIds);
  }

  return (
    <div className="p-4">
      <h2 className="text-lg font-semibold">
        Light Post {marker.lightPostNumber}
      </h2>
      <hr className="my-2" />
      {marker.complains.map((complain) => (
        <div
          key={complain.complainId || complain.id}
          className="flex items-center pt-2 gap-2 ml-2"
        >
          <Checkbox
            checked={checkedStatus[complain.complainId] || false}
            onChange={(e) =>
              handleCheckboxChange(complain.complainId, e.target.checked)
            }
          />
          <p>{complain.subject}</p>
        </div>
      ))}
      <Button size="xs" className="mt-2" onClick={handleClose}>
        Save
      </Button>
    </div>
  );
}
