import { getWorkpackageById } from "@/app/api/actions/workpackageAction";
import ComplainDetail from "@/app/components/complain/ComplainDetail";
import { Complain } from "@/types";
import { useEffect, useState } from "react";

export default function ComplainDetailContainer({
  complain,
}: {
  complain: Complain;
}) {
  const [project, setProject] = useState(null);
  const [lightPost, setLightPost] = useState(null);

  useEffect(() => {
    if (complain && complain.workpackageType === "ProjectComplain") {
      console.log("project complain", complain);
      fetchProjectByWorkpackageId(complain.complainId);
    } else if (complain && complain.workpackageType === "LightPostComplain") {
      fetchLightPostByWorkpackageId(complain.complainId);
    }
  }, [complain]);

  async function fetchProjectByWorkpackageId(complainId: string) {
    // Fetch project details by complainId if needed
    const work = await getWorkpackageById(
      complain?.complainId,
      "ProjectComplain"
    );

    setProject(work?.project);
  }

  async function fetchLightPostByWorkpackageId(complainId: string) {
    // Fetch project details by complainId if needed
    const work = await getWorkpackageById(
      complain?.complainId,
      "LightPostComplain"
    );
    setLightPost(work?.lightPost);
  }

  return (
    <div>
      <ComplainDetail
        complain={{ ...complain, project: project, lightPost: lightPost }}
      />
    </div>
  );
}
