import {View} from "react-native";
import React from "react";
import {SignedIn, SignedOut, useClerk, useUser} from "@clerk/clerk-expo";
import {Link, router} from "expo-router";
import {useAppDispatch, useAppSelector} from "@/behavior/hooks";
import {RootState} from "@/behavior/store";
import {RegisterRequest} from "@/behavior/auth/types";
import {setAccessToken} from "@/behavior/auth/authSlice";
import {
  useHelloQuery,
  useIsCredentialTakenQuery,
  useRegisterMutation,
} from "@/behavior/auth/authApi";
import {SafeAreaView} from "react-native-safe-area-context";
import {Button, Text} from "react-native-paper";

const Home = () => {
  const {user} = useUser();
  const {user: clerkUser, client, session} = useClerk();
  const userId = useAppSelector((state: RootState) => state.auth.clientId);
  console.log("userid", userId);
  const [register, {isLoading}] = useRegisterMutation();
  //const { data: helloMessage, isLoading: isLoadingHello } = useHelloQuery(null, {skip});
  //const {data: isCredentialsTaken, {isLoading, error}} = useIsCredentialTakenQuery();
  const dispatch = useAppDispatch();

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
        <Button
          onPress={() => router.replace("/(auth)/welcome")}
          mode='contained'
        >Go to onboarding</Button>
        <Button mode='contained' onPress={() => foo()}>Test foo</Button>
        <Button mode='contained' onPress={() => test()}>Test request</Button>
      </SignedIn>
      <SignedOut>
        <Link href="../../(auth)/login">
          <Text>Sign in</Text>
        </Link>
        <Link href="../../(auth)/register">
          <Text>Sign up</Text>
        </Link>
      </SignedOut>
    </SafeAreaView>
  );
};

export default Home;
