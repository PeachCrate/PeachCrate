import { Redirect } from "expo-router";
import { useAuth, useClerk } from "@clerk/clerk-expo";
import React, { useEffect } from "react";
import { useAppDispatch } from "@/behavior/hooks";
import {setClientId, setSessionId} from "@/behavior/auth/authSlice";
import * as ScreenOrientation from "expo-screen-orientation";


const Index = () => {
  const dispatch = useAppDispatch();
  const { session } = useClerk();

  useEffect(() => {
    const lockOrientation = async () => {
      await ScreenOrientation.lockAsync(ScreenOrientation.OrientationLock.PORTRAIT);
    };
    lockOrientation();
  }, []);
  
  useEffect(() => {
    if (!session) return;
    dispatch(setClientId(session.user.id!));
    dispatch(setSessionId(session.id));
  }, [session?.user.id]);

  if (session?.status) {
    return <Redirect href={"/(root)/(tabs)/home"} />;
  }
  return <Redirect href="/(auth)/welcome" />;
};

export default Index;
