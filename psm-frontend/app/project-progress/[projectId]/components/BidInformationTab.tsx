"use client";

import React, { useState, useEffect } from "react";
import { Project, Tender } from "@/types";

interface BidInformationTabProps {
  projectId: string;
  project: Project;
}

export function BidInformationTab({ project }: BidInformationTabProps) {
  const [tenders, setTenders] = useState<Tender[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // For now, use the tenders from the project object
    // Later you might want to fetch this separately from an API
    setTenders(project.tenders || []);
    setLoading(false);
  }, [project]);

  if (loading) {
    return (
      <div className="flex items-center justify-center h-32">
        <div className="text-gray-600 dark:text-gray-400">
          Loading bid information...
        </div>
      </div>
    );
  }

  return (
    <div>
      <div className="mb-6">
        <h2 className="text-lg font-semibold text-gray-900 dark:text-white">
          Bid Information
        </h2>
        <p className="text-gray-600 dark:text-gray-400 text-sm">
          View all submitted bids and tenders for this project
        </p>
      </div>

      {/* Tender Timeline */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 p-6 mb-6">
        <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
          Tender Timeline
        </h3>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {project.tenderOpeningDate && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Tender Opening Date
              </label>
              <div className="text-gray-900 dark:text-white">
                {new Date(project.tenderOpeningDate).toLocaleDateString()}
              </div>
            </div>
          )}

          {project.tenderClosingDate && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Tender Closing Date
              </label>
              <div className="text-gray-900 dark:text-white">
                {new Date(project.tenderClosingDate).toLocaleDateString()}
              </div>
            </div>
          )}
        </div>
      </div>

      {/* Submitted Bids */}
      {tenders.length === 0 ? (
        <div className="text-center py-12 bg-gray-50 dark:bg-gray-800 rounded-lg">
          <div className="text-gray-500 dark:text-gray-400 mb-4">
            No bids have been submitted for this project yet.
          </div>
        </div>
      ) : (
        <div className="space-y-4">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white">
            Submitted Bids ({tenders.length})
          </h3>

          {tenders.map((tender) => (
            <div
              key={tender.tenderId}
              className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 p-6"
            >
              <div className="flex justify-between items-start mb-4">
                <div className="flex-1">
                  <h4 className="text-lg font-medium text-gray-900 dark:text-white mb-2">
                    {tender.subject}
                  </h4>

                  <div className="grid grid-cols-1 md:grid-cols-3 gap-4 text-sm">
                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">
                        Company:
                      </span>
                      <div className="text-gray-900 dark:text-white">
                        {tender.contractor?.name ||
                          `Company ID: ${tender.contractorId}`}
                      </div>
                      {tender.contractor?.address && (
                        <div className="text-gray-600 dark:text-gray-400 text-xs mt-1">
                          {tender.contractor.address}
                        </div>
                      )}
                    </div>

                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">
                        Bid Amount:
                      </span>
                      <div className="text-gray-900 dark:text-white font-semibold text-lg">
                        ${tender.bidAmount.toLocaleString()}
                      </div>
                    </div>

                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">
                        Submitted Date:
                      </span>
                      <div className="text-gray-900 dark:text-white">
                        {new Date(tender.submittedDate).toLocaleDateString()}
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              {tender.note && (
                <div className="mt-4">
                  <span className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                    Notes:
                  </span>
                  <div className="text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-700 p-3 rounded-md text-sm">
                    {tender.note}
                  </div>
                </div>
              )}

              {tender.tenderDocument && (
                <div className="mt-4">
                  <span className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                    Tender Document:
                  </span>
                  <div className="flex items-center p-3 bg-gray-50 dark:bg-gray-700 rounded-md">
                    <svg
                      className="w-5 h-5 text-gray-400 mr-2"
                      fill="none"
                      stroke="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
                      />
                    </svg>
                    <span className="text-gray-900 dark:text-white text-sm">
                      {typeof tender.tenderDocument === "string"
                        ? tender.tenderDocument
                        : "Tender document attached"}
                    </span>
                  </div>
                </div>
              )}
            </div>
          ))}

          {/* Bid Summary */}
          <div className="bg-blue-50 dark:bg-blue-900/20 rounded-lg p-6 mt-6">
            <h4 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
              Bid Summary
            </h4>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div className="text-center">
                <div className="text-2xl font-bold text-blue-600 dark:text-blue-400">
                  {tenders.length}
                </div>
                <div className="text-sm text-gray-600 dark:text-gray-400">
                  Total Bids
                </div>
              </div>

              <div className="text-center">
                <div className="text-2xl font-bold text-green-600 dark:text-green-400">
                  $
                  {Math.min(
                    ...tenders.map((t) => t.bidAmount)
                  ).toLocaleString()}
                </div>
                <div className="text-sm text-gray-600 dark:text-gray-400">
                  Lowest Bid
                </div>
              </div>

              <div className="text-center">
                <div className="text-2xl font-bold text-orange-600 dark:text-orange-400">
                  $
                  {Math.max(
                    ...tenders.map((t) => t.bidAmount)
                  ).toLocaleString()}
                </div>
                <div className="text-sm text-gray-600 dark:text-gray-400">
                  Highest Bid
                </div>
              </div>
            </div>

            <div className="mt-4 text-center">
              <div className="text-lg font-medium text-gray-900 dark:text-white">
                Average Bid: $
                {Math.round(
                  tenders.reduce((sum, t) => sum + t.bidAmount, 0) /
                    tenders.length
                ).toLocaleString()}
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
