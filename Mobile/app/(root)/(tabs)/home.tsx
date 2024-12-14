import { SafeAreaView } from "react-native-safe-area-context";
import { Text, View } from "react-native";
import React from "react";
import { SignedIn, SignedOut, useUser } from "@clerk/clerk-expo";
import { Link } from "expo-router";

const Home = () => {
  const { user } = useUser();

  return (
    <View>
      <SignedIn>
        <Text>Hello {user?.emailAddresses[0].emailAddress}</Text>
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
