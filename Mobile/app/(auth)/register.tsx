import {
  Alert,
  Button,
  GestureResponderEvent,
  ScrollView,
  Text,
  View,
} from "react-native";
import InputField from "@/components/InputField";
import { FontAwesomeIcon } from "@fortawesome/react-native-fontawesome";
import {
  faPerson,
  faAt,
  faLock,
  faCheckCircle,
} from "@fortawesome/free-solid-svg-icons";
import CustomButton from "@/components/CustomButton";

import React, { useEffect, useState } from "react";
import OAuth from "@/components/OAuth";
import { Link, router } from "expo-router";
import { ReactNativeModal } from "react-native-modal";
import { useClerk, useSignUp } from "@clerk/clerk-expo";
import {
  authApi,
  useIsCredentialTakenQuery,
  useRegisterMutation,
} from "@/behavior/auth/authApi";
import { RegisterRequest } from "@/behavior/auth/types";
import { useAppDispatch } from "@/behavior/hooks";
import { setAccessToken } from "@/behavior/auth/authSlice";
import { Form, Formik, useFormik } from "formik";
import {
  initialRegisterForm,
  RegisterForm,
  registerFormSchema,
} from "@/app/(auth)/register.types";

const Register = () => {
  const { session } = useClerk();
  const { isLoaded, signUp, setActive } = useSignUp();
  const [showSuccessModal, setShowSuccessModal] = useState(false);
  const dispatch = useAppDispatch();

  const [form, setForm] = useState<RegisterForm>({
    name: "",
    email: "",
    password: "",
  });

  const [verification, setVerification] = useState({
    state: "default",
    error: "",
    code: "",
  });

  const [register, { isLoading }] = useRegisterMutation();
  // const {
  //   data: isTaken,
  //   isLoading: isLoadingCredentials,
  //   isError: isErrorCredentials,
  //   error: errorCredentials,
  // } = useIsCredentialTakenQuery({ login: form.name, email: form.email });

  // useEffect(() => {
  //   if (isErrorCredentials) Alert.alert(errorCredentials.data);
  // }, [isErrorCredentials]);

  useEffect(() => {
    const t = dispatch(authApi.endpoints.hello.initiate(null));
    console.log("t", t.data);
  }, []);

  // Handle submission of sign-up form
  const onSignUpPress = async (values: RegisterForm) => {
    if (!isLoaded) return;

    // Start sign-up process using email and password provided
    try {
      await signUp.create({
        emailAddress: values.email,
        password: values.password,
      });

      // Send user an email with verification code
      await signUp.prepareEmailAddressVerification({ strategy: "email_code" });

      // Set 'pendingVerification' to true to display second form
      // and capture OTP code
      setVerification({ ...verification, state: "pending" });
    } catch (err: any) {
      // See https://clerk.com/docs/custom-flows/error-handling
      // for more info on error handling
      console.error(JSON.stringify(err, null, 2));
      Alert.alert("Error", err.errors[0].longMessage);
    }
  };

  // Handle submission of verification form
  const onVerifyPress = async () => {
    if (!isLoaded) return;
    try {
      // Use the code the user provided to attempt verification
      const signUpAttempt = await signUp.attemptEmailAddressVerification({
        code: verification.code,
      });

      // If verification was completed, set the session to active
      // and redirect the user
      if (signUpAttempt.status === "complete") {
        /// TODO: create user in db
        await setActive({ session: signUpAttempt.createdSessionId });
        const request: RegisterRequest = {
          login: form.name,
          email: form.email,
          password: form.password,
          clerkId: session!.user.id!,
        };
        const resp = await register(request).unwrap();
        dispatch(setAccessToken(resp.accessToken.token));
        dispatch(setAccessToken(resp.refreshToken.token));
        setVerification({ ...verification, state: "success" });
        //router.replace("/");
      } else {
        // If the status is not complete, check why. User may need to
        // complete further steps.
        setVerification({
          ...verification,
          error: "Verification failed",
          state: "failed",
        });
        console.error(JSON.stringify(signUpAttempt, null, 2));
      }
    } catch (err: any) {
      // See https://clerk.com/docs/custom-flows/error-handling
      // for more info on error handling
      setVerification({
        ...verification,
        error: err.errors[0].longMessage,
        state: "failed",
      });
      console.error(JSON.stringify(err, null, 2));
    }
  };

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
        <Formik initialValues={initialRegisterForm} onSubmit={onSignUpPress}>
          {({ handleChange, handleBlur, handleSubmit, values, errors }) => (
            <View>
              <InputField
                label="Name"
                placeholder="Enter name"
                Icon={() => <FontAwesomeIcon icon={faPerson} />}
                value={values.name}
                onChangeText={handleChange("name")}
                onBlur={handleBlur}
                error={errors.name}
              />
              <InputField
                label="Email"
                placeholder="Enter email"
                Icon={() => <FontAwesomeIcon icon={faAt} />}
                textContentType="emailAddress"
                value={values.email}
                onChangeText={handleChange("email")}
                onBlur={handleBlur}
                error={errors.email}
              />
              <InputField
                label="Password"
                placeholder="Enter password"
                Icon={() => <FontAwesomeIcon icon={faLock} />}
                secureTextEntry={true}
                textContentType="password"
                value={values.password}
                onChangeText={handleChange("password")}
                onBlur={handleBlur}
                error={errors.password}
              />
              <CustomButton
                title="Sign Up"
                onPress={() => handleSubmit()}
                className="mt-6"
              />
            </View>
          )}
        </Formik>

        <OAuth />
        <Link
          href="/login"
          className="text-lg text-center text-general-200 mt-10"
          replace
        >
          Already have an account?{" "}
          <Text className="text-primary-500">Log In</Text>
        </Link>
        <View id="clerk-captcha" />
        <ReactNativeModal
          isVisible={verification.state === "pending"}
          onModalHide={() => {
            if (verification.state === "success") {
              setShowSuccessModal(true);
            }
          }}
        >
          <View className="bg-white px-7 py-9 rounded-2xl min-h-[300px]">
            <Text className="text-2xl font-JakartaExtraBold">Verification</Text>
            <Text className="font-Jakarta mb-5">
              Email sent to {form.email}
            </Text>
            <InputField
              label="Code"
              placeholder="123456"
              Icon={() => <FontAwesomeIcon icon={faLock} />}
              keyboardType="numeric"
              value={verification.code}
              onChangeText={(code) =>
                setVerification({ ...verification, code })
              }
              error={""}
            />
            {verification.error && (
              <Text className="text-red-500 text-sm mt-1"></Text>
            )}
            <CustomButton
              title="Verify Email"
              onPress={onVerifyPress}
              className="mt-5 bg-success-500"
            />
          </View>
        </ReactNativeModal>

        <ReactNativeModal isVisible={showSuccessModal}>
          <View className="bg-white px-7 py-9 rounded-2xl min-h-[300px]">
            <View className="w-[110px] h-[110px] mx-auto my-5">
              <FontAwesomeIcon
                icon={faCheckCircle}
                size={110}
                color={"green"}
              />
            </View>
            <Text className="text-3xl font-JakartaBold text-center">
              Verified
            </Text>
            <Text className="text-base text-gray-400 font-Jakarta text-center mt-2">
              You have successfully verified your account.
            </Text>
            <CustomButton
              title="Browse Home"
              onPress={() => {
                setShowSuccessModal(false);
                router.push(`/(root)/(tabs)/home`);
              }}
              className="mt-5"
            />
          </View>
        </ReactNativeModal>
      </View>
    </ScrollView>
  );
};

export default Register;
