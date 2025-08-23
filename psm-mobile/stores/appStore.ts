import { create } from "zustand";

type AppState = {
  unreadNotificationCount: number | null;
  updateNotificationCount: (count: number | null) => void;
};

export const appStore = create<AppState>((set) => ({
  unreadNotificationCount: null,
  updateNotificationCount: (count) => set({ unreadNotificationCount: count }),
}));
