import { getWorkpackagePaging } from "@/app/api/actions/workpackageAction";
import { Paging, Workpackage } from "@/types";
import { Button, ButtonGroup, Checkbox, Pagination } from "flowbite-react";
import { useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";

const itemsPerPage = 2;

export default function WorkpackageList({
  selectedWorkpackages,
  handleWorkpackageClick,
}: {
  selectedWorkpackages: Workpackage[];
  handleWorkpackageClick: (
    workpackage: Workpackage,
    isChecked: boolean
  ) => void;
}) {
  const [data, setData] = useState<Paging<Workpackage>>({
    records: [],
    totalItems: 0,
  });

  const [pageIndex, setPageIndex] = useState(1);

  const [statusList, setStatusList] = useState<string[]>(["Created", "Open"]);

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
      const result = await getWorkpackagePaging({
        status: statusList.join(","),
        pageSize: itemsPerPage,
        pageIndex: pageIndex,
        duration: 14,
      });

      setData(result);
    } catch (error) {
      console.error("Error fetching workpackage data:", error);
    }
  };

  function statusChange(status: string) {
    if (statusList.includes(status)) {
      setStatusList(statusList.filter((sta) => sta !== status));
    } else {
      setStatusList((prev) => [...prev, status]);
    }
  }

  console.log("data", data);

  return (
    <div>
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
              </tr>
            </thead>
            <tbody>
              {data &&
                data.records.map((workpackage) => (
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
                    <td className="px-6 py-4">{`${workpackage.createdDate?.day}-${workpackage.createdDate?.month}-${workpackage.createdDate?.year}`}</td>
                    <td className="px-6 py-4">{workpackage.status}</td>
                    <td className="px-6 py-4">
                      <Checkbox
                        id="accept"
                        onChange={(e) =>
                          handleWorkpackageClick(workpackage, e.target.checked)
                        }
                        checked={selectedWorkpackages
                          .map((x) => x.workPackageId)
                          .includes(workpackage.workPackageId)}
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
                currentPage={parseInt(pageIndex) || 1}
                totalPages={Math.ceil(data.totalItems / itemsPerPage)}
                onPageChange={(page) => {
                  setPageIndex(page);
                }}
              />
            </div>
          ) : null}
        </div>
      </div>
      ;
    </div>
  );
}
