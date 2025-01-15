import { Alert, View } from "react-native";
import InputField from "@/components/InputField";
import { Button } from "react-native-paper";
import React from "react";
import { router } from "expo-router";
import { useSignIn } from "@clerk/clerk-expo";
import { useFormik } from "formik";
import { initialLoginForm, loginFormSchema } from "./login.types";

const LoginForm = () => {
  const { signIn, setActive, isLoaded } = useSignIn();

  const formik = useFormik({
    initialValues: initialLoginForm,
    validationSchema: loginFormSchema,
    onSubmit: async (values) => {
      if (!isLoaded) return;

      try {
        const signInAttempt = await signIn.create({
          identifier: values.email,
          password: values.password,
        });

        if (signInAttempt.status === "complete") {
          await setActive({ session: signInAttempt.createdSessionId });
          router.replace("/(root)/(tabs)/home");
        } else {
          console.log(JSON.stringify(signInAttempt, null, 2));
          Alert.alert("Error", "Log in failed. Please try again.");
        }
      } catch (err: any) {
        console.log(JSON.stringify(err, null, 2));
        Alert.alert("Error", err.errors[0].longMessage);
      }
    },
  });

  return (
    <View className="gap-3">
      <InputField
        label="Email"
        placeholder="Enter email"
        iconName="at"
        textContentType="emailAddress"
        value={formik.values.email}
        onChangeText={formik.handleChange("email")}
        onBlur={formik.handleBlur("email")}
        error={formik.errors.email}
      />
      <InputField
        label="Password"
        placeholder="Enter password"
        iconName="lock"
        secureTextEntry
        textContentType="password"
        value={formik.values.password}
        onChangeText={formik.handleChange("password")}
        onBlur={formik.handleBlur("password")}
        error={formik.errors.password}
      />
      <Button mode="contained" onPress={() => formik.handleSubmit()} className="mt-6">
        Login
      </Button>
    </View>
  );
};

export default LoginForm;
