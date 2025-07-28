"use client";
import { Pagination } from "flowbite-react";
import { useSearchParams, useRouter } from "next/navigation";

export default function useTicketPaging({
  totalCount,
}: {
  totalCount: number;
}) {
  const searchParams = useSearchParams();
  const router = useRouter();
  const pageIndex = searchParams.get("pageIndex");

  const itemsPerPage = 5;
  const totalPages = Math.ceil(totalCount / itemsPerPage);

  console.log("paging", pageIndex, totalPages, totalCount);

  return (
    <div>
      <Pagination
        currentPage={parseInt(pageIndex || "1")}
        totalPages={totalPages}
        onPageChange={(page) => {
          router.push(`?pageSize=${itemsPerPage}&pageIndex=${page}`);
        }}
      />
    </div>
  );
}
