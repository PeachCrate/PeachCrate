import { SafeAreaView } from "react-native-safe-area-context";
import { ScrollView, Text, View } from "react-native";
import React, { useState } from "react";
import InputField from "@/components/InputField";
import { FontAwesomeIcon } from "@fortawesome/react-native-fontawesome";
import { faAt, faLock, faPerson } from "@fortawesome/free-solid-svg-icons";
import CustomButton from "@/components/CustomButton";
import OAuth from "@/components/OAuth";
import { Link } from "expo-router";

const Login = () => {
  const [form, setForm] = useState({
    email: "",
    password: "",
  });

  function onSignInPress() {}

  return (
    <ScrollView className="flex-1 bg-white">
      <View className="flex-1 bg-white relative w-full h-[250]">
        <View className="text-2xl text-black font-JakartaSemiBold absolute bottom-5 left-5">
          <Text className="text-2xl text-black font-JakartaSemiBold absolute bottom-5 left-5">
            Log in
          </Text>
        </View>
      </View>
      <View className="p-5">
        <InputField
          label="Email"
          placeholder="Enter email"
          Icon={() => <FontAwesomeIcon icon={faAt} />}
          textContentType="emailAddress"
          value={form.email}
          onChangeText={(value) => setForm({ ...form, email: value })}
        />
        <InputField
          label="Password"
          placeholder="Enter password"
          Icon={() => <FontAwesomeIcon icon={faLock} />}
          secureTextEntry={true}
          textContentType="password"
          value={form.password}
          onChangeText={(value) => setForm({ ...form, password: value })}
        />
        <CustomButton
          title="Sign Up"
          onPress={onSignInPress}
          className="mt-6"
        />
        <OAuth />
        <Link
          href="/register"
          className="text-lg text-center text-general-200 mt-10"
          replace
        >
          Don&#39;t have an account?{" "}
          <Text className="text-primary-500">Sign up</Text>
        </Link>
      </View>
    </ScrollView>
  );
};

export default Login;
