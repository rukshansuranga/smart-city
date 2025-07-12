"use client";
import { Pagination } from "flowbite-react";
import { useSearchParams, useRouter } from "next/navigation";

export default function getTicketPaging({
  totalCount,
}: {
  totalCount: string;
}) {
  const searchParams = useSearchParams();
  const router = useRouter();
  const pageIndex = parseInt(searchParams.get("pageIndex"), 10) || 1;

  const itemsPerPage = 5;
  const totalPages = Math.ceil(parseInt(totalCount) / itemsPerPage);

  console.log("paging", pageIndex, totalPages, totalCount);

  return (
    <div>
      <Pagination
        currentPage={pageIndex || 1}
        totalPages={totalPages}
        onPageChange={(page) => {
          router.push(`?pageSize=${itemsPerPage}&pageIndex=${page}`);
        }}
      />
    </div>
  );
}
