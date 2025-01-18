import { configureStore } from "@reduxjs/toolkit";
import authReducer from "@/behavior/auth/authSlice";
import { authApi } from "@/behavior/auth/authApi";
import {categoryApi} from "@/behavior/category/categoryApi";

const store = configureStore({
  reducer: {
    auth: authReducer,
    [authApi.reducerPath]: authApi.reducer,
    [categoryApi.reducerPath]: categoryApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware()
      .concat(authApi.middleware)
      .concat(categoryApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export default store;
