import * as Yup from "yup";
import { createYupSchema } from "@/behavior/types";

export interface RegisterForm {
  name: string;
  email: string;
  password: string;
}

export const initialRegisterForm: RegisterForm = {
  name: "",
  email: "",
  password: "",
};

export const registerFormSchema = createYupSchema<RegisterForm>(
  Yup.object().shape({
    name: Yup.string().required("Name is required"),
    email: Yup.string()
      .email("Invalid email format")
      .required("Email is required"),
    password: Yup.string().required("Password is required"),
  }),
);
