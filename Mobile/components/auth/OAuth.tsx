import {View, Alert} from "react-native";
import React from "react";

import {router} from "expo-router";
import {useOAuth} from "@clerk/clerk-expo";
import {googleOAuth} from "@/lib/auth";
import {Button, Text} from "react-native-paper";
import {useToast} from "react-native-paper-toast";
import {useAppDispatch} from "@/behavior/hooks";

const OAuth = () => {
  const toast = useToast();
  const {startOAuthFlow} = useOAuth({strategy: "oauth_google"});
  const dispatch = useAppDispatch();
  
  const handleGoogleSignIn = async () => {
    const result = await googleOAuth(startOAuthFlow, dispatch);
    console.log('AUTH RESULT', result);
    if (result.code === "session_exists") {
      toast.show({type: "success", message: "Session exists. Redirecting to home screen."});
      router.replace("/(root)/(tabs)/home");
    }
    toast.show({type: result.success ? 'success' : 'error', message: result.message})
  };

  return (
    <View>
      <View className="flex flex-row justify-center items-center mt-3 gap-x-3 mb-3">
        <View className="flex-1 h-[1px] bg-general-100"/>
        <Text variant='bodyLarge'>Or</Text>
        <View className="flex-1 h-[1px] bg-general-100"/>
      </View>
      <Button
        icon='google'
        mode='outlined'
        onPress={handleGoogleSignIn}>
        Log In with Google
      </Button>
    </View>
  );
};

export default OAuth;
