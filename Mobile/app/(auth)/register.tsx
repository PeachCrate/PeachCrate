import {
  ScrollView,
  View,
} from "react-native";
import React, {useEffect, useState} from "react";
import {Link} from "expo-router";
import { Text, useTheme} from "react-native-paper";
import RegisterForm from "@/components/auth/RegisterForm";
import OAuth from "@/components/auth/OAuth";

const Register = () => {
  const theme = useTheme();

  return (
    <ScrollView className="flex-1">
      <View className="flex-1 relative w-full h-[250]">
        <View className="text-2xl text-black font-JakartaSemiBold absolute bottom-5 left-5">
          <Text variant='headlineSmall'>
            Create new account
          </Text>
        </View>
      </View>
      <View className="p-5">
        <RegisterForm />
        <OAuth/>
        <Link
          href="/login"
          className="text-lg text-center text-general-200 mt-10"
          replace
        >
          Already have an account?{" "}
          <Text style={{color: theme.colors.tertiary}}>Log In</Text>
        </Link>
        <View id="clerk-captcha"/>
      </View>
    </ScrollView>
  );
};

export default Register;
