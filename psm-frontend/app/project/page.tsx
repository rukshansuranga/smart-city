"use client";
import { Project } from "@/types";
import {
  Button,
  Datepicker,
  Pagination,
  Spinner,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeadCell,
  TableRow,
} from "flowbite-react";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import qs from "query-string";
import { useSearchParams } from "next/navigation";
import { filterProjects } from "../api/client/projectActions";
import Link from "next/link";
import { ProjectStatus, ProjectType } from "@/enums";
import toast from "react-hot-toast";
import { LuPencil } from "react-icons/lu";

export type ProjectFilter = {
  pageSize: number;
  pageNumber: number;
  selectedType?: string;
  searchText?: string;
  startDate?: Date | null;
  endDate?: Date | null;
};

export default function ProjectList() {
  const router = useRouter();
  const searchParams = useSearchParams();

  const [projects, setProjects] = useState<Partial<Project>[]>([]);
  const [totalItems, setTotalItems] = useState<number>(0);
  const [totalPages] = useState(0);
  const [currentPage] = useState(1);

  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [filter, setFilter] = useState<ProjectFilter>({
    pageSize: 8,
    pageNumber: 1,
    selectedType: "",
    searchText: "",
    // startDate: new Date(lastYearNow),
    // endDate: new Date(),
    startDate: null,
    endDate: null,
  });

  function generateQuery(queryFilter: ProjectFilter) {
    const formatFilter = {
      ...queryFilter,
      startDate: filter.startDate?.toISOString().slice(0, 10),
      endDate: filter.endDate?.toISOString().slice(0, 10),
    };

    const url = qs.stringifyUrl(
      { url: "", query: formatFilter },
      { skipEmptyString: true }
    );
    return url;
  }

  function handleSearch() {
    setFilter({ ...filter, pageNumber: 1 });
    const url = generateQuery({ ...filter, pageNumber: 1 });
    console.log("url", url);
    router.push(url);
  }

  const pageIndex = parseInt(searchParams.get("pageIndex")!) || 1;

  useEffect(() => {
    console.log("searchParams", searchParams.toString());
    fetchProjects();

    setFilter({ ...filter, pageNumber: pageIndex });
  }, [searchParams]);

  async function fetchProjects() {
    setIsLoading(true);
    try {
      const url = generateQuery(filter);
      const response = await filterProjects(url);

      if (response.isSuccess) {
        setTotalItems(response.data?.totalItems);
        setProjects(response.data?.records);
      } else {
        setTotalItems(0);
        setProjects([]);
        toast.error(response.message || "Failed to fetch projects");
      }
    } catch (error) {
      console.error("Error fetching projects:", error);
      // Optionally, you can set an error state here to display an error message in the UI
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

  console.log("projects", totalItems);

  return (
    <div className="flex flex-col gap-2">
      <div className="flex justify-between items-center">
        <div className="flex justify-start items-center gap-2">
          <div className="w-2/12">
            <label
              htmlFor="types"
              className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
            >
              Project type
            </label>
            <select
              value={filter.selectedType}
              id="types"
              onChange={(e) =>
                setFilter({ ...filter, selectedType: e.target.value })
              }
              className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
            >
              {Object.keys(ProjectType)
                .filter((key) => !isNaN(Number(key)))
                .map((key) => (
                  <option value={key} key={key}>
                    {ProjectType[key as keyof typeof ProjectType]}
                  </option>
                ))}
            </select>
          </div>
          <div className="w-4/12">
            <label
              htmlFor="search_text"
              className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
            >
              Search Text
            </label>
            <input
              type="text"
              id="search_text"
              onChange={(e) =>
                setFilter({ ...filter, searchText: e.target.value })
              }
              value={filter.searchText}
              className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              placeholder="Search..."
            />
          </div>
          <div className="w-2/12">
            <label className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">
              Start Date
            </label>
            <Datepicker
              label="Start Date"
              value={filter.startDate}
              onChange={(date) => setFilter({ ...filter, startDate: date })}
            />
          </div>
          <div className="w-2/12">
            <label className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">
              End Date
            </label>
            <Datepicker
              label="End Date"
              value={filter.endDate}
              onChange={(date) => setFilter({ ...filter, endDate: date })}
            />
          </div>
          <div className="self-end w-2/12">
            <Button onClick={handleSearch}>Search Project</Button>
          </div>
        </div>
        <div className="self-end">
          <Button onClick={() => router.push("/project/new")}>
            Add Project
          </Button>
        </div>
      </div>
      {projects.length === 0 ? (
        <div className="flex justify-center items-center">
          <p className="text-gray-500">No projects found.</p>
        </div>
      ) : (
        <>
          <div className="mt-5">
            <Table>
              <TableHead>
                <TableRow>
                  <TableHeadCell>Name</TableHeadCell>
                  <TableHeadCell>Type</TableHeadCell>
                  <TableHeadCell>Location</TableHeadCell>
                  <TableHeadCell>Status</TableHeadCell>
                  <TableHeadCell>
                    <span className="sr-only">Edit</span>
                  </TableHeadCell>
                </TableRow>
              </TableHead>
              <TableBody className="divide-y">
                {projects?.map((project) => (
                  <TableRow
                    key={project.id}
                    className="bg-white dark:border-gray-700 dark:bg-gray-800"
                  >
                    <TableCell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                      {project.subject}
                    </TableCell>
                    <TableCell>
                      {ProjectType[project.type as ProjectType]}
                    </TableCell>
                    <TableCell>{project.location}</TableCell>
                    <TableCell>
                      {ProjectStatus[project.status as ProjectStatus]}
                    </TableCell>

                    <TableCell>
                      <Link
                        href={`/project/update/${project.id}`}
                        className="inline-flex items-center justify-center p-2 text-sm font-medium text-white bg-blue-600 rounded-lg hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                      >
                        <LuPencil className="w-4 h-4" />
                      </Link>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
          <div>
            <Pagination
              currentPage={currentPage || 1}
              totalPages={totalPages || 1}
              onPageChange={(page) => {
                setFilter({ ...filter, pageNumber: page });
                const url = generateQuery({ ...filter, pageNumber: page });
                router.push(url);
              }}
            />
          </div>
        </>
      )}
    </div>
  );
}
