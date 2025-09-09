// import { getWorkpackageById } from "@/app/api/actions/workpackageAction";
import { getComplainById } from "@/app/api/client/complainAction";
import ComplainDetail from "@/app/components/complain/ComplainDetail";
import { Complain, LightPost, Project } from "@/types";
import { useEffect, useState } from "react";

export default function ComplainDetailContainer({
  complain,
}: {
  complain: Complain;
}) {
  const [project, setProject] = useState<Project | null | undefined>(null);
  const [lightPost, setLightPost] = useState<LightPost | null | undefined>(
    null
  );

  useEffect(() => {
    if (complain && complain.complainType === "ProjectComplain") {
      console.log("project complain", complain);
      fetchProjectByComplainId();
    } else if (complain && complain.complainType === "LightPostComplain") {
      fetchLightPostByComplainId();
    }
  }, [complain]);

  async function fetchProjectByComplainId() {
    // Fetch project details by complainId if needed
    const response = await getComplainById(
      complain?.complainId ?? "",
      "ProjectComplain"
    );

    if (!response.isSuccess) {
      console.log("Error fetching project complain:", response.message);
      return;
    }

    const project = response.data?.project;

    setProject(project);
  }

  async function fetchLightPostByComplainId() {
    // Fetch project details by complainId if needed
    const response = await getComplainById(
      complain?.complainId ?? "",
      "LightPostComplain"
    );

    if (!response.isSuccess) {
      console.log("Error fetching light post complain:", response.message);
      return;
    }

    const lightPost = response.data?.lightPost;

    setLightPost(lightPost);
  }

  return (
    <div>
      <ComplainDetail
        complain={{ ...complain, project: project, lightPost: lightPost }}
      />
    </div>
  );
}
