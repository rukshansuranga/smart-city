import {
  ProjectStatus,
  ProjectType,
  TicketStatus,
  TicketType,
  TicketWorkpackageType,
  ComplainStatus,
  ProjectProgressApprovedStatus,
} from "@/enums";

// Role-based access control types
export type UserRole =
  | "admin"
  | "manager"
  | "staff"
  | "contractor"
  | "councillor";

export type Permission =
  | "dashboard"
  | "board"
  | "complain"
  | "ticket"
  | "projects"
  | "user"
  | "projectprogress";

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
  isIncluded?: boolean;
};

export type Complain = {
  complainId?: string;
  subject: string;
  detail: string;
  createdAt: Date;
  updatedAt: Date;
  status: ComplainStatus;
  clientId: string;
  client?: {
    userId: string;
    firstName: string;
  };
  project?: Project | null;
  lightPostNumber?: string;
  lightPost?: LightPost | null;
  complainType: string;
  ticketId: string;
  ticketPackages: Ticket[];
};

export type TicketPckage = {
  id: number;
  ticketId: number;
  complainId: number;
  complain?: Complain;
};

export type Ticket = {
  ticketId?: number;
  subject: string;
  detail?: string;
  note?: string;
  userId: string;
  createdAt?: Date;
  status: TicketStatus;
  type?: TicketType;
  estimate: number;
  priority?: number;
  dueDate?: Date | string;
  user?: {
    userId: string;
    firstName: string;
  };
  ticketPackages?: TicketPckage[];
  packages?: Complain[];
  ticketWorkpackageType?: TicketWorkpackageType;
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
  projectId?: string; // Alternative project ID (for API consistency)
  subject: string; // Project name/title
  description?: string; // Project description
  type: ProjectType; // Project type (Road, Building, etc.)
  city: string; // City/Area name
  specificationDocument?: string | File | null; // Document file or URL
  estimatedCost?: number; // Project estimated cost
  tenderOpeningDate?: Date | null; // Tender opening date
  tenderClosingDate?: Date | null; // Tender closing date
  latitude?: number; // Location latitude
  longitude?: number; // Location longitude
  status: ProjectStatus; // Project status
  createdAt?: Date; // Project creation date
  updatedAt?: Date; // Last update date
  tenders?: Tender[]; // Related tenders
  complains?: Complain[]; // Related complaints
  startDate?: Date | null; // Project start date
  endDate?: Date | null; // Project end date
  location?: string; // Project location
  locationNote?: string | null; // Additional location notes
  awardedTenderId?: number; // ID of the awarded tender
}

export type Tender = {
  tenderId?: number;
  projectId: string;
  project?: Project;
  subject: string;
  note?: string;
  contractorId: string;
  contractor?: Contractor;
  submittedDate: Date;
  tenderDocument?: string | File | null;
  bidAmount: number;
};

export type Contractor = {
  contractorId: number;
  name: string;
  address: string;
  //contactNumber: string;
};

// Backend API Response wrapper type
export type ApiResponse<T> = {
  isSuccess: boolean;
  message: string;
  data: T;
  errors: string[];
};

export type LightPost = {
  lightPostNumber: string;
  location: string;
  latitude: number;
  longitude: number;
};

type LightPostComplainWithChecked = LightPostComplain & { isChecked?: boolean };

export type ActiveLightPostMarker = {
  lightPostNumber: string;
  lightPost: LightPost;
  complains: LightPostComplainWithChecked[];
  isIncluded?: boolean; // Indicates if the light post is included in the cart
};

export type UpdateTicketPayload = {
  ticketId: number;
  complainIds: number[];
};

export type ProjectProgress = {
  projectProgressId: string;
  projectId: string;
  summary?: string;
  description?: string;
  progressDate: string;
  progressPercentage: number;
  approvedBy?: string;
  approvedAt?: Date;
  approvedNote?: string;
  projectProgressApprovedStatus?: ProjectProgressApprovedStatus;
};
