import {SafeAreaView} from "react-native-safe-area-context";
import {Alert, ScrollView, View} from "react-native";
import React, {useCallback, useState} from "react";
import InputField from "@/components/InputField";
import {FontAwesomeIcon} from "@fortawesome/react-native-fontawesome";
import {faAt, faLock, faPerson} from "@fortawesome/free-solid-svg-icons";
import CustomButton from "@/components/CustomButton";
import OAuth from "@/components/OAuth";
import {Link, router} from "expo-router";
import {useSignIn} from "@clerk/clerk-expo";
import {Button, Icon, Text, TextInput, useTheme} from 'react-native-paper';
import {initialRegisterForm, registerFormSchema} from "@/app/(auth)/register.types";
import {Formik} from "formik";
import {initialLoginForm, loginFormSchema} from "@/app/(auth)/login.types";

const Login = () => {
  const theme = useTheme();
  const {signIn, setActive, isLoaded} = useSignIn();

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
        await setActive({session: signInAttempt.createdSessionId});
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
    <ScrollView className="flex-1">
      <View className="flex-1 relative w-full h-[250]">
        <View className="absolute bottom-5 left-5">
          <Text variant='headlineSmall'>
            Log in
          </Text>
        </View>
      </View>
      <View className="flex flex-col m-5">
        <Formik
          initialValues={initialLoginForm}
          validationSchema={loginFormSchema}
          onSubmit={onSignInPress}>
          {({handleChange, handleBlur, handleSubmit, values, errors}) => (
            <View className='gap-3'>
              <InputField
                label="Email"
                placeholder="Enter email"
                iconName='at'
                textContentType="emailAddress"
                value={values.email}
                onChangeText={handleChange("email")}
                onBlur={handleBlur}
                error={errors.email}
              />
              <InputField
                label="Password"
                placeholder="Enter password"
                iconName='lock'
                secureTextEntry={true}
                textContentType="password"
                value={values.password}
                onChangeText={handleChange("password")}
                onBlur={handleBlur}
                error={errors.password}
              />
              <Button mode='contained' onPress={() => handleSubmit()} className="mt-6">Login</Button>
            </View>
          )}
        </Formik>
        <OAuth/>
        <Link
          href="/register"
          className="text-lg text-center text-general-200 mt-10"
          replace
        >
          Don&#39;t have an account?{" "}
          <Text style={{color: theme.colors.tertiary}}>Sign up</Text>
        </Link>
      </View>
    </ScrollView>
  );
};

export default Login;
