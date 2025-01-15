import * as Yup from "yup";
import {createYupSchema} from "@/behavior/types";

export interface RegisterFormType {
  name: string;
  email: string;
  password: string;
}

export const initialRegisterForm: RegisterFormType = {
  name: "",
  email: "",
  password: "",
};

export const registerFormSchema = createYupSchema<RegisterFormType>(
  Yup.object().shape({
    name: Yup
      .string()
      .max(20, "Maximum length is 20 characters.")
      .min(5, "Minimum length is 5 characters.")
      .required("Name is required"),
    email: Yup
      .string()
      .required("Email is required")
      .email("Invalid email format"),
    password: Yup
      .string()
      .required("Password is required")
  }),
);
