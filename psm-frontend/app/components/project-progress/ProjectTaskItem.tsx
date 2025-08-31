"use client";
import { Badge } from "flowbite-react";
import { HiCheckCircle, HiClock, HiExclamationCircle } from "react-icons/hi";
import { ProjectTask } from "@/types";

interface ProjectTaskItemProps {
  task: ProjectTask;
}

export default function ProjectTaskItem({ task }: ProjectTaskItemProps) {
  const getStatusIcon = (status: string) => {
    switch (status) {
      case "completed":
        return <HiCheckCircle className="w-5 h-5 text-green-500" />;
      case "in-progress":
        return <HiClock className="w-5 h-5 text-blue-500" />;
      case "overdue":
        return <HiExclamationCircle className="w-5 h-5 text-red-500" />;
      default:
        return <HiClock className="w-5 h-5 text-gray-500" />;
    }
  };

  const getStatusBadge = (status: string) => {
    const statusConfig = {
      completed: { color: "success", text: "Completed" },
      "in-progress": { color: "info", text: "In Progress" },
      pending: { color: "gray", text: "Pending" },
      overdue: { color: "failure", text: "Overdue" },
    };

    const config =
      statusConfig[status as keyof typeof statusConfig] || statusConfig.pending;

    return (
      <Badge color={config.color} size="sm">
        {config.text}
      </Badge>
    );
  };

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case "high":
        return "text-red-600 dark:text-red-400";
      case "medium":
        return "text-yellow-600 dark:text-yellow-400";
      default:
        return "text-green-600 dark:text-green-400";
    }
  };

  return (
    <div className="p-6 border-b border-gray-200 dark:border-gray-600 last:border-b-0">
      <div className="flex items-start justify-between">
        <div className="flex items-start space-x-3 flex-1">
          <div className="flex-shrink-0 mt-1">{getStatusIcon(task.status)}</div>
          <div className="flex-1 min-w-0">
            <div className="flex items-center justify-between mb-2">
              <h5 className="text-sm font-medium text-gray-900 dark:text-white">
                {task.title}
              </h5>
              <div className="flex items-center space-x-2">
                <span
                  className={`text-xs font-medium ${getPriorityColor(
                    task.priority
                  )}`}
                >
                  {task.priority.toUpperCase()}
                </span>
                {getStatusBadge(task.status)}
              </div>
            </div>
            <p className="text-sm text-gray-600 dark:text-gray-400 mb-2">
              {task.description}
            </p>
            <div className="flex items-center space-x-4 text-xs text-gray-500 dark:text-gray-400">
              <span>Due: {new Date(task.dueDate).toLocaleDateString()}</span>
              {task.assignee && <span>Assignee: {task.assignee}</span>}
              {task.completedDate && (
                <span className="text-green-600 dark:text-green-400">
                  Completed: {new Date(task.completedDate).toLocaleDateString()}
                </span>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
