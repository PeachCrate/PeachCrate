import { Text, View } from "react-native";
import React from "react";
import { SignedIn, SignedOut, useClerk, useUser } from "@clerk/clerk-expo";
import { Link, router } from "expo-router";
import CustomButton from "@/components/CustomButton";
import { useAppDispatch, useAppSelector } from "@/behavior/hooks";
import { RootState } from "@/behavior/store";
import { RegisterRequest } from "@/behavior/auth/types";
import { setAccessToken } from "@/behavior/auth/authSlice";
import {
  useHelloQuery,
  useIsCredentialTakenQuery,
  useIsLoginAndEmailTakenQuery,
  useRegisterMutation,
} from "@/behavior/auth/authApi";

const Home = () => {
  const { user } = useUser();
  const { user: clerkUser, client, session } = useClerk();
  const userId = useAppSelector((state: RootState) => state.auth.clientId);
  console.log("userid", userId);
  const [register, { isLoading }] = useRegisterMutation();
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
    const { data } = useHelloQuery(null);
    console.log("data", data);
  }

  return (
    <View>
      <SignedIn>
        {/*<Text>*/}
        {/*  Isloading: {isLoadingHello}, data: {helloMessage?.message}*/}
        {/*</Text>*/}
        {/*{isError ? <Text>{error.data}</Text>}*/}
        <Text>Hello {user?.emailAddresses[0].emailAddress}</Text>

        <Text>Hello {clerkUser?.emailAddresses[0].emailAddress}</Text>
        <View>
          {client.sessions.map((s) => (
            <Text key={s.id}>
              {s.status} {s.id} {s.user?.emailAddresses[0].emailAddress}
            </Text>
          ))}
        </View>
        <CustomButton
          title={"Go to onboarding"}
          onPress={() => router.replace("/(auth)/welcome")}
        />
        <CustomButton title={"Test foo"} onPress={() => foo()} />
        <CustomButton title={"Test request"} onPress={() => test()} />
      </SignedIn>
      <SignedOut>
        <Link href="../../(auth)/login">
          <Text>Sign in</Text>
        </Link>
        <Link href="../../(auth)/register">
          <Text>Sign up</Text>
        </Link>
      </SignedOut>
    </View>
  );
};

export default Home;
