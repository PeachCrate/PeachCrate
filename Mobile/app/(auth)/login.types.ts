import {createYupSchema} from "@/behavior/types";
import * as Yup from "yup";

export interface LoginForm {
  email: string;
  password: string;
}

export const initialLoginForm: LoginForm = {
  email: "",
  password: "",
};

export const loginFormSchema = createYupSchema<LoginForm>(
  Yup.object().shape({
    email: Yup
      .string()
      .required("Email is required")
      .email("Invalid email format"),
    password: Yup
      .string()
      .required("Password is required")
  }),
);
