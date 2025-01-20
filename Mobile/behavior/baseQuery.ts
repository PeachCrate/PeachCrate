import {BaseQueryFn, FetchArgs, fetchBaseQuery, FetchBaseQueryError} from "@reduxjs/toolkit/query";
import {RootState} from "@/behavior/store";
import * as SecureStore from 'expo-secure-store';
import {Tokens} from "@/behavior/auth/types";
import {setTokens} from "@/behavior/auth/authSlice";

export const BASE_API_URL = process.env.EXPO_PUBLIC_BASE_API_URL!;

export const baseQuery = fetchBaseQuery({
  baseUrl: BASE_API_URL,
  prepareHeaders: (headers, {getState}) => {
    const accessToken = SecureStore.getItem('accessToken');
    if (accessToken && !headers.has('Authorization')) {
      headers.set('Authorization', `Bearer ${accessToken}`);
    }
    console.log("headers", headers);
    return headers;
  },
});

export const baseQueryWithReauth: BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError> = async (
  args,
  api,
  extraOptions
) => {
  const refreshToken = SecureStore.getItem('refreshToken');
  if (!refreshToken)
    return {data: null};
  let result = await baseQuery(args, api, extraOptions);

  if (result.error && result.error.status === 401) {
    const {data} = await baseQuery({
      url: 'auth/refreshToken/',
      method: 'POST',
      body: {
        refreshToken: refreshToken
      },
      headers: {
        'Authorization': "Bearer " + refreshToken
      }
    }, api, extraOptions);

    if (data && typeof data == 'object' && 'accessToken' in data && 'refreshToken' in data) {
      const tokens = {accessToken: data.accessToken, refreshToken: data.refreshToken} as Tokens;
      api.dispatch(setTokens(tokens));

      // retry the initial query
      result = await baseQuery(args, api, extraOptions);
    } else {
      console.error('Auth failed');
    }
  }
  return result;
};