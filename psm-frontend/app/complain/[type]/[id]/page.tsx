import { getWorkpackageById } from "@/app/api/actions/workpackageAction";
import ComplainDetail from "@/app/components/complain/ComplainDetail";

export default async function ComplainDetailPage({
  params,
}: {
  params: Promise<{ id: string; type: string }>;
}) {
  const { id, type } = await params;
  const complain = await getWorkpackageById(id, type);

  if (!complain.isSuccess) {
    return <div>Error loading complain details</div>;
  }

  return <ComplainDetail complain={complain.data} />;
}
