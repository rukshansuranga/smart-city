"use client";
import {
  getComplainByTicketId,
  manageMappingByTicketAndPackage,
} from "@/app/api/client/complainAction";
import { Complain } from "@/types";
import { useEffect, useState } from "react";
import WorkpackageList from "./WorkpackageList";
import { ComplainType, ComplainStatus } from "@/enums";
import { Button, Spinner } from "flowbite-react";

export default function WorkpackageAssigned({
  ticketId,
  complainType,
}: {
  ticketId: number;
  complainType: ComplainType;
}) {
  const [workpackages, setWorkpackages] = useState<Complain[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    async function fetchWorkpackagesByTicketId() {
      try {
        setLoading(true);
        const response = await getComplainByTicketId(ticketId);

        if (!response.isSuccess) {
          console.error("Error fetching workpackages:", response.message);
          return;
        }

        setWorkpackages(response.data);
      } catch (error) {
        console.error("Error fetching workpackages:", error);
      } finally {
        setLoading(false);
      }
    }

    fetchWorkpackagesByTicketId();
  }, []);

  async function RemoveWorkpackage(complainId: number | undefined) {
    try {
      //await deleteMappingByTicketAndPackage(complainId!, ticketId);

      await manageMappingByTicketAndPackage({
        ticketId: ticketId,
        complainId: complainId!,
        action: "Remove",
      });

      const filterdWorkpackage = workpackages.filter(
        (x) => x.complainId !== complainId
      );
      setWorkpackages(filterdWorkpackage);
    } catch (error) {
      console.error("Error removing complain:", error);
    }
  }

  async function handleWorkpackageClick(
    complain: Complain,
    isChecked: boolean
  ) {
    if (isChecked) {
      await manageMappingByTicketAndPackage({
        ticketId: ticketId,
        complainId: parseInt(complain.complainId!),
        action: "Add",
      });
      setWorkpackages((list) => [...list, complain]);
    } else {
      await manageMappingByTicketAndPackage({
        ticketId: ticketId,
        complainId: parseInt(complain.complainId!),
        action: "Remove",
      });
      const filterd = workpackages.filter(
        (w) => w.complainId !== complain?.complainId
      );
      setWorkpackages(filterd);
    }
  }

  console.log("All workpackages:", workpackages);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  return (
    <div className="flex-col justify-between">
      <div>
        <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
          <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
              <th scope="col" className="px-6 py-3">
                ID
              </th>
              <th scope="col" className="px-6 py-3">
                Name
              </th>
              <th scope="col" className="px-6 py-3">
                Created Date
              </th>
              <th scope="col" className="px-6 py-3">
                Status
              </th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {workpackages.map((complain) => (
              <tr
                key={complain.complainId}
                className="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 border-gray-200"
              >
                <th
                  scope="row"
                  className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                >
                  {complain.complainId}
                </th>
                <td className="px-6 py-4">{complain.subject}</td>
                <td className="px-6 py-4">
                  {new Date(complain.createdAt).toISOString().slice(0, 10)}
                </td>
                <td className="px-6 py-4">{ComplainStatus[complain.status]}</td>
                <td>
                  <Button
                    onClick={() =>
                      RemoveWorkpackage(parseInt(complain.complainId!))
                    }
                  >
                    Remove
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <div className="mt-8">
        <WorkpackageList
          selectedWorkpackages={workpackages}
          handleWorkpackageClick={handleWorkpackageClick}
          complainType={complainType}
        />
      </div>
    </div>
  );
}
