import { Redirect } from "expo-router";
import { useAuth, useClerk } from "@clerk/clerk-expo";
import React, { useEffect } from "react";
import { useAppDispatch } from "@/behavior/hooks";
import { setClientId } from "@/behavior/auth/authSlice";

const Home = () => {
  const dispatch = useAppDispatch();
  const { session } = useClerk();

  useEffect(() => {
    if (!session) return;
    dispatch(setClientId(session.user.id!));
  }, [session?.user.id]);

  if (session?.status) {
    return <Redirect href={"/(root)/(tabs)/home"} />;
  }
  return <Redirect href="/(auth)/welcome" />;
};

export default Home;
