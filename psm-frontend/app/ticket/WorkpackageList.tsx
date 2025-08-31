import { getWorkpackagePaging } from "@/app/api/actions/workpackageAction";
import { TicketWorkpackageType, ComplainStatus } from "@/enums";
import { Paging, Complain } from "@/types";
import {
  Button,
  ButtonGroup,
  Checkbox,
  Pagination,
  Spinner,
} from "flowbite-react";
import { useEffect, useState } from "react";

const itemsPerPage = 8;

export default function WorkpackageList({
  selectedWorkpackages,
  handleWorkpackageClick,
  ticketWorkpackageType,
}: {
  selectedWorkpackages: Complain[];
  handleWorkpackageClick: (complain: Complain, isChecked: boolean) => void;
  ticketWorkpackageType: TicketWorkpackageType;
}) {
  const [data, setData] = useState<Paging<Complain>>({
    records: [],
    totalItems: 0,
  });

  const [pageIndex, setPageIndex] = useState<number>(1);

  const [statusList, setStatusList] = useState<ComplainStatus[]>([
    ComplainStatus.New,
    ComplainStatus.InProgress,
  ]);

  const [loading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    fetchData();
  }, [pageIndex]);

  useEffect(() => {
    if (pageIndex !== 1) {
      setPageIndex(1);
    } else {
      fetchData();
    }
  }, [statusList]);

  const fetchData = async () => {
    try {
      setLoading(true);

      console.log(45666, ticketWorkpackageType);

      const result = await getWorkpackagePaging({
        status: statusList.join(","),
        pageSize: itemsPerPage.toString(),
        pageIndex: pageIndex.toString(),
        duration: "60",
        ticketWorkpackageType: TicketWorkpackageType[ticketWorkpackageType],
      });

      setData(result);
    } catch (error) {
      console.error("Error fetching complain data:", error);
    } finally {
      setLoading(false);
    }
  };

  function statusChange(status: ComplainStatus) {
    let list = [];
    if (statusList.includes(status)) {
      list = statusList.filter((sta) => sta !== status);
    } else {
      list = [...statusList, status];
    }
    setStatusList(list);
  }

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  return (
    <div>
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
                statusList.includes(ComplainStatus.Close) ? "bg-green-700" : ""
              } `}
              onClick={() => statusChange(ComplainStatus.Close)}
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
              </tr>
            </thead>
            <tbody>
              {data &&
                data.records.map((complain) => (
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
                      {new Date(complain?.createdAt).toISOString().slice(0, 10)}
                    </td>
                    <td className="px-6 py-4">
                      {ComplainStatus[complain.status]}
                    </td>
                    <td className="px-6 py-4">
                      <Checkbox
                        id="accept"
                        onChange={(e) =>
                          handleWorkpackageClick(complain, e.target.checked)
                        }
                        checked={selectedWorkpackages
                          .map((x) => x.complainId)
                          .includes(complain.complainId)}
                      />
                    </td>
                  </tr>
                ))}
            </tbody>
          </table>
        </div>

        <div className="flex justify-center mt-5 ">
          {data?.records?.length > 0 ? (
            <div>
              <Pagination
                currentPage={pageIndex}
                totalPages={Math.ceil(data.totalItems / itemsPerPage)}
                onPageChange={(page) => {
                  setPageIndex(page);
                }}
              />
            </div>
          ) : null}
        </div>
      </div>
    </div>
  );
}
