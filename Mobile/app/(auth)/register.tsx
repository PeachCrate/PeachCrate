import { ScrollView, Text, View } from "react-native";
import InputField from "@/components/InputField";
import { FontAwesomeIcon } from "@fortawesome/react-native-fontawesome";
import { faPerson, faAt, faLock } from "@fortawesome/free-solid-svg-icons";
import CustomButton from "@/components/CustomButton";

import React, { useState } from "react";
import OAuth from "@/components/OAuth";
import { Link } from "expo-router";

const Register = () => {
  const [form, setForm] = useState({
    name: "",
    email: "",
    password: "",
  });

  function onSignUpPress() {}

  return (
    <ScrollView className="flex-1 bg-white">
      <View className="flex-1 bg-white relative w-full h-[250]">
        <View className="text-2xl text-black font-JakartaSemiBold absolute bottom-5 left-5">
          <Text className="text-2xl text-black font-JakartaSemiBold absolute bottom-5 left-5">
            Create new account
          </Text>
        </View>
      </View>
      <View className="p-5">
        <InputField
          label="Name"
          placeholder="Enter name"
          Icon={() => <FontAwesomeIcon icon={faPerson} />}
          value={form.name}
          onChangeText={(value) => setForm({ ...form, name: value })}
        />
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
          onPress={onSignUpPress}
          className="mt-6"
        />
        <OAuth />
        <Link
          href="/login"
          className="text-lg text-center text-general-200 mt-10"
          replace
        >
          Already have an account?{" "}
          <Text className="text-primary-500">Log In</Text>
        </Link>
      </View>
    </ScrollView>
  );
};

export default Register;
