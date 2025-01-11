import {View, Alert} from "react-native";
import React from "react";
import {FontAwesomeIcon} from "@fortawesome/react-native-fontawesome";
import {faGoogle} from "@fortawesome/free-brands-svg-icons";
import CustomButton from "@/components/CustomButton";
import {router} from "expo-router";
import {useOAuth} from "@clerk/clerk-expo";
import {googleOAuth} from "@/lib/auth";
import {Button, Text} from "react-native-paper";

const OAuth = () => {
  const {startOAuthFlow} = useOAuth({strategy: "oauth_google"});

  const handleGoogleSignIn = async () => {
    const result = await googleOAuth(startOAuthFlow);

    if (result.code === "session_exists") {
      Alert.alert("Success", "Session exists. Redirecting to home screen.");
      router.replace("/(root)/(tabs)/home");
    }

    Alert.alert(result.success ? "Success" : "Error", result.message);
  };

  return (
    <View>
      <View className="flex flex-row justify-center items-center mt-3 gap-x-3 mb-5">
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
