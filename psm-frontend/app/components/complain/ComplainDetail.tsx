import { ProjectStatus, ProjectType, ComplainStatus } from "@/enums";
import { Complain } from "@/types";
import { Badge, Card, Label, Textarea, TextInput } from "flowbite-react";

export default function ComplainDetail({ complain }: { complain: Complain }) {
  return (
    <div>
      <Card>
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
          <Badge color="pink" className="p-2">
            <Label>{ComplainStatus[complain.status]}</Label>
          </Badge>
          <Badge color="cyan" className="p-2">
            <Label>{complain.client?.firstName}</Label>
          </Badge>
        </div>
      </Card>

      {complain.workpackageType == "ProjectComplain" && (
        <Card className="mt-3">
          <div className="mt-6 mb-2">
            <h3>Project Details</h3>
          </div>

          <div className="flex justify-between mx-2 mt-4  w-full">
            <div>
              <div className="flex flex-col gap-3">
                <Label>Project Name</Label>
                <Badge color="info">
                  <Label>{complain.project?.subject}</Label>
                </Badge>
              </div>
            </div>

            <div>
              <div className="flex flex-col gap-3">
                <Label>Type</Label>
                <Badge color="indigo">
                  <Label>{ProjectType[complain.project?.type]}</Label>
                </Badge>
              </div>
            </div>

            <div>
              <div className="flex flex-col gap-3">
                <Label>Status</Label>
                <Badge color="pink">
                  <Label>{ProjectStatus[complain.project?.status]}</Label>
                </Badge>
              </div>
            </div>
          </div>
        </Card>
      )}

      {complain.workpackageType == "LightPostComplain" && (
        <Card className="mt-3">
          <div className="mt-6 mb-2">
            <h3>Light Post</h3>
          </div>
          <div className="flex justify-between mx-2 mt-4  w-full">
            <div>
              <div className="flex flex-col gap-3">
                <Label>Light Post Number</Label>
                <Badge color="info">
                  <Label>{complain.lightPost?.lightPostNumber}</Label>
                </Badge>
              </div>
            </div>

            <div>
              <div className="flex flex-col gap-3">
                <Label>Latitude</Label>
                <Badge color="indigo">
                  <Label>{complain.lightPost?.latitude}</Label>
                </Badge>
              </div>
            </div>

            <div>
              <div className="flex flex-col gap-3">
                <Label>Longitude</Label>
                <Badge color="pink">
                  <Label>{complain.lightPost?.longitude}</Label>
                </Badge>
              </div>
            </div>
          </div>
        </Card>
      )}
    </div>
  );
}
