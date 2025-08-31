"use client";

import { Complain } from "@/types";
import TicketForm from "../../TicketForm";
import { TicketWorkpackageType } from "@/enums";

export default function CreateTicketForm({
  open,
  packages,
  ticketWorkpackageType,
}: {
  open: (open: boolean) => void;
  packages: Complain[];
  ticketWorkpackageType: TicketWorkpackageType;
}) {
  function handleClose() {
    open(false);
  }

  return (
    <div className="flex-col">
      <TicketForm
        ticketWorkpackageType={ticketWorkpackageType}
        isInternal={false}
        handleClose={handleClose}
        workpackageList={packages}
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
            {packages.map((complain) => (
              <tr key={complain.complainId}>
                <td className="px-6 py-4">{complain.complainId}</td>
                <td className="px-6 py-4">{complain.subject}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
