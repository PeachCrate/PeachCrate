import { configureStore } from "@reduxjs/toolkit";
import authReducer from "@/behavior/auth/authSlice";
import { authApi } from "@/behavior/auth/authApi";
import {createEpicMiddleware} from "redux-observable";

const store = configureStore({
  reducer: {
    auth: authReducer,
    [authApi.reducerPath]: authApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(authApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export default store;
