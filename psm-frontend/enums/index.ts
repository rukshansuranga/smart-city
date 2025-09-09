export enum ProjectStatus {
  New = 0,
  InProgress = 1,
  Completed = 2,
  OnHold = 3,
}
export enum ProjectType {
  Road = 0,
  Building = 1,
  Irrigation = 2,
}
export enum TicketPriority {
  Low = 0,
  Medium = 1,
  High = 2,
}
export enum TicketStatus {
  Open = 0,
  InProgress = 1,
  Resolved = 2,
  Closed = 3,
}
export enum ComplainStatus {
  New = 0,
  Assigned = 1,
  InProgress = 2,
  Closed = 3,
}

// export enum TicketType {
//   Internal = 0,
//   External = 1,
// }

// export enum TicketWorkpackageType {
//   GeneralComplain = 0,
//   LightPostComplain = 1,
//   ProjectComplain = 2,
// }

export enum ComplainType {
  GeneralComplain = 0,
  LightPostComplain = 1,
  ProjectComplain = 2,
}

export enum ProjectProgressApprovedStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2,
}

export enum ProjectCoordinatorType {
  Coordinator = 0,
  Supporter = 1,
}

export enum AuthType {
  Admin = 0,
  Staff = 1,
  Contractor = 2,
  Councillor = 3,
}

export enum TicketType {
  ProjectTicket = 0,
  ComplainTicket = 1,
  InternalTicket = 2,
}
