import { deleteItemAsync, getItemAsync, setItemAsync } from "expo-secure-store";
import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";

type UserState = {
  isSignedIn: boolean;
  accessToken: string | null;
  idToken: string | null;
  userInfo: any | null;

  _hasHydrated: boolean;
  logIn: (token: {
    accessToken: string;
    idToken: string;
    userInfo: any;
  }) => void;
  logOut: () => void;
  setHasHydrated: (value: boolean) => void;
};

export const useAuthStore = create(
  persist<UserState>(
    (set) => ({
      isSignedIn: false,
      accessToken: null,
      idToken: null,
      userInfo: null,

      _hasHydrated: false,
      logIn: (token: {
        accessToken: string;
        idToken: string;
        userInfo: any;
      }) => {
        set((state) => {
          console.log("Logging in with token:", token);
          return {
            ...state,
            isSignedIn: true,
            accessToken: token.accessToken,
            idToken: token.idToken,
            userInfo: token.userInfo,
          };
        });
      },

      logOut: () => {
        set((state) => ({
          ...state,
          accessToken: null,
          idToken: null,
          userInfo: null,
          isSignedIn: false,
        }));
      },

      setHasHydrated: (value: boolean) => {
        set((state) => ({
          ...state,
          _hasHydrated: value,
        }));
      },
    }),
    {
      name: "auth-store",
      storage: createJSONStorage(() => ({
        setItem: setItemAsync,
        getItem: getItemAsync,
        removeItem: deleteItemAsync,
      })),
      onRehydrateStorage: (state) => {
        return () => state.setHasHydrated(true);
      },
    }
  )
);
