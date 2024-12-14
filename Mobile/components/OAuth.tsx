import { View, Text } from "react-native";
import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-native-fontawesome";
import { faGoogle } from "@fortawesome/free-brands-svg-icons";
import CustomButton from "@/components/CustomButton";

const OAuth = () => {
  function handleGoogleSignIn() {}

  return (
    <View>
      <View className="flex flex-row justify-center items-center mt-4 gap-x-3">
        <View className="flex-1 h-[1px] bg-general-100" />
        <Text className="text-lg">Or</Text>
        <View className="flex-1 h-[1px] bg-general-100" />
      </View>
      <CustomButton
        title="Log In with Google"
        className="mt-5 w-full shadow-none"
        IconLeft={() => <FontAwesomeIcon icon={faGoogle} />}
        bgVariant="outline"
        textVariant="primary"
        onPress={handleGoogleSignIn}
        showShadow={false}
      />
    </View>
  );
};

export default OAuth;
