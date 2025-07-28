"use client";
import { useEffect, useState } from "react";
import { getWorkpackagePaging } from "../api/actions/workpackageAction";

import { Paging, Workpackage } from "@/types";
import {
  Button,
  ButtonGroup,
  Checkbox,
  Modal,
  ModalBody,
  ModalHeader,
} from "flowbite-react";

import CreateTicketForm from "../ticket/new/popup";
import { useSearchParams, useRouter } from "next/navigation";
import WorkpackagPaging from "./WorkpackagePaging";

const itemsPerPage = 2;

export default function WorkpackagePage() {
  const router = useRouter();

  const [data, setData] = useState<Paging<Workpackage>>({
    records: [],
    totalItems: 0,
  });

  const [statusList, setStatusList] = useState<string[]>([
    "Created",
    "Open",
    "New",
  ]);

  const [selectedWorkpackages, setSelectedWorkpackages] = useState<
    Workpackage[]
  >([]);

  const searchParams = useSearchParams();
  const pageIndex = searchParams.get("pageIndex") ?? "1";

  const [openModal, setOpenModal] = useState<boolean>(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const result = await getWorkpackagePaging({
          status: statusList.join(","),
          pageSize: itemsPerPage.toString(),
          pageIndex: pageIndex,
          duration: "60",
        });

        setData(result);
      } catch (error) {
        console.error("Error fetching workpackage data:", error);
      }
    };

    fetchData();
  }, [searchParams]);

  console.log("data", data);

  useEffect(() => {
    router.push(
      `?pageSize=${itemsPerPage}&pageIndex=1&status=${statusList.join(",")}`
    );
  }, [statusList]);

  function addWorkpackage(
    workpackage: Workpackage | undefined,
    e: React.ChangeEvent<HTMLInputElement>
  ) {
    if (e.target.checked) {
      if (
        !selectedWorkpackages.find(
          (wp) => wp.workPackageId === workpackage?.workPackageId
        )
      ) {
        setSelectedWorkpackages((prev) => [...prev, workpackage!]);
      }
    } else {
      setSelectedWorkpackages(
        selectedWorkpackages.filter(
          (wp) => wp.workPackageId !== workpackage?.workPackageId
        )
      );
    }
  }

  function openPopupModel(initWorkpackage: Workpackage, open: boolean) {
    if (
      !selectedWorkpackages.find(
        (wp) => wp.workPackageId === initWorkpackage?.workPackageId
      ) ||
      selectedWorkpackages.length === 0
    ) {
      setSelectedWorkpackages((prev) => [...prev, initWorkpackage!]);
    }

    setOpenModal(open);
  }

  function statusChange(status: string) {
    if (statusList.includes(status)) {
      setStatusList(statusList.filter((sta) => sta !== status));
    } else {
      setStatusList((prev) => [...prev, status]);
    }
  }

  if (data.records.length === 0) {
    return (
      <div className="flex justify-center items-center h-screen">
        <h1 className="text-2xl font-bold">No Workpackages Found</h1>
      </div>
    );
  }

  return (
    <div className="flex-col">
      <div className="flex justify-center mb-5">
        <ButtonGroup className="gap-0.5">
          <Button
            className={`shadow-lg ${
              statusList.includes("Created") ? "bg-green-700" : ""
            } `}
            onClick={() => statusChange("Created")}
          >
            Created
          </Button>
          <Button
            className={`shadow-lg ${
              statusList.includes("Open") ? "bg-green-700" : ""
            } `}
            onClick={() => statusChange("Open")}
          >
            Open
          </Button>
          <Button
            className={`shadow-lg ${
              statusList.includes("Pending") ? "bg-green-700" : ""
            } `}
            onClick={() => statusChange("Pending")}
          >
            Pending
          </Button>
          <Button
            className={`shadow-lg ${
              statusList.includes("Done") ? "bg-green-700" : ""
            } `}
            onClick={() => statusChange("Done")}
          >
            Done
          </Button>
          <Button
            className={`shadow-lg ${
              statusList.includes("Closed") ? "bg-green-700" : ""
            } `}
            onClick={() => statusChange("Closed")}
          >
            Closed
          </Button>
        </ButtonGroup>
      </div>
      <div className="w-full">
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
              <th scope="col" className="px-6 py-3">
                {" "}
              </th>
            </tr>
          </thead>
          <tbody>
            {data?.records.map((workpackage) => (
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
                <td className="px-6 py-4">
                  {new Date(workpackage.createdDate).toISOString().slice(0, 10)}
                </td>
                <td className="px-6 py-4">{workpackage.status}</td>
                <td className="px-6 py-4">
                  <Checkbox
                    id="accept"
                    onChange={(e) => addWorkpackage(workpackage, e)}
                    checked={selectedWorkpackages
                      .map((x) => x.workPackageId)
                      .includes(workpackage.workPackageId)}
                  />
                </td>
                <td className="px-6 py-4">
                  <Button onClick={() => openPopupModel(workpackage, true)}>
                    Create Ticket
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="flex justify-center mt-5 ">
        {data?.records?.length > 0 ? (
          <WorkpackagPaging
            totalCount={data?.totalItems}
            itemsPerPage={itemsPerPage}
            pageIndex={parseInt(pageIndex)}
          />
        ) : null}
      </div>
      <Modal dismissible show={openModal} onClose={() => setOpenModal(false)}>
        <ModalHeader>Create Ticket</ModalHeader>
        <ModalBody>
          <CreateTicketForm
            open={setOpenModal}
            packages={selectedWorkpackages}
          />
        </ModalBody>
      </Modal>
    </div>
  );
}
