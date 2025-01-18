import {createApi} from "@reduxjs/toolkit/query/react";
import {
  RegisterRequest,
  Tokens,
  HelloResp,
  IsLoginAndEmailTakenRequest, LoginRequest, OAuthSignInRequest,
} from "@/behavior/auth/types";
import {baseQuery, baseQueryWithReauth} from "@/behavior/baseQuery";

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
    deleteAccount: builder.mutation<string, null>({
      query: (request) => ({
        url: '/auth/',
        method: "DELETE",
      }),
    }),
    isCredentialTaken: builder.mutation<boolean, IsLoginAndEmailTakenRequest>({
      query: (request) => ({
        url: `/auth/IsCredentialTaken?login=${request.login}&email=${request.email}`,
        method: "POST",
      }),
    }),
    hello: builder.query<HelloResp, null>({
      query: () => ({ url: "/auth/hello" }),
    }),
  }),
});

// Export hooks for usage in functional components
export const { 
  useRegisterMutation, 
  useHelloQuery, 
  useIsCredentialTakenMutation, 
  useLoginMutation,
  useDeleteAccountMutation,
  useOAuthSignInMutation,
} = authApi;
