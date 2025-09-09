"use client";
import {
  getTendersByProjectIdId,
  deleteTender,
} from "@/app/api/client/tenderActions";
import { Tender } from "@/types";
import {
  Button,
  Modal,
  Spinner,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeadCell,
  TableRow,
} from "flowbite-react";
import { useEffect, useState, useCallback } from "react";
import TenderForm from "./TenderForm";
import toast from "react-hot-toast";
import { HiPencilAlt, HiTrash } from "react-icons/hi";

interface TenderListProps {
  projectId: string;
  initialData?: Tender[];
  onDataChange?: () => void;
}

export default function TenderListComponent({
  projectId,
  initialData,
  onDataChange,
}: TenderListProps) {
  const [tenders, setTenders] = useState<Tender[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingTenderId, setEditingTenderId] = useState<string | undefined>(
    undefined
  );
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [tenderToDelete, setTenderToDelete] = useState<string | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);

  // Helper function to check if tender submission is still open
  const isTenderSubmissionOpen = () => {
    if (!tenders.length) return true; // Allow creation if no tenders exist

    const project = tenders[0]?.project;
    if (!project?.tenderClosingDate) return true; // Allow if no closing date is set

    const closingDate = new Date(project.tenderClosingDate);
    const now = new Date();

    // Compare dates only, not time
    const closingDateOnly = new Date(
      closingDate.getFullYear(),
      closingDate.getMonth(),
      closingDate.getDate()
    );
    const nowDateOnly = new Date(
      now.getFullYear(),
      now.getMonth(),
      now.getDate()
    );

    return closingDateOnly >= nowDateOnly; // Allow if closing date is today or in the future
  };

  const handleOpenModal = (tenderId?: string) => {
    setEditingTenderId(tenderId);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingTenderId(undefined);
  };

  const handleTenderSuccess = () => {
    // Refresh the tender list
    if (onDataChange) {
      onDataChange();
    } else {
      fetchTendersByProjectId();
    }
    handleCloseModal();
  };

  const handleDeleteClick = (tenderId: string) => {
    setTenderToDelete(tenderId);
    setShowDeleteModal(true);
  };

  const handleDeleteCancel = () => {
    setShowDeleteModal(false);
    setTenderToDelete(null);
  };

  const handleDeleteConfirm = async () => {
    if (!tenderToDelete) return;

    setIsDeleting(true);
    try {
      const result = await deleteTender(tenderToDelete);

      if (result.isSuccess) {
        toast.success("Tender deleted successfully");
        // Refresh the tender list
        if (onDataChange) {
          onDataChange();
        } else {
          fetchTendersByProjectId();
        }
      } else {
        toast.error(result.message || "Failed to delete tender");
      }
    } catch (error) {
      console.error("Error deleting tender:", error);
      toast.error("An error occurred while deleting the tender");
    } finally {
      setIsDeleting(false);
      setShowDeleteModal(false);
      setTenderToDelete(null);
    }
  };

  const fetchTendersByProjectId = useCallback(async () => {
    if (!projectId) return;

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
    if (initialData) {
      // Use provided initial data
      setTenders(initialData);
      setIsLoading(false);
    } else {
      // Fetch data if no initial data provided
      fetchTendersByProjectId();
    }
  }, [initialData, fetchTendersByProjectId]);

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-8">
        <Spinner aria-label="Loading tenders" size="lg" />
      </div>
    );
  }

  return (
    <div className="w-full">
      {tenders.length === 0 ? (
        <div className="flex flex-col gap-5 justify-center items-center py-8">
          <div className="text-gray-500 dark:text-gray-400">
            No tenders found for this project
          </div>
          {isTenderSubmissionOpen() ? (
            <Button type="button" onClick={() => handleOpenModal()}>
              Create New Tender
            </Button>
          ) : (
            <div className="text-red-500 dark:text-red-400 text-sm">
              Tender submission period has closed
            </div>
          )}
        </div>
      ) : (
        <div className="w-full">
          <div className="mb-4 flex justify-between items-center">
            <div className="text-lg font-medium text-gray-900 dark:text-white">
              {tenders.length} Tender{tenders.length !== 1 ? "s" : ""} Found
            </div>
            {isTenderSubmissionOpen() ? (
              <Button type="button" size="sm" onClick={() => handleOpenModal()}>
                Create New Tender
              </Button>
            ) : (
              <div className="text-red-500 dark:text-red-400 text-sm">
                Tender submission period has closed
              </div>
            )}
          </div>

          <div className="overflow-x-auto">
            <Table>
              <TableHead>
                <TableRow>
                  <TableHeadCell>Subject</TableHeadCell>
                  <TableHeadCell>Company</TableHeadCell>
                  <TableHeadCell>Bid Amount</TableHeadCell>
                  <TableHeadCell>Submitted Date</TableHeadCell>
                  <TableHeadCell>Status</TableHeadCell>
                  <TableHeadCell>
                    <span className="sr-only">Actions</span>
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
                    <TableCell className="text-gray-700 dark:text-gray-300">
                      {tender.contractor?.name || "N/A"}
                    </TableCell>
                    <TableCell className="text-gray-700 dark:text-gray-300">
                      ${tender.bidAmount?.toLocaleString() || "N/A"}
                    </TableCell>
                    <TableCell className="text-gray-700 dark:text-gray-300">
                      {tender.submittedDate
                        ? new Date(tender.submittedDate).toLocaleDateString()
                        : "N/A"}
                    </TableCell>
                    <TableCell>
                      <span className="bg-blue-100 text-blue-800 text-xs font-medium px-2.5 py-0.5 rounded dark:bg-blue-900 dark:text-blue-300">
                        Active
                      </span>
                    </TableCell>
                    <TableCell>
                      {isTenderSubmissionOpen() ? (
                        <div className="flex gap-2">
                          <button
                            onClick={() =>
                              handleOpenModal(tender.tenderId?.toString())
                            }
                            className="p-2 font-medium text-blue-600 hover:bg-blue-50 hover:text-blue-700 rounded-lg dark:text-blue-500 dark:hover:bg-blue-900/20"
                            title="Edit tender"
                          >
                            <HiPencilAlt className="w-4 h-4" />
                          </button>
                          <button
                            onClick={() =>
                              handleDeleteClick(
                                tender.tenderId?.toString() || ""
                              )
                            }
                            className="p-2 font-medium text-red-600 hover:bg-red-50 hover:text-red-700 rounded-lg dark:text-red-500 dark:hover:bg-red-900/20"
                            title="Delete tender"
                          >
                            <HiTrash className="w-4 h-4" />
                          </button>
                        </div>
                      ) : (
                        <div className="text-gray-400 dark:text-gray-500 text-sm">
                          Submission closed
                        </div>
                      )}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </div>
      )}

      {/* Tender Form Modal */}
      <Modal show={isModalOpen} onClose={handleCloseModal} size="4xl">
        <div className="p-6">
          <div className="flex items-center justify-between pb-4 mb-4 border-b border-gray-200 dark:border-gray-600">
            <h3 className="text-xl font-semibold text-gray-900 dark:text-white">
              {editingTenderId ? "Edit Tender" : "Create New Tender"}
            </h3>
            <button
              onClick={handleCloseModal}
              className="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5 ml-auto inline-flex items-center dark:hover:bg-gray-600 dark:hover:text-white"
            >
              <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                <path
                  fillRule="evenodd"
                  d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                  clipRule="evenodd"
                />
              </svg>
            </button>
          </div>
          <TenderForm
            tenderId={editingTenderId}
            selectedProjectId={projectId}
            onClose={handleCloseModal}
            onSuccess={handleTenderSuccess}
          />
        </div>
      </Modal>

      {/* Delete Confirmation Modal */}
      <Modal show={showDeleteModal} onClose={handleDeleteCancel} size="md">
        <div className="p-6">
          <div className="flex items-center justify-between pb-4 mb-4 border-b border-gray-200 dark:border-gray-600">
            <h3 className="text-xl font-semibold text-gray-900 dark:text-white">
              Confirm Delete
            </h3>
            <button
              onClick={handleDeleteCancel}
              className="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5 ml-auto inline-flex items-center dark:hover:bg-gray-600 dark:hover:text-white"
            >
              <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                <path
                  fillRule="evenodd"
                  d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                  clipRule="evenodd"
                />
              </svg>
            </button>
          </div>
          <div className="mb-6">
            <p className="text-gray-700 dark:text-gray-300">
              Are you sure you want to delete this tender? This action cannot be
              undone.
            </p>
          </div>
          <div className="flex justify-end gap-3">
            <Button
              color="gray"
              onClick={handleDeleteCancel}
              disabled={isDeleting}
            >
              Cancel
            </Button>
            <Button
              color="failure"
              onClick={handleDeleteConfirm}
              disabled={isDeleting}
            >
              {isDeleting ? (
                <>
                  <Spinner size="sm" className="mr-2" />
                  Deleting...
                </>
              ) : (
                "Delete"
              )}
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
