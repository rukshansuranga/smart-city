"use client";
import { Card } from "flowbite-react";
import { Bar, Doughnut } from "react-chartjs-2";
import { PageProtection } from "../components/PageProtection";

import {
  Chart as ChartJS,
  ArcElement,
  Tooltip,
  Legend,
  Title,
  CategoryScale,
  LinearScale,
  BarElement,
  ChartOptions,
} from "chart.js";
import type { Context } from "chartjs-plugin-datalabels";
import ChartDataLabels from "chartjs-plugin-datalabels";

ChartJS.register(
  ArcElement,
  CategoryScale,
  LinearScale,
  BarElement,
  Tooltip,
  Legend,
  Title,
  ChartDataLabels
);

function DashboardContent() {
  const overviewData = {
    labels: ["LightPost", "Garbage", "Project", "General"],
    datasets: [
      {
        label: "Complaints",
        data: [300, 50, 100, 50],
        backgroundColor: [
          "rgb(255, 99, 132)",
          "rgb(54, 162, 235)",
          "rgb(255, 205, 86)",
          "rgb(75, 192, 192)",
        ],
        hoverOffset: 4,
      },
    ],
  };

  const lightpostData = {
    labels: ["New", "OnGoing", "Close"],
    datasets: [
      {
        label: "Complaints",
        data: [300, 50, 100],
        backgroundColor: [
          "rgb(255, 99, 132)",
          "rgb(54, 162, 235)",
          "rgb(255, 205, 86)",
        ],
        hoverOffset: 4,
      },
    ],
  };

  const generalData = {
    labels: ["New", "OnGoing", "Close"],
    datasets: [
      {
        label: "Complaints",
        data: [25, 100, 75],
        backgroundColor: [
          "rgb(255, 99, 132)",
          "rgb(54, 162, 235)",
          "rgb(255, 205, 86)",
        ],
        hoverOffset: 4,
      },
    ],
  };

  const projectData = {
    labels: ["New", "OnGoing", "Close"],
    datasets: [
      {
        label: "Complaints",
        data: [25, 100, 75],
        backgroundColor: [
          "rgb(255, 99, 132)",
          "rgb(54, 162, 235)",
          "rgb(255, 205, 86)",
        ],
        hoverOffset: 4,
      },
    ],
  };

  const garbageData = {
    labels: ["New", "OnGoing", "Close"],
    datasets: [
      {
        label: "Complaints",
        data: [25, 100, 75],
        backgroundColor: [
          "rgb(255, 99, 132)",
          "rgb(54, 162, 235)",
          "rgb(255, 205, 86)",
        ],
        hoverOffset: 4,
      },
    ],
  };

  const optionOverview: ChartOptions<"doughnut"> = {
    responsive: true,
    plugins: {
      legend: {
        display: false,
        position: "top",
      },
      title: {
        display: true,
        text: "Complaints Overview, last 7 days", // The title text
        font: {
          size: 16, // Optional: customize font size
        },
        color: "#333", // Optional: customize font color
      },
      datalabels: {
        color: "#000", // Set the color of the labels to black
        font: {
          weight: "bold",
          size: 14,
        },
        formatter: (value: unknown, context: Context) => {
          const labels = context.chart.data.labels;
          const datasets = context.chart.data.datasets;
          const label =
            labels && labels[context.dataIndex]
              ? labels[context.dataIndex]
              : "";
          const dataset = datasets && datasets[0] ? datasets[0] : { data: [] };
          // Filter out non-number values
          const dataArr = Array.isArray(dataset.data)
            ? dataset.data.filter((v): v is number => typeof v === "number")
            : [];
          const total = dataArr.reduce((sum, val) => sum + val, 0);
          const val =
            typeof dataset.data[context.dataIndex] === "number"
              ? (dataset.data[context.dataIndex] as number)
              : 0;
          const percent = total ? ((val / total) * 100).toFixed(1) : 0;
          return `${label}: ${percent}%`;
        },
      },
    },
  };

  const optionLightPostOptions: ChartOptions<"doughnut"> = {
    responsive: true,
    plugins: {
      legend: {
        display: false,
        position: "top",
      },
      title: {
        display: true,
        text: "LightPost, last 7 days", // The title text
        font: {
          size: 12, // Optional: customize font size
        },
        color: "#333", // Optional: customize font color
      },
      datalabels: {
        color: "#000", // Set the color of the labels to black
        font: {
          weight: "bold",
          size: 10,
        },
        formatter: (value: unknown, context: Context) => {
          const labels = context.chart.data.labels;
          const datasets = context.chart.data.datasets;
          const label =
            labels && labels[context.dataIndex]
              ? labels[context.dataIndex]
              : "";
          const dataset = datasets && datasets[0] ? datasets[0] : { data: [] };
          const dataArr = Array.isArray(dataset.data)
            ? dataset.data.filter((v): v is number => typeof v === "number")
            : [];
          const total = dataArr.reduce((sum, val) => sum + val, 0);
          const val =
            typeof dataset.data[context.dataIndex] === "number"
              ? (dataset.data[context.dataIndex] as number)
              : 0;
          const percent = total ? ((val / total) * 100).toFixed(1) : 0;
          return `${label}: ${percent}%`;
        },
      },
    },
  };

  const generalOptions: ChartOptions<"doughnut"> = {
    responsive: true,
    plugins: {
      legend: {
        display: false,
        position: "top",
      },
      title: {
        display: true,
        text: "General Complaints, last 7 days", // The title text
        font: {
          size: 12, // Optional: customize font size
        },
        color: "#333", // Optional: customize font color
      },
      datalabels: {
        color: "#000", // Set the color of the labels to black
        font: {
          weight: "bold",
          size: 10,
        },
        formatter: (value: unknown, context: Context) => {
          const labels = context.chart.data.labels;
          const datasets = context.chart.data.datasets;
          const label =
            labels && labels[context.dataIndex]
              ? labels[context.dataIndex]
              : "";
          const dataset = datasets && datasets[0] ? datasets[0] : { data: [] };
          const dataArr = Array.isArray(dataset.data)
            ? dataset.data.filter((v): v is number => typeof v === "number")
            : [];
          const total = dataArr.reduce((sum, val) => sum + val, 0);
          const val =
            typeof dataset.data[context.dataIndex] === "number"
              ? (dataset.data[context.dataIndex] as number)
              : 0;
          const percent = total ? ((val / total) * 100).toFixed(1) : 0;
          return `${label}: ${percent}%`;
        },
      },
    },
  };

  const projectOptions: ChartOptions<"doughnut"> = {
    responsive: true,
    plugins: {
      legend: {
        display: false,
        position: "top",
      },
      title: {
        display: true,
        text: "Project Complaints, last 7 days", // The title text
        font: {
          size: 12, // Optional: customize font size
        },
        color: "#333", // Optional: customize font color
      },
      datalabels: {
        color: "#000", // Set the color of the labels to black
        font: {
          weight: "bold",
          size: 10,
        },
        formatter: (value: unknown, context: Context) => {
          const labels = context.chart.data.labels;
          const datasets = context.chart.data.datasets;
          const label =
            labels && labels[context.dataIndex]
              ? labels[context.dataIndex]
              : "";
          const dataset = datasets && datasets[0] ? datasets[0] : { data: [] };
          const dataArr = Array.isArray(dataset.data)
            ? dataset.data.filter((v): v is number => typeof v === "number")
            : [];
          const total = dataArr.reduce((sum, val) => sum + val, 0);
          const val =
            typeof dataset.data[context.dataIndex] === "number"
              ? (dataset.data[context.dataIndex] as number)
              : 0;
          const percent = total ? ((val / total) * 100).toFixed(1) : 0;
          return `${label}: ${percent}%`;
        },
      },
    },
  };

  const garbageOptions: ChartOptions<"doughnut"> = {
    responsive: true,
    plugins: {
      legend: {
        display: false,
        position: "top",
      },
      title: {
        display: true,
        text: "Garbage Complaints, last 7 days", // The title text
        font: {
          size: 12, // Optional: customize font size
        },
        color: "#333", // Optional: customize font color
      },
      datalabels: {
        color: "#000", // Set the color of the labels to black
        font: {
          weight: "bold",
          size: 10,
        },
        formatter: (value: unknown, context: Context) => {
          const labels = context.chart.data.labels;
          const datasets = context.chart.data.datasets;
          const label =
            labels && labels[context.dataIndex]
              ? labels[context.dataIndex]
              : "";
          const dataset = datasets && datasets[0] ? datasets[0] : { data: [] };
          const dataArr = Array.isArray(dataset.data)
            ? dataset.data.filter((v): v is number => typeof v === "number")
            : [];
          const total = dataArr.reduce((sum, val) => sum + val, 0);
          const val =
            typeof dataset.data[context.dataIndex] === "number"
              ? (dataset.data[context.dataIndex] as number)
              : 0;
          const percent = total ? ((val / total) * 100).toFixed(1) : 0;
          return `${label}: ${percent}%`;
        },
      },
    },
  };

  const projectBarData = {
    labels: [
      "Road A",
      "Road B",
      "Irrigation 1",
      "Irrigation 2",
      "Building 1",
      "Building 2",
    ],
    datasets: [
      {
        label: "My First Dataset",
        data: [65, 59, 80, 81, 56, 55],
        backgroundColor: "rgba(75, 192, 192, 0.6)",
        borderColor: "rgba(75, 192, 192, 1)",
        borderWidth: 1,
      },
    ],
  };

  const projectBarOptions: ChartOptions<"bar"> = {
    indexAxis: "y",
    responsive: true,
    plugins: {
      legend: {
        display: false,
        position: "top",
      },
      title: {
        display: true,
        text: "Project Completion Summary",
      },
    },
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen px-20">
      <div className="flex w-full gap-5">
        <div className="w-1/2">
          <Card>
            <div className="flex gap-10 mx-8 mt-5">
              <div className="w-1/3">
                <div className="flex flex-col justify-between items-center">
                  <h5 className="text-3xl font-extrabold tracking-tight text-blue-600 mb-5">
                    212
                  </h5>
                  <p className="font-normal text-gray-700">NEW</p>
                </div>
              </div>
              <div className="w-1/3">
                <div className="flex flex-col justify-between items-center">
                  <h5 className="text-3xl font-extrabold tracking-tight text-blue-600 mb-5">
                    202
                  </h5>
                  <p className="font-normal text-gray-700">ONGOING</p>
                </div>
              </div>
              <div className="w-1/3 ">
                <div className="flex flex-col justify-between items-center">
                  <h5 className="text-3xl font-extrabold tracking-tight text-blue-600 mb-5">
                    362
                  </h5>
                  <p className="font-normal text-gray-700">CLOSED</p>
                </div>
              </div>
            </div>
            <div className="mx-auto mt-4 text-green-500">
              <p>Weekly Tickets</p>
            </div>
          </Card>
        </div>
        <div className="w-1/2">
          <Card>
            <div className="flex gap-10 mx-8 mt-5">
              <div className="w-1/3">
                <div className="flex flex-col justify-between items-center">
                  <h5 className="text-3xl font-extrabold tracking-tight text-blue-600 mb-5">
                    212
                  </h5>
                  <p className="font-normal text-gray-700">NEW</p>
                </div>
              </div>
              <div className="w-1/3">
                <div className="flex flex-col justify-between items-center">
                  <h5 className="text-3xl font-extrabold tracking-tight text-blue-600 mb-5">
                    202
                  </h5>
                  <p className="font-normal text-gray-700">ONGOING</p>
                </div>
              </div>
              <div className="w-1/3">
                <div className="flex flex-col justify-between items-center">
                  <h5 className="text-3xl font-extrabold tracking-tight text-blue-600 mb-5">
                    362
                  </h5>
                  <p className="font-normal text-gray-700">CLOSED</p>
                </div>
              </div>
            </div>
            <div className="mx-auto mt-4 text-green-500">
              <p>Weekly Tickets</p>
            </div>
          </Card>
        </div>
      </div>
      <div className="w-full mt-10">
        <Card className="justify-center items-center">
          <div className="mx-40">
            <Doughnut data={overviewData} options={optionOverview} />
          </div>
        </Card>
      </div>

      <Card className="w-full mt-10">
        <div className="flex w-full justify-center items-center">
          <div className="w-1/4">
            <Doughnut data={lightpostData} options={optionLightPostOptions} />
          </div>
          <div className="w-1/4">
            <Doughnut data={generalData} options={generalOptions} />
          </div>
          <div className="w-1/4">
            <Doughnut data={projectData} options={projectOptions} />
          </div>
          <div className="w-1/4">
            <Doughnut data={garbageData} options={garbageOptions} />
          </div>
        </div>
      </Card>

      <Card className="w-full mt-10">
        <Bar data={projectBarData} options={projectBarOptions} />
      </Card>
    </div>
  );
}

export default function Dashboard() {
  return (
    <PageProtection permission="dashboard">
      <DashboardContent />
    </PageProtection>
  );
}
