import { Tabs } from "expo-router";
import React from "react";
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

const TabIcon = ({
  source,
  focused,
}: {
  source: IconDefinition;
  focused: boolean;
}) => (
  <View className={`${focused ? "mb-4 " : ""}`}>
    <FontAwesomeIcon icon={source} size={30} color={"orange"} />
  </View>
);

export default function TabsLayout() {
  return (
    <Tabs
      //initialRouteName="index"
      screenOptions={{
        headerShown: false,
        tabBarStyle: {
          backgroundColor: "#1f2937", // Tailwind gray-800
          height: 60,
          borderTopWidth: 0,
        },
        tabBarActiveTintColor: "#3b82f6", // Tailwind blue-500
        tabBarInactiveTintColor: "#9ca3af", // Tailwind gray-400
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
            <TabIcon source={faHouse} focused={focused.focused} />
          ),
        }}
      />
      <Tabs.Screen
        name="playground"
        options={{
          title: "Playground",
          headerShown: false,
          tabBarIcon: (focused) => (
            <TabIcon source={faBasketball} focused={focused.focused} />
          ),
        }}
      />
      <Tabs.Screen
        name="shoppingLists"
        options={{
          title: "Lists",
          headerShown: false,
          tabBarIcon: (focused) => (
            <TabIcon source={faShoppingBasket} focused={focused.focused} />
          ),
        }}
      />
      <Tabs.Screen
        name="products"
        options={{
          title: "Products",
          headerShown: false,
          tabBarIcon: (focused) => (
            <TabIcon source={faProductHunt} focused={focused.focused} />
          ),
        }}
      />
      <Tabs.Screen
        name="profile"
        options={{
          title: "Profile",
          headerShown: false,
          tabBarIcon: (focused) => (
            <TabIcon source={faUser} focused={focused.focused} />
          ),
        }}
      />
    </Tabs>
  );
}
