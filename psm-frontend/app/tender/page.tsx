"use client";
import { useEffect, useState } from "react";
import { getAllProjects } from "../api/actions/projectActions";
import { Button, Spinner } from "flowbite-react";
import Link from "next/link";
import { Project } from "@/types";

export default function TenderPage() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [selectedProject, setSelectedProject] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    fetchProjects();
  }, []);

  async function fetchProjects() {
    setIsLoading(true);
    // Fetch projects from API
    const list = await getAllProjects();
    setProjects(list);
    setIsLoading(false);
  }

  console.log("seleted project", selectedProject);

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spinner aria-label="Center-aligned Large spinner example" />
      </div>
    );
  }

  return (
    <div className="flex flex-col items-center justify-center p-4 gap-5">
      <p>This is the tender page where you can manage tenders.</p>

      <div>
        <label htmlFor="project-select" className="block mb-2">
          Project List
        </label>
        <select
          id="project-select"
          value={selectedProject ?? ""}
          onChange={(e) => setSelectedProject(e.target.value)}
          className="border rounded p-2"
        >
          <option value="">Select Project</option>
          {projects.map((project) => (
            <option key={project.id} value={project.id}>
              {project.name}
            </option>
          ))}
        </select>
      </div>
      <div>
        <Button disabled={!selectedProject} className="mt-4">
          <Link href={`/tender/list/${selectedProject}`}>Manage Tenders</Link>
        </Button>
      </div>
    </div>
  );
}
