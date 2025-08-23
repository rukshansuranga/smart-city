"use client";
import { useEffect, useState } from "react";
import { getWorkpackagePaging } from "../../api/actions/workpackageAction";

import { Paging, Workpackage } from "@/types";
import {
  Button,
  ButtonGroup,
  Checkbox,
  Modal,
  ModalBody,
  ModalHeader,
  Pagination,
} from "flowbite-react";

import CreateTicketForm from "../../ticket/new/popup";
import { useSearchParams, useRouter } from "next/navigation";
import Link from "next/link";
import { Spinner } from "flowbite-react";
import { TicketWorkpackageType, WorkpackageStatus } from "@/enums";

export default function WorkpackageList({
  type,
  pageSize,
}: {
  type: string;
  pageSize: number;
}) {
  const router = useRouter();

  const [data, setData] = useState<Paging<Workpackage>>({
    records: [],
    totalItems: 0,
  });

  const [loading, setLoading] = useState<boolean>(false);

  const [statusList, setStatusList] = useState<WorkpackageStatus[]>([
    WorkpackageStatus.New,
    WorkpackageStatus.InProgress,
  ]);

  const [selectedWorkpackages, setSelectedWorkpackages] = useState<
    Workpackage[]
  >([]);
  const [ticketWorkpackageType, setTicketWorkpackageType] = useState<
    TicketWorkpackageType | undefined
  >(undefined);

  const searchParams = useSearchParams();
  const pageIndex = searchParams.get("pageIndex") ?? "1";

  const [openModal, setOpenModal] = useState<boolean>(false);

  useEffect(() => {
    if (type) {
      setTicketWorkpackageType(
        TicketWorkpackageType[type as keyof typeof TicketWorkpackageType]
      );
    }
  }, [type]);

  console.log("Ticket Workpackage Type:", ticketWorkpackageType);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const result = await getWorkpackagePaging({
          status: statusList.join(","),
          pageSize: pageSize.toString(),
          pageIndex: pageIndex,
          type: type,
          duration: "60",
        });
        console.log("Fetched workpackage data:", result);

        setData(result);
      } catch (error) {
        console.error("Error fetching workpackage data:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [searchParams]);

  function addWorkpackage(
    workpackage: Workpackage | undefined,
    e: React.ChangeEvent<HTMLInputElement>
  ) {
    if (e.target.checked) {
      if (
        !selectedWorkpackages.find(
          (wp) => wp.workpackageId === workpackage?.workpackageId
        )
      ) {
        setSelectedWorkpackages((prev) => [...prev, workpackage!]);
      }
    } else {
      setSelectedWorkpackages(
        selectedWorkpackages.filter(
          (wp) => wp.workpackageId !== workpackage?.workpackageId
        )
      );
    }
  }

  function openPopupModel(initWorkpackage: Workpackage, open: boolean) {
    if (
      !selectedWorkpackages.find(
        (wp) => wp.workpackageId === initWorkpackage?.workpackageId
      ) ||
      selectedWorkpackages.length === 0
    ) {
      setSelectedWorkpackages((prev) => [...prev, initWorkpackage!]);
    }

    setOpenModal(open);
  }

  function statusChange(status: WorkpackageStatus) {
    let list = [];
    if (statusList.includes(status)) {
      list = statusList.filter((sta) => sta !== status);
      //setStatusList(statusList.filter((sta) => sta !== status));
    } else {
      list = [...statusList, status];
      //setStatusList((prev) => [...prev, status]);
    }
    setStatusList(list);
    router.push(`?pageSize=${pageSize}&pageIndex=1&status=${list.join(",")}`);
  }

  if (loading || data.records.length === 0) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  return (
    <div className="flex-col">
      <div className="flex justify-center mb-5">
        <ButtonGroup className="gap-0.5">
          <Button
            className={`shadow-lg ${
              statusList.includes(WorkpackageStatus.New) ? "bg-green-700" : ""
            } `}
            onClick={() => statusChange(WorkpackageStatus.New)}
          >
            New
          </Button>

          <Button
            className={`shadow-lg ${
              statusList.includes(WorkpackageStatus.InProgress)
                ? "bg-green-700"
                : ""
            } `}
            onClick={() => statusChange(WorkpackageStatus.InProgress)}
          >
            In Progress
          </Button>
          <Button
            className={`shadow-lg ${
              statusList.includes(WorkpackageStatus.Close) ? "bg-green-700" : ""
            } `}
            onClick={() => statusChange(WorkpackageStatus.Close)}
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
                  {new Date(workpackage.createdAt)?.toISOString()?.slice(0, 10)}
                </td>
                <td className="px-6 py-4">
                  {WorkpackageStatus[workpackage.status]}
                </td>
                <td className="px-6 py-4">
                  {workpackage.ticketPackages?.length > 0 ? null : (
                    <Checkbox
                      id="accept"
                      onChange={(e) => addWorkpackage(workpackage, e)}
                      checked={selectedWorkpackages
                        .map((x) => x.workpackageId)
                        .includes(workpackage.workpackageId)}
                    />
                  )}
                </td>
                <td className="px-6 py-4">
                  {workpackage.ticketPackages?.length > 0 ? (
                    <Button className="bg-green-400 hover:bg-green-600">
                      <Link
                        href={`/ticket/workpackage/${workpackage.workpackageId}`}
                      >
                        View Ticket
                      </Link>
                    </Button>
                  ) : (
                    <Button onClick={() => openPopupModel(workpackage, true)}>
                      Create Ticket
                    </Button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="flex justify-center mt-5 ">
        {data?.records?.length > 0 ? (
          <Pagination
            currentPage={parseInt(pageIndex)}
            totalPages={Math.ceil(data?.totalItems / pageSize)}
            onPageChange={(page) => {
              router.push(
                `?pageSize=${pageSize}&pageIndex=${page}&status=${statusList.join(
                  ","
                )}`
              );
            }}
          />
        ) : null}
      </div>
      <Modal dismissible show={openModal} onClose={() => setOpenModal(false)}>
        <ModalHeader>Create Ticket</ModalHeader>
        <ModalBody>
          <CreateTicketForm
            open={setOpenModal}
            packages={selectedWorkpackages}
            ticketWorkpackageType={ticketWorkpackageType!}
          />
        </ModalBody>
      </Modal>
    </div>
  );
}
