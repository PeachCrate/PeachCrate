import {View} from "react-native";
import React from "react";
import {SafeAreaView} from "react-native-safe-area-context";
import TeeterGame from "@/components/teeter/TeeterGame";
import {Text} from "react-native-paper";

const Playground = () => {
  return (
    <SafeAreaView>
      <View>
        <Text variant='headlineLarge'>Teeter</Text>
        <TeeterGame/>
      </View>
    </SafeAreaView>
  );
};

export default Playground;
