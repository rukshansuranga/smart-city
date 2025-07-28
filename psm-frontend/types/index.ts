export type LightPostComplaint = {
  id?: string;
  lightPostNumber: string;
  userId: string;
  complainDate: string;
  resolveDate: string;
  name: string;
  description: string;
  note: string;
  status: string;
};

export type Workpackage = {
  workPackageId?: string;
  name: string;
  detail: string;
  createdDate: Date;
  updatedDate: Date;
  status: string;
  clientId: string;
  ticketId: string;
  ticketPackages: Ticket[];
};

export type TicketPckage = {
  id: number;
  ticketId: number;
  workPackageId: number;
};

export type Ticket = {
  ticketId?: number;
  title: string;
  detail?: string;
  note?: string;
  userId: string;
  createdDate?: Date;
  status?: string;
  type?: string;
  user?: {
    userId: string;
    name: string;
  };
};

export type User = {
  userId: number;
  name: string;
  telNumber: string;
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
  name: string;
  description?: string;
  type: string;
  location?: string;
  specificationDocument?: string;
  estimatedCost?: number;
  tenderOpeningDate?: Date | null;
  tenderClosingDate?: Date | null;
  latitude?: number;
  longitude?: number;
  status: string;
}

export type Tender = {
  id?: number;
  projectId: string;
  project?: Project;
  name: string;
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
