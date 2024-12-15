import { SafeAreaView } from "react-native-safe-area-context";
import { Text, View } from "react-native";
import React from "react";
import { SignedIn, SignedOut, useClerk, useUser } from "@clerk/clerk-expo";
import { Link, router } from "expo-router";
import CustomButton from "@/components/CustomButton";

const Home = () => {
  const { user } = useUser();
  const { user: clerkUser, session, client } = useClerk();

  return (
    <View>
      <SignedIn>
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
