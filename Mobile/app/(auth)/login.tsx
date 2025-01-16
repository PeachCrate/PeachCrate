import { ScrollView, View} from "react-native";
import React from "react";
import {Link, router} from "expo-router";
import {useSignIn} from "@clerk/clerk-expo";
import {Button, Icon, Text, TextInput, useTheme} from 'react-native-paper';
import LoginForm from "@/components/auth/LoginForm";
import OAuth from "@/components/auth/OAuth";

const Login = () => {
  const theme = useTheme();
  return (
    <ScrollView className="flex-1">
      <View className="flex-1 relative w-full h-[250]">
        <View className="absolute bottom-5 left-5">
          <Text variant='headlineSmall'>
            Log in
          </Text>
        </View>
      </View>
      <View className="flex flex-col m-5">
        <LoginForm/>
        <OAuth/>
        <Link
          href="/register"
          className="text-lg text-center text-general-200 mt-10"
          replace
        >
          Don&#39;t have an account?{" "}
          <Text style={{color: theme.colors.tertiary}}>Sign up</Text>
        </Link>
      </View>
    </ScrollView>
  );
};

export default Login;
