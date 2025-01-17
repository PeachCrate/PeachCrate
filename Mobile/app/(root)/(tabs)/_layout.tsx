import { router, Tabs } from "expo-router";
import React, { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-native-fontawesome";
import {
  faBasketball,
  faHouse,
  faShoppingBasket,
  faUser,
} from "@fortawesome/free-solid-svg-icons";
import { View } from "react-native";
import { IconDefinition } from "@fortawesome/fontawesome-svg-core";
import { faProductHunt } from "@fortawesome/free-brands-svg-icons";
import {useTheme} from "react-native-paper";
import TabIcon from "@/components/basic/TabIcon";

export default function TabsLayout() {
  const theme = useTheme();
  /// TODO: Change bottom navigation
  /// TODO: START DEVELOPING APPLICATION !!!!!!!

  return (
    <>
    <Tabs
      initialRouteName="home"
      screenOptions={{
        headerShown: false,
        tabBarStyle: {
          backgroundColor: theme.colors.background, // Tailwind gray-800
          height: 60,
          borderTopWidth: 0,
        },
        tabBarActiveTintColor: theme.colors.primary, // Tailwind blue-500
        tabBarInactiveTintColor: theme.colors.backdrop, // Tailwind gray-400
        tabBarLabelStyle: {
          fontSize: 12,
          fontWeight: "600",
        },
      }}
    >
      <Tabs.Screen
        name="home"
        options={{
          title: "Home",
          headerShown: false,
          tabBarIcon: (focused) => (
            <TabIcon source={"home"} focused={focused.focused} />
          ),
        }}
      />
      <Tabs.Screen
        name="playground"
        options={{
          title: "Playground",
          headerShown: false,
          tabBarIcon: (focused) => (
            <TabIcon source={"basketball-hoop"} focused={focused.focused} />
          ),
        }}
      />
      <Tabs.Screen
        name="categoriesList"
        options={{
          title: "Categories",
          headerShown: false,
          tabBarIcon: (focused) => (
            <TabIcon source={'rectangle'} focused={focused.focused} />
          ),
        }}
      />
      <Tabs.Screen
        name="profile"
        options={{
          title: "Profile",
          headerShown: false,
          tabBarIcon: (focused) => (
            <TabIcon source={'account'} focused={focused.focused} />
          ),
        }}
      />
    </Tabs>
    </>
  );
}
