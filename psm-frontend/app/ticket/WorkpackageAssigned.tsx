"use client";
import {
  getWorkpackageByTicketId,
  manageMappingByTicketAndPackage,
} from "@/app/api/actions/workpackageAction";
import { Workpackage } from "@/types";
import { Button, Spinner } from "flowbite-react";
import { useEffect, useState } from "react";
import WorkpackageList from "./WorkpackageList";

export default function WorkpackageAssigned({
  ticketId,
}: {
  ticketId: number;
}) {
  const [workpackages, setWorkpackages] = useState<Workpackage[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    async function fetchWorkpackagesByTicketId() {
      try {
        setLoading(true);
        const data = await getWorkpackageByTicketId(ticketId);
        setWorkpackages(data);
      } catch (error) {
        console.error("Error fetching workpackages:", error);
      } finally {
        setLoading(false);
      }
    }

    fetchWorkpackagesByTicketId();
  }, []);

  async function RemoveWorkpackage(workpackageId: number | undefined) {
    try {
      //await deleteMappingByTicketAndPackage(workpackageId!, ticketId);

      await manageMappingByTicketAndPackage({
        ticketId: ticketId,
        workpackageId: workpackageId!,
        action: "Remove",
      });

      const filterdWorkpackage = workpackages.filter(
        (x) => x.workpackageId !== workpackageId
      );
      setWorkpackages(filterdWorkpackage);
    } catch (error) {
      console.error("Error removing workpackage:", error);
    }
  }

  async function handleWorkpackageClick(
    workpackage: Workpackage,
    isChecked: boolean
  ) {
    if (isChecked) {
      await manageMappingByTicketAndPackage({
        ticketId: ticketId,
        workpackageId: parseInt(workpackage.workpackageId!),
        action: "Add",
      });
      setWorkpackages((list) => [...list, workpackage]);
    } else {
      await manageMappingByTicketAndPackage({
        ticketId: ticketId,
        workpackageId: parseInt(workpackage.workpackageId!),
        action: "Remove",
      });
      const filterd = workpackages.filter(
        (w) => w.workpackageId !== workpackage?.workpackageId
      );
      setWorkpackages(filterd);
    }
  }

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
            {workpackages.map((workpackage) => (
              <tr
                key={workpackage.workpackageId}
                className="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 border-gray-200"
              >
                <th
                  scope="row"
                  className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                >
                  {workpackage.workpackageId}
                </th>
                <td className="px-6 py-4">{workpackage.subject}</td>
                <td className="px-6 py-4">
                  {new Date(workpackage.createdAt).toISOString().slice(0, 10)}
                </td>
                <td className="px-6 py-4">{workpackage.status}</td>
                <td>
                  <Button
                    onClick={() =>
                      RemoveWorkpackage(parseInt(workpackage.workpackageId!))
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
        />
      </div>
    </div>
  );
}
