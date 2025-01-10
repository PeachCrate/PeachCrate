import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";

export interface AuthState {
  clientId: string;
  accessToken: string;
  refreshToken: string;
}

const initialState: AuthState = {
  clientId: "",
  accessToken: "",
  refreshToken: "",
};

export const counterSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setClientId: (state, action: PayloadAction<string>) => {
      state.clientId = action.payload;
    },
    setAccessToken: (state, action: PayloadAction<string>) => {
      state.accessToken = action.payload;
    },
    setRefreshToken: (state, action: PayloadAction<string>) => {
      state.refreshToken = action.payload;
    },
  },
});

// Action creators are generated for each case reducer function
export const { setClientId, setAccessToken, setRefreshToken } =
  counterSlice.actions;

export default counterSlice.reducer;
