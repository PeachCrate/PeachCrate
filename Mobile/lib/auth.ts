import * as SecureStore from "expo-secure-store";
import * as Linking from "expo-linking";
import {TokenCache} from "@clerk/clerk-expo/dist/cache";
import {authApi} from "@/behavior/auth/authApi";
import {OAuthSignInRequest, RegisterRequest} from "@/behavior/auth/types";
import store, {AppDispatch} from "@/behavior/store";
import {setTokens} from "@/behavior/auth/authSlice";
import {StartOAuthFlowParams, StartOAuthFlowReturnType} from "@clerk/clerk-expo";

const createTokenCache = (): TokenCache => {
  return {
    getToken: async (key: string) => {
      try {
        const item = await SecureStore.getItemAsync(key);
        if (item) {
          console.log(`${key} was used 🔐 \n`);
        } else {
          console.log("No values stored under key: " + key);
        }
        return item;
      } catch (error) {
        console.error("secure store get item error: ", error);
        await SecureStore.deleteItemAsync(key);
        return null;
      }
    },
    saveToken: (key: string, token: string) => {
      return SecureStore.setItemAsync(key, token);
    },
  };
};

// SecureStore is not supported on the web
export const tokenCache = createTokenCache();

export const googleOAuth = async (startOAuthFlow: (startOAuthFlowParams?: StartOAuthFlowParams) => Promise<StartOAuthFlowReturnType>, dispatch: AppDispatch) => {
  try {
    const {createdSessionId, setActive, signUp} = await startOAuthFlow({
      redirectUrl: Linking.createURL("/(root)/(tabs)/home"),
    });

    if (createdSessionId && signUp) {
      if (setActive) {
        await setActive({session: createdSessionId});

        if (signUp.createdUserId) {
          const request: OAuthSignInRequest = {
            login: signUp.firstName + ' ' + signUp.lastName,
            email: signUp.emailAddress!,
            clerkId: signUp.createdUserId,
          };
          console.log('req', request);
          const response = await dispatch(
            authApi.endpoints.oAuthSignIn.initiate(request)
          ).unwrap();
          dispatch(setTokens(response));
        }

        return {
          success: true,
          code: "success",
          message: "You have successfully signed in with Google",
        };
      }
    }

    return {
      success: false,
      message: "An error occurred while signing in with Google",
    };
  } catch (err: any) {
    console.error(err);
    return {
      success: false,
      code: err.code,
      message: err?.errors[0]?.longMessage,
    };
  }
};
