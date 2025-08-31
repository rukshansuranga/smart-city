import { getWorkpackageById } from "@/app/api/actions/workpackageAction";
import { ComplainStatus } from "@/enums";
import { Label, Textarea, TextInput } from "flowbite-react";

export default async function WorkpackageDetail({
  params: { id },
}: {
  params: { id: string };
}) {
  const complain = await getWorkpackageById(id);

  return (
    <div className="max-w-3xl mx-auto bg-white p-6 shadow-md rounded-lg">
      <div>
        <div className="mb-2 block">
          <Label>Subject</Label>
        </div>
        <TextInput value={complain.subject} disabled />
      </div>
      <div>
        <div className="mb-2 block">
          <Label>Detail</Label>
        </div>
        <Textarea value={complain.detail} disabled />
      </div>
      <div className="flex justify-between mx-2 mt-4">
        <Label>{ComplainStatus[complain.status]}</Label>
        <Label>{complain.client?.firstName}</Label>
      </div>
      {complain.workpackageType == "ProjectComplain" && (
        <>
          <div className="w-1/2">
            <Label>{complain.project?.id}</Label>
          </div>
          <div className="w-1/2">
            <Label>{complain.project?.description}</Label>
          </div>
        </>
      )}
    </div>
  );
}
