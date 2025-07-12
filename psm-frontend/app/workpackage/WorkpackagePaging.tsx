"use client";
import { Pagination } from "flowbite-react";
import { useRouter } from "next/navigation";

export default function WorkpackagPaging({
  totalCount,
  itemsPerPage,
  pageIndex,
}: {
  totalCount: number;
  itemsPerPage: number;
  pageIndex: number;
}) {
  const router = useRouter();

  const totalPages = Math.ceil(totalCount / itemsPerPage);

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
