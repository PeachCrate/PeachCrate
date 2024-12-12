import {Image, StyleSheet, Platform, Button, View} from 'react-native';
import {useState} from "react";
import {useAuthRequest} from 'expo-auth-session/providers/google'
import {maybeCompleteAuthSession} from "expo-web-browser";
import {makeRedirectUri} from "expo-auth-session";
import {
  GoogleSignin, isErrorWithCode,
  isNoSavedCredentialFoundResponse,
  isSuccessResponse, statusCodes
} from '@react-native-google-signin/google-signin';
import {SafeAreaView} from "react-native-safe-area-context";

maybeCompleteAuthSession();

GoogleSignin.configure({
  webClientId: '895609237505-t8ei9i1450sg1d570jupcjnflh7dmhpa.apps.googleusercontent.com',
});

export default function HomeScreen() {


  const signIn = async () => {
    try {
      await GoogleSignin.hasPlayServices();
      const response = await GoogleSignin.signIn();

      if (isSuccessResponse(response)) {
        // read user's info
        console.log(response.data);
      } else if (response.type === 'cancelled') {
        // Handle cancelled response
        console.log("Sign-in was cancelled.");
      } else if (isNoSavedCredentialFoundResponse(response)) {
        // Android and Apple only.
        // No saved credential found (user has not signed in yet, or they revoked access)
        // call `createAccount()`
      }
    } catch (error) {
      console.error(error);
    }
  };
  // const [userInfo, setUserInfo] = useState(null);
  // const [request, response, promptAsync] = useAuthRequest({
  //   androidClientId: "895609237505-1eobcbefmk0556nqshsj3271jq6j8aep.apps.googleusercontent.com",
  //   webClientId: "895609237505-t8ei9i1450sg1d570jupcjnflh7dmhpa.apps.googleusercontent.com",
  //   redirectUri: makeRedirectUri({
  //     scheme: "mobile"
  //   })
  // })
  // console.log("response", response);
  // console.log("request", request);
  return (
    <SafeAreaView>
      <View>
        <Button title="Sign in" onPress={() => signIn()}/>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  titleContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  stepContainer: {
    gap: 8,
    marginBottom: 8,
  },
  reactLogo: {
    height: 178,
    width: 290,
    bottom: 0,
    left: 0,
    position: 'absolute',
  },
});
