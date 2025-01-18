import * as Yup from "yup";

export const createYupSchema = <T extends object>(
  schema: Yup.ObjectSchema<T>,
): Yup.ObjectSchema<T> => schema;
