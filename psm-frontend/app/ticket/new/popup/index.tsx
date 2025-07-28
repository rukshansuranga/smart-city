"use client";

import { Workpackage } from "@/types";
import TicketForm from "../../TicketForm";

export default function CreateTicketForm({
  open,
  packages,
}: {
  open: (open: boolean) => void;
  packages: Workpackage[];
}) {
  function handleClose() {
    open(false);
  }

  return (
    <div className="flex-col">
      <TicketForm
        isInternal={false}
        handleClose={handleClose}
        workpackageIdList={packages.map((pkg) => pkg.workPackageId!)}
      />
      <div className="w-full mt-4">
        <table className="w-full bg-gray-200 text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
          <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
              <th scope="col" className="px-6 py-3">
                ID
              </th>
              <th scope="col" className="px-6 py-3">
                Name
              </th>
            </tr>
          </thead>
          <tbody>
            {packages.map((workpackage) => (
              <tr key={workpackage.workPackageId}>
                <td className="px-6 py-4">{workpackage.workPackageId}</td>
                <td className="px-6 py-4">{workpackage.name}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
