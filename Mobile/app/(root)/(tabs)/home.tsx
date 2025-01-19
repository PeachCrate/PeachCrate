import {View} from "react-native";
import React from "react";
import {SignedIn, SignedOut, useClerk, useUser} from "@clerk/clerk-expo";
import {Link, Redirect, router} from "expo-router";
import {useAppDispatch, useAppSelector} from "@/behavior/hooks";
import {RootState} from "@/behavior/store";
import {RegisterRequest} from "@/behavior/auth/types";
import {setAccessToken} from "@/behavior/auth/authSlice";
import {
  useHelloQuery,
  useRegisterMutation,
} from "@/behavior/auth/authApi";
import {SafeAreaView} from "react-native-safe-area-context";
import {Button, Text} from "react-native-paper";

const Home = () => {
  const {user} = useUser();
  const {user: clerkUser, client, session} = useClerk();
  const sessionId = useAppSelector(state => state.auth.sessionId);

  // if (!sessionId) {
  //   return <Redirect href={"/"}/>;
  // }

  return (
    <SafeAreaView>
      <SignedIn>
        <Text variant='bodyMedium'>Hello {user?.emailAddresses[0].emailAddress}</Text>
        <Text variant='bodyMedium'>Hello {clerkUser?.emailAddresses[0].emailAddress}</Text>
        <View>
          {client.sessions.map((s) => (
            <Text key={s.id} variant='bodyMedium'>
              {s.status} {s.id} {s.user?.emailAddresses[0].emailAddress}
            </Text>
          ))}
        </View>
      </SignedIn>
      <SignedOut>
        <Text>You are not logged in</Text>
        <Button
          onPress={() => router.replace("/(auth)/welcome")}
          mode='contained'
        >
          Go to onboarding
        </Button>
      </SignedOut>
    </SafeAreaView>
  );
};

export default Home;
