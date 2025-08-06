import { getWorkpackageById } from "@/app/api/actions/workpackageAction";
import { WorkpackageStatus } from "@/enums";
import { Label, Textarea, TextInput } from "flowbite-react";

export default async function WorkpackageDetail({
  params: { id },
}: {
  params: { id: string };
}) {
  const workpackage = await getWorkpackageById(id);

  return (
    <div className="max-w-3xl mx-auto bg-white p-6 shadow-md rounded-lg">
      <div>
        <div className="mb-2 block">
          <Label>Subject</Label>
        </div>
        <TextInput value={workpackage.subject} disabled />
      </div>
      <div>
        <div className="mb-2 block">
          <Label>Detail</Label>
        </div>
        <Textarea value={workpackage.detail} disabled />
      </div>
      <div className="flex justify-between mx-2 mt-4">
        <Label>{WorkpackageStatus[workpackage.status]}</Label>
        <Label>{workpackage.client?.firstName}</Label>
      </div>
      {workpackage.workpackageType == "ProjectComplain" && (
        <>
          <div className="w-1/2">
            <Label>{workpackage.project?.id}</Label>
          </div>
          <div className="w-1/2">
            <Label>{workpackage.project?.description}</Label>
          </div>
        </>
      )}
    </div>
  );
}
