import { ProjectStatus, ProjectType, WorkpackageStatus } from "@/enums";
import { Workpackage } from "@/types";
import { Badge, Card, Label, Textarea, TextInput } from "flowbite-react";

export default function ComplainDetail({
  workpackage,
}: {
  workpackage: Workpackage;
}) {
  return (
    <div>
      <Card>
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
          <Badge color="pink" className="p-2">
            <Label>{WorkpackageStatus[workpackage.status]}</Label>
          </Badge>
          <Badge color="cyan" className="p-2">
            <Label>{workpackage.client?.firstName}</Label>
          </Badge>
        </div>
      </Card>

      {workpackage.workpackageType == "ProjectComplain" && (
        <Card className="mt-3">
          <div className="mt-6 mb-2">
            <h3>Project Details</h3>
          </div>

          <div className="flex justify-between mx-2 mt-4  w-full">
            <div>
              <div className="flex flex-col gap-3">
                <Label>Project Name</Label>
                <Badge color="info">
                  <Label>{workpackage.project?.subject}</Label>
                </Badge>
              </div>
            </div>

            <div>
              <div className="flex flex-col gap-3">
                <Label>Type</Label>
                <Badge color="indigo">
                  <Label>{ProjectType[workpackage.project?.type]}</Label>
                </Badge>
              </div>
            </div>

            <div>
              <div className="flex flex-col gap-3">
                <Label>Status</Label>
                <Badge color="pink">
                  <Label>{ProjectStatus[workpackage.project?.status]}</Label>
                </Badge>
              </div>
            </div>
          </div>
        </Card>
      )}

      {workpackage.workpackageType == "LightPostComplain" && (
        <Card className="mt-3">
          <div className="mt-6 mb-2">
            <h3>Light Post</h3>
          </div>
          <div className="flex justify-between mx-2 mt-4  w-full">
            <div>
              <div className="flex flex-col gap-3">
                <Label>Light Post Number</Label>
                <Badge color="info">
                  <Label>{workpackage.lightPost?.lightPostNumber}</Label>
                </Badge>
              </div>
            </div>

            <div>
              <div className="flex flex-col gap-3">
                <Label>Latitude</Label>
                <Badge color="indigo">
                  <Label>{workpackage.lightPost?.latitude}</Label>
                </Badge>
              </div>
            </div>

            <div>
              <div className="flex flex-col gap-3">
                <Label>Longitude</Label>
                <Badge color="pink">
                  <Label>{workpackage.lightPost?.longitude}</Label>
                </Badge>
              </div>
            </div>
          </div>
        </Card>
      )}
    </div>
  );
}
