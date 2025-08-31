import { fetchWrapper } from "@/lib/fetchWrapper";

export async function readNotification(notificationId: number): Promise<void> {
  console.log("Reading notification:", notificationId);
  return fetchWrapper.get(`notification/read/${notificationId}`);
}

export async function addRating(rating: {
  complainId: number;
  rating: number;
  note: string;
  clientId: string;
  notificationId: number;
}): Promise<void> {
  return fetchWrapper.post("notification/rating", rating);
}

export async function getNotifications(
  clientId: number
): Promise<Notification[]> {
  return fetchWrapper.get(`notification/client/${clientId}`);
}

export async function getUnreadNotificationCount(
  clientId: number
): Promise<number> {
  const response = await fetchWrapper.get(
    `notification/unread/count/${clientId}`
  );
  return response || 0;
}
