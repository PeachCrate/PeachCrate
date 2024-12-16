import { Redirect } from "expo-router";
import { useAuth, useClerk } from "@clerk/clerk-expo";
import React from "react";

const Home = () => {
  const { session } = useClerk();
  //console.log("user", user);
  console.log();
  //console.log("session", session);
  if (session?.status) {
    return <Redirect href={"/(root)/(tabs)/home"} />;
  }
  return <Redirect href="/(auth)/welcome" />;
};

export default Home;
