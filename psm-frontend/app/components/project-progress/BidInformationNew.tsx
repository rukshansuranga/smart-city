"use client";

import React, { useState, useEffect } from "react";
import { Spinner, Badge, Card, Button } from "flowbite-react";
import {
  HiCurrencyDollar,
  HiCalendar,
  HiOfficeBuilding,
  HiDocument,
} from "react-icons/hi";
import { getTendersByProjectIdId } from "@/app/api/client/tenderActions";
import { Tender } from "@/types";

interface BidInformationProps {
  projectId: string;
}

export default function BidInformation({ projectId }: BidInformationProps) {
  const [tenders, setTenders] = useState<Tender[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadBidData = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const data = await getTendersByProjectIdId(projectId);
        setTenders(data);
      } catch (error) {
        console.error("Error fetching tender data:", error);
        setError("Failed to load bid information");
      } finally {
        setIsLoading(false);
      }
    };

    if (projectId) {
      loadBidData();
    }
  }, [projectId]);

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: "USD",
    }).format(amount);
  };

  const formatDate = (date: Date) => {
    return new Date(date).toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  };

  const getLowestBid = () => {
    if (tenders.length === 0) return null;
    return Math.min(...tenders.map((t) => t.bidAmount));
  };

  const getHighestBid = () => {
    if (tenders.length === 0) return null;
    return Math.max(...tenders.map((t) => t.bidAmount));
  };

  const getAverageBid = () => {
    if (tenders.length === 0) return null;
    const total = tenders.reduce((sum, t) => sum + t.bidAmount, 0);
    return total / tenders.length;
  };

  const sortedTenders = [...tenders].sort((a, b) => a.bidAmount - b.bidAmount);

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-8">
        <Spinner aria-label="Loading bid information" size="lg" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center py-8">
        <p className="text-red-500 dark:text-red-400">{error}</p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Bid Statistics */}
      <div className="grid grid-cols-1 sm:grid-cols-4 gap-4">
        <Card>
          <div className="text-center">
            <p className="text-2xl font-bold text-blue-600 dark:text-blue-400">
              {tenders.length}
            </p>
            <p className="text-sm text-gray-500 dark:text-gray-400">
              Total Bids
            </p>
          </div>
        </Card>

        <Card>
          <div className="text-center">
            <p className="text-lg font-bold text-green-600 dark:text-green-400">
              {getLowestBid() ? formatCurrency(getLowestBid()!) : "N/A"}
            </p>
            <p className="text-sm text-gray-500 dark:text-gray-400">
              Lowest Bid
            </p>
          </div>
        </Card>

        <Card>
          <div className="text-center">
            <p className="text-lg font-bold text-red-600 dark:text-red-400">
              {getHighestBid() ? formatCurrency(getHighestBid()!) : "N/A"}
            </p>
            <p className="text-sm text-gray-500 dark:text-gray-400">
              Highest Bid
            </p>
          </div>
        </Card>

        <Card>
          <div className="text-center">
            <p className="text-lg font-bold text-yellow-600 dark:text-yellow-400">
              {getAverageBid() ? formatCurrency(getAverageBid()!) : "N/A"}
            </p>
            <p className="text-sm text-gray-500 dark:text-gray-400">
              Average Bid
            </p>
          </div>
        </Card>
      </div>

      {/* Bid Table */}
      <Card>
        <div className="p-6 border-b border-gray-200 dark:border-gray-600">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white flex items-center">
            <HiCurrencyDollar className="w-5 h-5 mr-2 text-green-500" />
            Submitted Bids
          </h3>
          <p className="text-sm text-gray-600 dark:text-gray-400 mt-1">
            All tender submissions for this project, sorted by bid amount
          </p>
        </div>

        {tenders.length > 0 ? (
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-600">
              <thead className="bg-gray-50 dark:bg-gray-700">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                    Rank
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                    Company
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                    Bid Amount
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                    Submitted Date
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                    Subject
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                    Document
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-600">
                {sortedTenders.map((tender, index) => (
                  <tr
                    key={tender.tenderId}
                    className={`${
                      index === 0 ? "bg-green-50 dark:bg-green-900/20" : ""
                    } hover:bg-gray-50 dark:hover:bg-gray-700`}
                  >
                    <td className="px-6 py-4 whitespace-nowrap font-medium text-gray-900 dark:text-white">
                      <div className="flex items-center">
                        {index === 0 && (
                          <Badge color="success" size="sm" className="mr-2">
                            Lowest
                          </Badge>
                        )}
                        #{index + 1}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <HiOfficeBuilding className="w-4 h-4 mr-2 text-gray-400" />
                        <div>
                          <div className="font-medium text-gray-900 dark:text-white">
                            {tender.contractor?.name || "Unknown Company"}
                          </div>
                          {tender.contractor?.address && (
                            <div className="text-sm text-gray-500 dark:text-gray-400">
                              {tender.contractor.address}
                            </div>
                          )}
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-lg font-semibold text-gray-900 dark:text-white">
                        {formatCurrency(tender.bidAmount)}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center text-gray-500 dark:text-gray-400">
                        <HiCalendar className="w-4 h-4 mr-1" />
                        {formatDate(tender.submittedDate)}
                      </div>
                    </td>
                    <td className="px-6 py-4">
                      <div className="max-w-xs">
                        <div className="font-medium text-gray-900 dark:text-white">
                          {tender.subject}
                        </div>
                        {tender.note && (
                          <div className="text-sm text-gray-500 dark:text-gray-400 truncate">
                            {tender.note}
                          </div>
                        )}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      {tender.tenderDocument ? (
                        <div className="flex items-center">
                          <HiDocument className="w-4 h-4 mr-1 text-blue-500" />
                          <a
                            href={
                              typeof tender.tenderDocument === "string"
                                ? tender.tenderDocument
                                : "#"
                            }
                            className="text-blue-600 dark:text-blue-400 hover:underline text-sm"
                            target="_blank"
                            rel="noopener noreferrer"
                          >
                            View
                          </a>
                        </div>
                      ) : (
                        <span className="text-gray-400 text-sm">
                          No document
                        </span>
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex space-x-2">
                        <Button size="xs" color="light">
                          Details
                        </Button>
                        {index === 0 && (
                          <Button size="xs" color="success">
                            Accept
                          </Button>
                        )}
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : (
          <div className="p-8 text-center">
            <div className="text-gray-400 dark:text-gray-500 mb-2">
              <HiCurrencyDollar className="mx-auto h-12 w-12" />
            </div>
            <h3 className="text-sm font-medium text-gray-900 dark:text-white mb-1">
              No Bids Submitted
            </h3>
            <p className="text-sm text-gray-500 dark:text-gray-400">
              No tender submissions have been received for this project yet.
            </p>
          </div>
        )}
      </Card>

      {/* Bid Analysis */}
      {tenders.length > 1 && (
        <Card>
          <div className="p-6">
            <h4 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">
              Bid Analysis
            </h4>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <h5 className="font-medium text-gray-700 dark:text-gray-300 mb-2">
                  Price Distribution
                </h5>
                <div className="space-y-2">
                  <div className="flex justify-between text-sm">
                    <span>Lowest:</span>
                    <span className="font-medium text-green-600">
                      {formatCurrency(getLowestBid()!)}
                    </span>
                  </div>
                  <div className="flex justify-between text-sm">
                    <span>Average:</span>
                    <span className="font-medium">
                      {formatCurrency(getAverageBid()!)}
                    </span>
                  </div>
                  <div className="flex justify-between text-sm">
                    <span>Highest:</span>
                    <span className="font-medium text-red-600">
                      {formatCurrency(getHighestBid()!)}
                    </span>
                  </div>
                  <div className="flex justify-between text-sm border-t pt-2">
                    <span>Price Range:</span>
                    <span className="font-medium">
                      {formatCurrency(getHighestBid()! - getLowestBid()!)}
                    </span>
                  </div>
                </div>
              </div>
              <div>
                <h5 className="font-medium text-gray-700 dark:text-gray-300 mb-2">
                  Competitive Analysis
                </h5>
                <div className="space-y-2 text-sm">
                  <p>
                    <strong>Competition Level:</strong>{" "}
                    {tenders.length >= 5
                      ? "High"
                      : tenders.length >= 3
                      ? "Medium"
                      : "Low"}
                  </p>
                  <p>
                    <strong>Price Variance:</strong>{" "}
                    {(
                      ((getHighestBid()! - getLowestBid()!) / getLowestBid()!) *
                      100
                    ).toFixed(1)}
                    %
                  </p>
                  <p className="text-gray-600 dark:text-gray-400">
                    {tenders.length >= 5
                      ? "Strong competition with multiple competitive bids."
                      : tenders.length >= 3
                      ? "Moderate competition with reasonable bid variety."
                      : "Limited competition. Consider extending tender period."}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </Card>
      )}
    </div>
  );
}
