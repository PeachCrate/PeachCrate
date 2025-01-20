import {createApi} from "@reduxjs/toolkit/query/react";
import {
  RegisterRequest,
  Tokens,
  PingResponse,
  IsLoginAndEmailTakenRequest,
  LoginRequest,
  OAuthSignInRequest,
  SwitchAccountRequest, DeleteAccountRequest,
} from "@/behavior/auth/types";
import {baseQuery} from "@/behavior/baseQuery";

export const authApi = createApi({
  reducerPath: "authApi", // Define a reducer path
  baseQuery: baseQuery,
  tagTypes: ["Auth"], // Add relevant tag types for caching/invalidations
  endpoints: (builder) => ({
    register: builder.mutation<Tokens, RegisterRequest>({
      query: (request) => ({
        url: "/Auth/register",
        method: "POST",
        body: request,
      }),
    }),
    login: builder.mutation<Tokens, LoginRequest>({
      query: (request) => ({
        url: '/Auth/login',
        method: "POST",
        body: request,
      }),
    }),
    oAuthSignIn: builder.mutation<Tokens, OAuthSignInRequest>({
      query: (request) => ({
        url: '/Auth/OAuthSignIn',
        method: "POST",
        body: request
      })
    }),
    switchAccount: builder.mutation<Tokens, SwitchAccountRequest>({
      query: (request) => ({
        url: '/Auth/switchAccount',
        method: "POST",
        body: request
      })
    }),
    deleteAccount: builder.mutation<string, DeleteAccountRequest>({
      query: (request) => ({
        url: '/auth/',
        method: "DELETE",
        body: request,
      }),
    }),
    isCredentialTaken: builder.mutation<boolean, IsLoginAndEmailTakenRequest>({
      query: (request) => ({
        url: `/auth/IsCredentialTaken?login=${request.login}&email=${request.email}`,
        method: "POST",
      }),
    }),
    ping: builder.query<PingResponse, null>({
      query: () => ({url: "/auth/ping"}),
    }),
  }),
});

// Export hooks for usage in functional components
export const {
  useRegisterMutation,
  usePingQuery,
  useIsCredentialTakenMutation,
  useLoginMutation,
  useDeleteAccountMutation,
  useOAuthSignInMutation,
  useSwitchAccountMutation,
} = authApi;
