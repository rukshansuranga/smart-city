"use client";
import {
  getWorkpackageByTicketId,
  manageMappingByTicketAndPackage,
} from "@/app/api/actions/workpackageAction";
import { Workpackage } from "@/types";
import { Button } from "flowbite-react";
import { useEffect, useState } from "react";
import WorkpackageList from "./WorkpackageList";

export default function WorkpackageAssigned({
  ticketId,
}: {
  ticketId: number;
}) {
  const [workpackages, setWorkpackages] = useState<Workpackage[]>([]);

  useEffect(() => {
    async function fetchWorkpackagesByTicketId() {
      const data = await getWorkpackageByTicketId(ticketId);
      setWorkpackages(data);
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
        (x) => x.workPackageId !== workpackageId
      );
      setWorkpackages(filterdWorkpackage);
    } catch (error) {}
  }

  async function handleWorkpackageClick(
    workpackage: Workpackage,
    isChecked: boolean
  ) {
    if (isChecked) {
      await manageMappingByTicketAndPackage({
        ticketId: ticketId,
        workpackageId: parseInt(workpackage.workPackageId!),
        action: "Add",
      });
      setWorkpackages((list) => [...list, workpackage]);
    } else {
      await manageMappingByTicketAndPackage({
        ticketId: ticketId,
        workpackageId: parseInt(workpackage.workPackageId!),
        action: "Remove",
      });
      const filterd = workpackages.filter(
        (w) => w.workPackageId !== workpackage?.workPackageId
      );
      setWorkpackages(filterd);
    }

    console.log("handleWorkpackageClick", id, isChecked);
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
                key={workpackage.workPackageId}
                className="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 border-gray-200"
              >
                <th
                  scope="row"
                  className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                >
                  {workpackage.workPackageId}
                </th>
                <td className="px-6 py-4">{workpackage.name}</td>
                <td className="px-6 py-4">{workpackage.createdDate}</td>
                <td className="px-6 py-4">{workpackage.status}</td>
                <td>
                  <Button
                    onClick={() =>
                      RemoveWorkpackage(parseInt(workpackage.workPackageId!))
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
