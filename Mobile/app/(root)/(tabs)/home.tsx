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
  const [register, {isLoading}] = useRegisterMutation();
  
  const dispatch = useAppDispatch();
  
  if (!sessionId) {
    return <Redirect href={"/(auth)/welcome"}/>;
  }
  
  async function test() {
    const request: RegisterRequest = {
      login: "test",
      email: "test@mail",
      password: "qwerty13",
      clerkId: session!.user.id!,
    };
    console.log(request);
    const resp = await register(request)
      .unwrap()
      .catch((error) => console.error("DDDDD", error));
    console.log(resp);
    //dispatch(setAccessToken(resp.accessToken.token));
    //dispatch(setAccessToken(resp.refreshToken.token));
  }

  function foo() {
    // const {data} = useHelloQuery(null);
    // console.log("data", data);
  }

  return (
    <SafeAreaView>
      <Text variant='bodyMedium'>Hello {user?.emailAddresses[0].emailAddress}</Text>

      <Text variant='bodyMedium'>Hello {clerkUser?.emailAddresses[0].emailAddress}</Text>
      <View>
        {client.sessions.map((s) => (
          <Text key={s.id} variant='bodyMedium'>
            {s.status} {s.id} {s.user?.emailAddresses[0].emailAddress}
          </Text>
        ))}
      </View>
      <Button
        onPress={() => router.replace("/(auth)/welcome")}
        mode='contained'
      >Go to onboarding</Button>
      <Button mode='contained' onPress={() => foo()}>Test foo</Button>
      <Button mode='contained' onPress={() => test()}>Test request</Button>
      <SignedOut><Text>SIGN OUT OUT</Text></SignedOut>
      <SignedIn><Text>signin</Text></SignedIn>
    </SafeAreaView>
  );
};

export default Home;
