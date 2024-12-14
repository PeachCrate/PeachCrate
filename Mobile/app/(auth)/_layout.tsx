import {Stack, Tabs} from 'expo-router';
import React from 'react';
import {Platform, Text, View} from 'react-native';
import {SafeAreaView} from "react-native-safe-area-context";
import {useFonts} from "expo-font";

const Layout = () => {
  
  return (
    <Stack>
      <Stack.Screen name="welcome" options={{headerShown: false}}/>
      <Stack.Screen name="register" options={{headerShown: false}}/>
      <Stack.Screen name="login" options={{headerShown: false}}/>
    </Stack>
  );
};

export default Layout;
