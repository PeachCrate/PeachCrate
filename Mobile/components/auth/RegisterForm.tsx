import {Alert, View} from "react-native";
import InputField from "@/components/basic/InputField";
import {Button, Dialog, Portal, Text, useTheme} from "react-native-paper";
import React, {useState} from "react";
import {useFormik} from "formik";
import {initialRegisterForm, registerFormSchema, RegisterFormType} from "./register.types";
import {useClerk, useSignUp} from "@clerk/clerk-expo";
import {useAppDispatch} from "@/behavior/hooks";
import {useIsCredentialTakenMutation, useRegisterMutation} from "@/behavior/auth/authApi";
import {IsLoginAndEmailTakenRequest, RegisterRequest} from "@/behavior/auth/types";
import {setSessionId, setTokens} from "@/behavior/auth/authSlice";
import {router} from "expo-router";
import {FontAwesomeIcon} from "@fortawesome/react-native-fontawesome";
import {faCheckCircle} from "@fortawesome/free-solid-svg-icons";
import {useToast} from "react-native-paper-toast";
import {Is} from "@sinclair/typebox/value/is";

const RegisterForm = () => {
  const theme = useTheme();
  const toast = useToast()
  const {isLoaded, signUp, setActive} = useSignUp();
  const [showSuccessModal, setShowSuccessModal] = useState(false);
  const dispatch = useAppDispatch();
  const [register] = useRegisterMutation();
  const [isCredentialsTaken] = useIsCredentialTakenMutation();
  const [verification, setVerification] = useState({
    state: "default",
    error: "",
    code: "",
  });

  const formik = useFormik<RegisterFormType>({
    initialValues: initialRegisterForm,
    validationSchema: registerFormSchema,
    onSubmit: async (values) => {
      if (!isLoaded) return;

      try {
        const credsRequest: IsLoginAndEmailTakenRequest = {
          login: values.name,
          email: values.email,
        };
        let isTaken = false;
        await isCredentialsTaken(credsRequest)
          .unwrap()
          .catch(err => {
            toast.show({type: 'error', message: err.data});
            isTaken = true;
          });

        if (isTaken)
          return;

        await signUp.create({
          emailAddress: values.email,
          password: values.password,
        });

        await signUp.prepareEmailAddressVerification({strategy: "email_code"});
        setVerification({...verification, state: "pending"});
      } catch (err: any) {
        console.error(JSON.stringify(err, null, 2));
        Alert.alert("Error", err.errors[0].longMessage);
      }
    },
  });

  const onVerifyPress = async () => {
    if (!isLoaded) return;

    try {
      const signUpAttempt = await signUp.attemptEmailAddressVerification({
        code: verification.code,
      });

      if (signUpAttempt.status === "complete") {
        await setActive({session: signUpAttempt.createdSessionId});
        dispatch(setSessionId(signUpAttempt.createdSessionId!));
        const request: RegisterRequest = {
          login: formik.values.name,
          email: formik.values.email,
          password: formik.values.password,
          clerkId: signUpAttempt.createdUserId!,
        };

        const response = await register(request)
          .unwrap()
          .catch(err => Alert.alert(err));
        if (!response)
          return;
        dispatch(setTokens(response));
        setVerification({...verification, state: "success"});
        setShowSuccessModal(true);
      } else {
        setVerification({
          ...verification,
          error: "Verification failed",
          state: "failed",
        });
        toast.show({type:'error', message:"Verification failed" })
      }
    } catch (err: any) {
      setVerification({
        ...verification,
        error: err.errors[0].longMessage,
        state: "failed",
      });
      toast.show({type:'error', message:err.errors[0].longMessage })
    }
  };

  return (
    <>
      <View className="gap-3">
        <Button
          onPress={() => {
            formik.setValues({
              name: "Slava Varnav",
              email: "chudoha@gmail.com",
              password: "QazWsxEdc2213",
            });
          }}
        >
          Mock
        </Button>
        <InputField
          label="Name"
          placeholder="Enter name"
          iconName="account"
          value={formik.values.name}
          onChangeText={formik.handleChange("name")}
          onBlur={formik.handleBlur("name")}
          error={formik.errors.name}
        />
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
        <Button onPress={() => formik.handleSubmit()} className="mt-3" mode="contained">
          Sign Up
        </Button>
      </View>

      <Portal>
        <Dialog
          visible={verification.state === "pending"}
          onDismiss={() => {
            if (verification.state === "success") setShowSuccessModal(true);
          }}
        >
          <Dialog.Title>
            <Text variant="headlineMedium">Verification</Text>
          </Dialog.Title>
          <Dialog.Content>
            <Text className="mb-5" variant="bodyMedium">
              Email sent to {formik.values.email}
            </Text>
            <InputField
              label="Code"
              placeholder="123456"
              iconName="lock"
              keyboardType="numeric"
              value={verification.code}
              onChangeText={(code) => setVerification({...verification, code})}
              error={verification.error}
            />
            <Button className="mt-5" mode="contained" onPress={onVerifyPress}>
              Verify Email
            </Button>
          </Dialog.Content>
        </Dialog>
      </Portal>

      <Portal>
        <Dialog visible={showSuccessModal}>
          <View className="px-7 py-9 rounded-2xl min-h-[300px]">
            <View className="w-[110px] h-[110px] mx-auto my-5">
              <FontAwesomeIcon icon={faCheckCircle} size={110} color="green"/>
            </View>
            <Text className="text-center" variant="headlineMedium">
              Verified
            </Text>
            <Text className="text-gray-400 font-Jakarta text-center mt-2" variant="bodyMedium">
              You have successfully verified your account.
            </Text>
            <Button
              className="mt-5"
              onPress={() => {
                setShowSuccessModal(false);
                router.push(`/(root)/(tabs)/home`);
              }}
            >
              Browse Home
            </Button>
          </View>
        </Dialog>
      </Portal>
    </>
  );
};

export default RegisterForm;
