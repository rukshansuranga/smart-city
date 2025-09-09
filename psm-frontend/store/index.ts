// Directory: /app/counter/_store/index.ts

import { create } from "zustand";
import { devtools } from "zustand/middleware";

// State types
interface States {
  council: string;
  setCouncil: (council: string) => void;
  location: { lat: number; lng: number };
  setLocation: (location: { lat: number; lng: number }) => void;
}

// Define the initial state
const initialState = {
  council: "",
  location: { lat: 7.8731, lng: 80.7718 },
};

// Create the store with middleware
export const useCouncilStore = create<States>()(
  devtools(
    (set) => ({
      // Initial state
      ...initialState,

      // Actions
      setCouncil: (council) => {
        console.log("Setting council:", council); // Debug log
        set({ council }, false, { type: "council/set", payload: council });
      },
      setLocation: (location) => {
        console.log("Setting location:", location); // Debug log
        set({ location }, false, { type: "location/set", payload: location });
      },
    }),
    {
      name: "Council Store",
      store: "CouncilStore", // Explicit store name
      enabled: true,
      serialize: true,
    }
  )
);
