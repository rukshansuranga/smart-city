import {
  ProjectStatus,
  ProjectType,
  TicketStatus,
  TicketType,
  WorkpackageStatus,
} from "@/enums";

export type LightPostComplain = {
  id?: string;
  lightPostNumber: string;
  userId: string;
  complainDate: string;
  resolveDate: string;
  subject: string;
  description: string;
  note: string;
  status: string;
};

export type Workpackage = {
  workpackageId?: string;
  subject: string;
  detail: string;
  createdAt: Date;
  updatedAt: Date;
  status: WorkpackageStatus;
  clientId: string;
  client?: {
    userId: string;
    firstName: string;
  };
  project?: Project | null;
  lightPostNumber?: string;
  lightPost?: LightPost | null;
  workpackageType: string;
  ticketId: string;
  ticketPackages: Ticket[];
};

export type TicketPckage = {
  id: number;
  ticketId: number;
  workpackageId: number;
  workpackage?: Workpackage;
};

export type Ticket = {
  ticketId?: number;
  subject: string;
  detail?: string;
  note?: string;
  userId: string;
  createdAt?: Date;
  status?: TicketStatus;
  type?: TicketType;
  user?: {
    userId: string;
    firstName: string;
  };
  ticketPackages: TicketPckage[];
};

export type User = {
  userId: number;
  firstName: string;
  mobile: string;
};

export type Option = {
  value: string | number | null;
  text: string;
};

export type Paging<T> = {
  totalItems: number;
  records: T[];
};

export interface Project {
  id?: number;
  subject: string;
  description?: string;
  type: ProjectType;
  location?: string;
  specificationDocument?: string;
  estimatedCost?: number;
  tenderOpeningDate?: Date | null;
  tenderClosingDate?: Date | null;
  latitude?: number;
  longitude?: number;
  status: ProjectStatus;
}

export type Tender = {
  tenderId?: number;
  projectId: string;
  project?: Project;
  subject: string;
  note?: string;
  companyId: string;
  company?: Company;
  submittedDate?: Date;
  bidAmount: number;
};

export type Company = {
  id: number;
  name: string;
  address: string;
  //contactNumber: string;
};

export type LightPost = {
  lightPostNumber: string;
  location: string;
  latitude: number;
};
