"use client";
import { getTendersByProjectIdId } from "@/app/api/client/tenderActions";
import { Tender } from "@/types";
import {
  Button,
  Spinner,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeadCell,
  TableRow,
} from "flowbite-react";
import Link from "next/link";
import { useParams } from "next/navigation";
import { useEffect, useState, useCallback } from "react";
import toast from "react-hot-toast";

export default function TenderList() {
  const { projectId } = useParams<{ projectId: string }>();
  const [tenders, setTenders] = useState<Tender[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const fetchTendersByProjectId = useCallback(async () => {
    setIsLoading(true);
    try {
      const result = await getTendersByProjectIdId(projectId);

      if (result.isSuccess && result.data) {
        setTenders(result.data);
        console.log("Tenders fetched:", result.data);
      } else {
        console.error("Failed to fetch tenders:", result.message);
        toast.error(result.message || "Failed to load tenders");
        setTenders([]);
      }
    } catch (error) {
      console.error("Error fetching tenders:", error);
      toast.error("An error occurred while loading tenders");
      setTenders([]);
    } finally {
      setIsLoading(false);
    }
  }, [projectId]);

  useEffect(() => {
    if (projectId) {
      fetchTendersByProjectId();
    }
  }, [projectId, fetchTendersByProjectId]);

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  return (
    <>
      {tenders.length === 0 ? (
        <div className="flex flex-col gap-5 justify-center items-center">
          <div>No tenders found</div>
          <Button type="button">
            <Link href={`/tender/new/${projectId}`}>Create Tender</Link>
          </Button>
        </div>
      ) : (
        <div className="container mx-auto p-4">
          <div className="mb-5">
            {tenders.length > 0 ? tenders[1]?.project?.subject : ""}
          </div>
          <div className="my-4 flex justify-end">
            <Button type="button">
              <Link href={`/tender/new/${projectId}`}>Create Tender</Link>
            </Button>
          </div>
          <Table>
            <TableHead>
              <TableRow>
                <TableHeadCell>Name</TableHeadCell>
                <TableHeadCell>Company</TableHeadCell>
                <TableHeadCell>Bid Amount</TableHeadCell>
                <TableHeadCell>Submitted Date</TableHeadCell>
                <TableHeadCell>
                  <span className="sr-only">Edit</span>
                </TableHeadCell>
              </TableRow>
            </TableHead>
            <TableBody className="divide-y">
              {tenders?.map((tender) => (
                <TableRow
                  key={tender?.tenderId}
                  className="bg-white dark:border-gray-700 dark:bg-gray-800"
                >
                  <TableCell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                    {tender.subject}
                  </TableCell>
                  <TableCell>{tender.contractor?.name}</TableCell>
                  <TableCell>{tender.bidAmount}</TableCell>
                  <TableCell>
                    {new Date(tender.submittedDate ?? "")
                      .toISOString()
                      .slice(0, 10)}
                  </TableCell>

                  <TableCell>
                    <Link
                      href={`/tender/update/${projectId}/${tender.tenderId}`}
                      className="font-medium text-primary-600 hover:underline dark:text-primary-500 ml-2"
                    >
                      Edit
                    </Link>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </div>
      )}
    </>
  );
}
