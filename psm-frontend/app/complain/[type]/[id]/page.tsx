import { getWorkpackageById } from "@/app/api/actions/workpackageAction";
import ComplainDetail from "@/app/components/complain/ComplainDetail";

export default async function ComplainDetailPage({
  params: { id, type },
}: {
  params: { id: string; type: string };
}) {
  const workpackage = await getWorkpackageById(id, type);

  return <ComplainDetail workpackage={workpackage} />;
}
