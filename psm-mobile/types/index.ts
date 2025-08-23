import { NotificationStatus, NotificationType } from "@/enums/enum";

export enum WorkpackageStatus {
  New,
  InProgress,
  Close,
}

export type Client = {
  clientId: string;
  addressLine1?: string;
  addressLine2?: string;
  firstName?: string;
  lastName?: string;
  mobile?: string;
  city?: string;
};

export type Workpackage = {
  workpackageId: number;
  subject: string;
  detail?: string;
  status?: WorkpackageStatus;
  clientId?: string;
  client: {
    clientId?: string;
    firstName?: string;
    lastName?: string;
    mobile?: string;
  };
  workpackageType?: string;
};

export type GeneralComplain = Workpackage & {
  isPrivate: boolean;
};

export type Comment = {
  commentId?: number;
  text: string;
  workpackageId: number;
  isPrivate: boolean;
  clientId?: string;
  client: {
    clientId?: string;
    firstName?: string;
    lastName?: string;
    mobile?: string;
  };
  userId?: string;
};

export type Notification = {
  id: number;
  subject: string;
  message?: string;
  clientId: string;
  status: NotificationStatus;
  type: NotificationType;
  workpackage: Workpackage;
  isRead: boolean;
};
