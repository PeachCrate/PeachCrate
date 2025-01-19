import {createYupSchema} from "@/behavior/types";
import * as Yup from "yup";

export interface CategoryForm {
  title: string;
  description: string | undefined;
}

export const initialCategoryForm: CategoryForm = {
  title: "",
  description: "",
}

export const categoryFormSchema = createYupSchema<CategoryForm>(
  Yup.object().shape({
    title: Yup
      .string()
      .max(20, "Maximum length is 20 characters.")
      .min(5, "Minimum length is 5 characters.")
      .required("Title is required"),
    description: Yup
      .string()
      .max(100, "Maximum length is 100 characters.")
      .min(5, "Minimum length is 5 characters.")
  }),
)