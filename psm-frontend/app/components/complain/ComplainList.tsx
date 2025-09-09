"use client";
import { useEffect, useState } from "react";
import { getComplainPaging } from "../../api/client/complainAction";

import { Paging, Complain } from "@/types";
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
import { ComplainType, ComplainStatus, TicketType } from "@/enums";

export default function ComplainList({
  type,
  pageSize,
}: {
  type: string;
  pageSize: number;
}) {
  const router = useRouter();

  const [data, setData] = useState<Paging<Complain>>({
    records: [],
    totalItems: 0,
  });

  const [loading, setLoading] = useState<boolean>(false);

  const [statusList, setStatusList] = useState<ComplainStatus[]>([
    ComplainStatus.New,
    ComplainStatus.InProgress,
  ]);

  const [selectedComplains, setSelectedComplains] = useState<Complain[]>([]);
  const [complainType, setComplainType] = useState<ComplainType | undefined>(
    undefined
  );

  const searchParams = useSearchParams();
  const pageIndex = searchParams.get("pageIndex") ?? "1";

  const [openModal, setOpenModal] = useState<boolean>(false);

  useEffect(() => {
    if (type) {
      setComplainType(ComplainType[type as keyof typeof ComplainType]);
    }
  }, [type]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await getComplainPaging({
          status: statusList.join(","),
          pageSize: pageSize.toString(),
          pageIndex: pageIndex,
          complainType: type,
          duration: "60",
        });
        console.log("Fetched complain data:", response);

        if (response.isSuccess && response.data) {
          setData(response.data);
        } else {
          console.error("Error fetching complain data:", response.message);
          setData({
            records: [],
            totalItems: 0,
          });
        }
      } catch (error) {
        console.error("Error fetching complain data:", error);
        setData({
          records: [],
          totalItems: 0,
        });
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [searchParams]);

  function addComplain(
    complain: Complain | undefined,
    e: React.ChangeEvent<HTMLInputElement>
  ) {
    if (e.target.checked) {
      if (
        !selectedComplains.find((wp) => wp.complainId === complain?.complainId)
      ) {
        setSelectedComplains((prev) => [...prev, complain!]);
      }
    } else {
      setSelectedComplains(
        selectedComplains.filter((wp) => wp.complainId !== complain?.complainId)
      );
    }
  }

  function openPopupModel(initComplain: Complain, open: boolean) {
    if (
      !selectedComplains.find(
        (wp) => wp.complainId === initComplain?.complainId
      ) ||
      selectedComplains.length === 0
    ) {
      setSelectedComplains((prev) => [...prev, initComplain!]);
    }

    setOpenModal(open);
  }

  function statusChange(status: ComplainStatus) {
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
              statusList.includes(ComplainStatus.New) ? "bg-green-700" : ""
            } `}
            onClick={() => statusChange(ComplainStatus.New)}
          >
            New
          </Button>

          <Button
            className={`shadow-lg ${
              statusList.includes(ComplainStatus.InProgress)
                ? "bg-green-700"
                : ""
            } `}
            onClick={() => statusChange(ComplainStatus.InProgress)}
          >
            In Progress
          </Button>
          <Button
            className={`shadow-lg ${
              statusList.includes(ComplainStatus.Closed) ? "bg-green-700" : ""
            } `}
            onClick={() => statusChange(ComplainStatus.Closed)}
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
            {data?.records.map((complain) => (
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
                  {new Date(complain.createdAt)?.toISOString()?.slice(0, 10)}
                </td>
                <td className="px-6 py-4">{ComplainStatus[complain.status]}</td>
                <td className="px-6 py-4">
                  {complain.ticketPackages?.length > 0 ? null : (
                    <Checkbox
                      id="accept"
                      onChange={(e) => addComplain(complain, e)}
                      checked={selectedComplains
                        .map((x) => x.complainId)
                        .includes(complain.complainId)}
                    />
                  )}
                </td>
                <td className="px-6 py-4">
                  {complain.ticketPackages?.length > 0 ? (
                    <Button className="bg-green-400 hover:bg-green-600">
                      <Link href={`/ticket/complain/${complain.complainId}`}>
                        View Ticket
                      </Link>
                    </Button>
                  ) : (
                    <Button onClick={() => openPopupModel(complain, true)}>
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
            complains={selectedComplains}
            complainType={complainType!}
            ticketType={TicketType.ComplainTicket}
          />
        </ModalBody>
      </Modal>
    </div>
  );
}
