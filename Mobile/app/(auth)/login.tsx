import { SafeAreaView } from "react-native-safe-area-context";
import { Alert, ScrollView, Text, View } from "react-native";
import React, { useCallback, useState } from "react";
import InputField from "@/components/InputField";
import { FontAwesomeIcon } from "@fortawesome/react-native-fontawesome";
import { faAt, faLock, faPerson } from "@fortawesome/free-solid-svg-icons";
import CustomButton from "@/components/CustomButton";
import OAuth from "@/components/OAuth";
import { Link, router } from "expo-router";
import { useSignIn } from "@clerk/clerk-expo";

const Login = () => {
  const { signIn, setActive, isLoaded } = useSignIn();

  const [form, setForm] = useState({
    email: "",
    password: "",
  });

  const onSignInPress = useCallback(async () => {
    if (!isLoaded) return;

    try {
      const signInAttempt = await signIn.create({
        identifier: form.email,
        password: form.password,
      });

      if (signInAttempt.status === "complete") {
        await setActive({ session: signInAttempt.createdSessionId });
        router.replace("/(root)/(tabs)/home");
      } else {
        // See https://clerk.com/docs/custom-flows/error-handling for more info on error handling
        console.log(JSON.stringify(signInAttempt, null, 2));
        Alert.alert("Error", "Log in failed. Please try again.");
      }
    } catch (err: any) {
      console.log(JSON.stringify(err, null, 2));
      Alert.alert("Error", err.errors[0].longMessage);
    }
  }, [isLoaded, form]);

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
