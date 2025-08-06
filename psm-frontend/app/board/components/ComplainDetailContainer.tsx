import { getWorkpackageById } from "@/app/api/actions/workpackageAction";
import ComplainDetail from "@/app/components/complain/ComplainDetail";
import { Workpackage } from "@/types";
import { useEffect, useState } from "react";

export default function ComplainDetailContainer({
  workpackage,
}: {
  workpackage: Workpackage;
}) {
  const [project, setProject] = useState(null);
  const [lightPost, setLightPost] = useState(null);

  useEffect(() => {
    if (workpackage && workpackage.workpackageType === "ProjectComplain") {
      console.log("project complain", workpackage);
      fetchProjectByWorkpackageId(workpackage.workpackageId);
    } else if (
      workpackage &&
      workpackage.workpackageType === "LightPostComplain"
    ) {
      fetchLightPostByWorkpackageId(workpackage.workpackageId);
    }
  }, [workpackage]);

  async function fetchProjectByWorkpackageId(workpackageId: string) {
    // Fetch project details by workpackageId if needed
    const work = await getWorkpackageById(
      workpackage?.workpackageId,
      "ProjectComplain"
    );

    setProject(work?.project);
  }

  async function fetchLightPostByWorkpackageId(workpackageId: string) {
    // Fetch project details by workpackageId if needed
    const work = await getWorkpackageById(
      workpackage?.workpackageId,
      "LightPostComplain"
    );
    setLightPost(work?.lightPost);
  }

  return (
    <div>
      <ComplainDetail
        workpackage={{ ...workpackage, project: project, lightPost: lightPost }}
      />
    </div>
  );
}
