import {
  KeyboardAvoidingView, Platform,
  TextInputProps,
} from "react-native";
import React from "react";
import {HelperText, Text, TextInput, useTheme} from "react-native-paper";

interface InputFieldProps extends TextInputProps {
  label: string;
  iconName: string | undefined;
  secureTextEntry?: boolean;
  error: string | undefined;
}

const InputField = ({
                      label,
                      iconName,
                      secureTextEntry = false,
                      error,
                      ...props
                    }: InputFieldProps) => {
  const theme = useTheme();
  return (
    <>
      <KeyboardAvoidingView
        behavior={Platform.OS === "ios" ? "padding" : "height"}
      >
        <TextInput
          theme={{roundness: 10}}
          label={label}
          placeholder={props.placeholder}
          mode='flat'
          textContentType='nickname'
          value={props.value}
          onChangeText={props.onChangeText}
          left={iconName && <TextInput.Icon icon={iconName}/>}
          secureTextEntry={secureTextEntry}
        />
      </KeyboardAvoidingView>
      {error && (
        <HelperText type='error'>{error}</HelperText>
      )}
    </>
  );
};

export default InputField;
