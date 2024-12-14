import React from "react";
import { Platform, Text, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import CustomButton from "@/components/CustomButton";
import { useAuth } from "@clerk/clerk-expo";
import { router } from "expo-router";

export default function RootLayout() {
  const { signOut } = useAuth();
  function handleSignOut() {
    signOut();
    router.replace("/(auth)/register");
  }

  return (
    <SafeAreaView>
      <Text>RootLayout</Text>
      <CustomButton title="sign out" onPress={handleSignOut} />
    </SafeAreaView>
  );
}
