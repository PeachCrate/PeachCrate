import {createSlice} from "@reduxjs/toolkit";
import type {PayloadAction} from "@reduxjs/toolkit";
import {Tokens} from "@/behavior/auth/types";
import * as SecureStore from 'expo-secure-store';

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

export const authSlice = createSlice({
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
    setTokens: (state, action: PayloadAction<Tokens>) => {
      state.accessToken = action.payload.accessToken.token;
      state.refreshToken = action.payload.refreshToken.token;
      SecureStore.setItem('accessToken', action.payload.accessToken.token);
      SecureStore.setItem('refreshToken', action.payload.refreshToken.token)
    },
    clearTokens: (state) => {
      state.accessToken = '';
      state.refreshToken = '';
      SecureStore.setItem('refreshToken', '');
      SecureStore.setItem('accessToken', '');
    }
  },
});

// Action creators are generated for each case reducer function
export const {setClientId, setAccessToken, setRefreshToken, setTokens, clearTokens} =
  authSlice.actions;

export default authSlice.reducer;
