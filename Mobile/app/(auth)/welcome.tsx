import { SafeAreaView } from "react-native-safe-area-context";
import { Text, TouchableOpacity, View } from "react-native";
import { router } from "expo-router";
import { FontAwesomeIcon } from "@fortawesome/react-native-fontawesome";
import { faSpider } from "@fortawesome/free-solid-svg-icons/faSpider";
import { faMugSaucer } from "@fortawesome/free-solid-svg-icons/faMugSaucer";
import CustomButton from "@/components/CustomButton";
import React from "react";

const Welcome = () => {
  return (
    <SafeAreaView className="flex h-full items-center justify-end bg-white">
      <View className="flex pb-96">
        <Text>Our beautiful welcome page</Text>
      </View>
      <View className="flex flex-row flex-wrap m-3 p-2">
        <CustomButton
          title="Register"
          className="w-1/2"
          onPress={() => {
            router.replace("/(auth)/register");
          }}
          IconRight={() => <FontAwesomeIcon icon={faSpider} />}
        />
        <CustomButton
          title="Login"
          className="w-1/2"
          onPress={() => {
            router.replace("/(auth)/login");
          }}
          IconRight={() => <FontAwesomeIcon icon={faMugSaucer} />}
        />
      </View>
    </SafeAreaView>
  );
};

export default Welcome;
