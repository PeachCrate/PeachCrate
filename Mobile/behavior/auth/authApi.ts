import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import {
  RegisterRequest,
  RegisterResponse,
  HelloResp,
  IsLoginAndEmailTakenRequest,
} from "@/behavior/auth/types";

const baseQuery = fetchBaseQuery({
  baseUrl: "http://10.0.2.2:5093/api",
});

export const authApi = createApi({
  reducerPath: "authApi", // Define a reducer path
  baseQuery,
  tagTypes: ["Auth"], // Add relevant tag types for caching/invalidations
  endpoints: (builder) => ({
    register: builder.mutation<RegisterResponse, RegisterRequest>({
      query: (request) => ({
        url: "/Auth/register",
        method: "POST",
        body: request,
      }),
    }),
    isCredentialTaken: builder.query<boolean, IsLoginAndEmailTakenRequest>({
      query: (request) => ({
        url: `/auth/IsCredentialTaken?login=${request.login}&email=${request.email}`,
      }),
    }),
    hello: builder.query<HelloResp, null>({
      query: () => ({ url: "/auth/hello" }),
    }),
  }),
});

// Export hooks for usage in functional components
export const { useRegisterMutation, useHelloQuery, useIsCredentialTakenQuery } =
  authApi;
