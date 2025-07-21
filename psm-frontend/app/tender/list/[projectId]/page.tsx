"use client";
import { getTendersByProjectIdId } from "@/app/api/actions/tenderActions";
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
import { useEffect, useState } from "react";

export default function TenderList() {
  const { projectId } = useParams<{ projectId: string }>();
  const [tenders, setTenders] = useState<Tender[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    fetchTendersByProjectId();
  }, []);

  async function fetchTendersByProjectId() {
    setIsLoading(true);
    try {
      const tenderList = await getTendersByProjectIdId(projectId);
      setTenders(tenderList);

      console.log("Tenders fetched:", tenderList);
    } catch (error) {
      console.error("Error fetching tenders:", error);
      // Handle error appropriately, e.g., show a notification
    } finally {
      setIsLoading(false);
    }
  }

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  return (
    <div className="container mx-auto p-4">
      <div className="mb-5">
        {tenders.length > 0 ? tenders[1]?.project?.name : ""}
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
              key={tender?.id}
              className="bg-white dark:border-gray-700 dark:bg-gray-800"
            >
              <TableCell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                {tender.name}
              </TableCell>
              <TableCell>{tender.company?.name}</TableCell>
              <TableCell>{tender.bidAmount}</TableCell>
              <TableCell>
                {new Date(tender.submittedDate ?? "")
                  .toISOString()
                  .slice(0, 10)}
              </TableCell>

              <TableCell>
                <Link
                  href={`/tender/update/${projectId}/${tender.id}`}
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
  );
}
