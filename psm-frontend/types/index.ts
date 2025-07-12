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
  createdDate: string;
  updatedDate: string;
  status: string;
  clientId: string;
  ticketId: string;
};

export type Ticket = {
  ticketId?: number;
  title: string;
  detail: string;
  note: string;
  userId: string;
  createdDate?: string;
  user?: {
    userId: string;
    name: string;
  };
};

export type User = {
  userId: string;
  name: string;
  telNumber: string;
};

export type Paging<T> = {
  totalItems: number;
  records: T[];
};
