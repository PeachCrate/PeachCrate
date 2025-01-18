import { Text, View } from "react-native";
import React from "react";
import {SafeAreaView} from "react-native-safe-area-context";
import TeeterGame from "@/components/teeter/TeeterGame";

const Playground = () => {
  return (
    <SafeAreaView>
      <Text>Teeter</Text>
      <TeeterGame />
    </SafeAreaView>
  );
};

export default Playground;
