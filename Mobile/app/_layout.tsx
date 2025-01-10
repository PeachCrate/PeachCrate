import { useFonts } from "expo-font";
import { Stack } from "expo-router";
import * as SplashScreen from "expo-splash-screen";
import React, { useEffect } from "react";
import "react-native-reanimated";

import { ClerkProvider, ClerkLoaded } from "@clerk/clerk-expo";
import "../global.css";
import { tokenCache } from "@/lib/auth";
import { Provider } from "react-redux";
import store from "@/behavior/store";
import { MD3DarkTheme, MD3LightTheme, PaperProvider } from "react-native-paper";
import { useColorScheme } from "react-native";

// Prevent the splash screen from auto-hiding before asset loading is complete.
SplashScreen.preventAutoHideAsync();

const publishableKey = process.env.EXPO_PUBLIC_CLERK_PUBLISHABLE_KEY!;

export default function RootLayout() {
  const colorScheme = useColorScheme();
  const [loaded] = useFonts({
    "Jakarta-Bold": require("../assets/fonts/PlusJakartaSans-Bold.ttf"),
    "Jakarta-ExtraBold": require("../assets/fonts/PlusJakartaSans-ExtraBold.ttf"),
    "Jakarta-ExtraLight": require("../assets/fonts/PlusJakartaSans-ExtraLight.ttf"),
    "Jakarta-Light": require("../assets/fonts/PlusJakartaSans-Light.ttf"),
    "Jakarta-Medium": require("../assets/fonts/PlusJakartaSans-Medium.ttf"),
    Jakarta: require("../assets/fonts/PlusJakartaSans-Regular.ttf"),
    "Jakarta-SemiBold": require("../assets/fonts/PlusJakartaSans-SemiBold.ttf"),
  });

  useEffect(() => {
    if (loaded) {
      SplashScreen.hideAsync();
    }
  }, [loaded]);

  if (!loaded) {
    return null;
  }

  if (!publishableKey) {
    throw new Error(
      "Missing Publishable Key. Please set EXPO_PUBLIC_CLERK_PUBLISHABLE_KEY in your .env",
    );
  }

  const lightColors = {
    colors: {
      primary: "rgb(120, 74, 154)",
      onPrimary: "rgb(255, 255, 255)",
      primaryContainer: "rgb(243, 218, 255)",
      onPrimaryContainer: "rgb(46, 0, 77)",
      secondary: "rgb(151, 64, 102)",
      onSecondary: "rgb(255, 255, 255)",
      secondaryContainer: "rgb(255, 217, 228)",
      onSecondaryContainer: "rgb(62, 0, 33)",
      tertiary: "rgb(0, 99, 152)",
      onTertiary: "rgb(255, 255, 255)",
      tertiaryContainer: "rgb(205, 229, 255)",
      onTertiaryContainer: "rgb(0, 29, 50)",
      error: "rgb(186, 26, 26)",
      onError: "rgb(255, 255, 255)",
      errorContainer: "rgb(255, 218, 214)",
      onErrorContainer: "rgb(65, 0, 2)",
      background: "rgb(255, 251, 255)",
      onBackground: "rgb(29, 27, 30)",
      surface: "rgb(255, 251, 255)",
      onSurface: "rgb(29, 27, 30)",
      surfaceVariant: "rgb(234, 223, 234)",
      onSurfaceVariant: "rgb(75, 69, 77)",
      outline: "rgb(124, 117, 126)",
      outlineVariant: "rgb(205, 195, 206)",
      shadow: "rgb(0, 0, 0)",
      scrim: "rgb(0, 0, 0)",
      inverseSurface: "rgb(50, 47, 51)",
      inverseOnSurface: "rgb(246, 239, 243)",
      inversePrimary: "rgb(226, 182, 255)",
      elevation: {
        level0: "transparent",
        level1: "rgb(248, 242, 250)",
        level2: "rgb(244, 237, 247)",
        level3: "rgb(240, 232, 244)",
        level4: "rgb(239, 230, 243)",
        level5: "rgb(236, 226, 241)",
      },
      surfaceDisabled: "rgba(29, 27, 30, 0.12)",
      onSurfaceDisabled: "rgba(29, 27, 30, 0.38)",
      backdrop: "rgba(52, 46, 55, 0.4)",
    },
  };

  const darkColors = {
    colors: {
      primary: "rgb(226, 182, 255)",
      onPrimary: "rgb(70, 24, 104)",
      primaryContainer: "rgb(95, 50, 128)",
      onPrimaryContainer: "rgb(243, 218, 255)",
      secondary: "rgb(255, 176, 205)",
      onSecondary: "rgb(93, 17, 55)",
      secondaryContainer: "rgb(122, 41, 78)",
      onSecondaryContainer: "rgb(255, 217, 228)",
      tertiary: "rgb(148, 204, 255)",
      onTertiary: "rgb(0, 51, 82)",
      tertiaryContainer: "rgb(0, 75, 116)",
      onTertiaryContainer: "rgb(205, 229, 255)",
      error: "rgb(255, 180, 171)",
      onError: "rgb(105, 0, 5)",
      errorContainer: "rgb(147, 0, 10)",
      onErrorContainer: "rgb(255, 180, 171)",
      background: "rgb(29, 27, 30)",
      onBackground: "rgb(231, 225, 229)",
      surface: "rgb(29, 27, 30)",
      onSurface: "rgb(231, 225, 229)",
      surfaceVariant: "rgb(75, 69, 77)",
      onSurfaceVariant: "rgb(205, 195, 206)",
      outline: "rgb(150, 142, 152)",
      outlineVariant: "rgb(75, 69, 77)",
      shadow: "rgb(0, 0, 0)",
      scrim: "rgb(0, 0, 0)",
      inverseSurface: "rgb(231, 225, 229)",
      inverseOnSurface: "rgb(50, 47, 51)",
      inversePrimary: "rgb(120, 74, 154)",
      elevation: {
        level0: "transparent",
        level1: "rgb(39, 35, 41)",
        level2: "rgb(45, 39, 48)",
        level3: "rgb(51, 44, 55)",
        level4: "rgb(53, 46, 57)",
        level5: "rgb(57, 49, 62)",
      },
      surfaceDisabled: "rgba(231, 225, 229, 0.12)",
      onSurfaceDisabled: "rgba(231, 225, 229, 0.38)",
      backdrop: "rgba(52, 46, 55, 0.4)",
    },
  };

  const lightTheme = {
    ...MD3LightTheme,
    ...lightColors,
  };

  const darkTheme = {
    ...MD3DarkTheme,
    ...darkColors,
  };

  const theme = colorScheme == "dark" ? darkTheme : lightTheme;

  return (
    <Provider store={store}>
      <PaperProvider theme={theme}>
        <ClerkProvider publishableKey={publishableKey} tokenCache={tokenCache}>
          <ClerkLoaded>
            <Stack>
              <Stack.Screen name="index" options={{ headerShown: false }} />
              <Stack.Screen name="(auth)" options={{ headerShown: false }} />
              <Stack.Screen name="(root)" options={{ headerShown: false }} />
              <Stack.Screen name="+not-found" />
            </Stack>
          </ClerkLoaded>
        </ClerkProvider>
      </PaperProvider>
    </Provider>
  );
}
