import {
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
      <TextInput
        theme={{roundness: 30}}
        label={label}
        placeholder={props.placeholder}
        mode='outlined'
        textContentType='nickname'
        value={props.value}
        onChangeText={props.onChangeText}
        left={iconName && <TextInput.Icon icon={iconName}/>}
        secureTextEntry={secureTextEntry}
      />
      {error && (
        <HelperText type='error'>{error}</HelperText>
      )}
    </>
  );
};

export default InputField;
