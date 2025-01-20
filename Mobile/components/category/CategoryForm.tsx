import {useEffect, useState} from "react";
import {useAddCategoryMutation, useGetCategoryQuery, useUpdateCategoryMutation} from "@/behavior/category/categoryApi";
import {SafeAreaView} from "react-native-safe-area-context";
import {Button, Text, TextInput} from "react-native-paper";
import React from "react";
import {useFormik} from "formik";
import {categoryFormSchema, initialCategoryForm} from "@/components/category/form.types";
import InputField from "@/components/basic/InputField";
import {Category} from "@/behavior/category/types";
import {useRouter} from "expo-router";

type CategoryFormProps = {
  isEdit: boolean,
  categoryId?: number,
}

const CategoryForm = ({isEdit, categoryId}: CategoryFormProps) => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const router = useRouter();

  const [addCategory] = useAddCategoryMutation();
  const [updateCategory] = useUpdateCategoryMutation();
  const {data} = useGetCategoryQuery(categoryId!, {skip: !isEdit});

  const handleSubmit = async () => {
    const category: Category = {title: formik.values.title!, description: formik.values.description!, categoryId: categoryId};
    if (isEdit) {
      await updateCategory({categoryId: categoryId!, ...category});
    } else {
      await addCategory(category);
    }
    router.back();
  };

  const formik = useFormik({
    initialValues: {title: data?.title, description: data?.description},
    validationSchema: categoryFormSchema,
    onSubmit: handleSubmit,
  })

  useEffect(() => {
    if (isEdit && data) {
      setTitle(data.title);
      setDescription(data.description || '');
    }
  }, [isEdit, data]);

  return (
    <>
      <Text variant='headlineSmall'>{isEdit ? 'Update category' : 'Create category'}</Text>
      <InputField
        label='Title'
        placeholder="Enter title"
        iconName='format-title'
        value={formik.values.title}
        onChangeText={formik.handleChange("title")}
        onBlur={formik.handleBlur("title")}
        error={formik.errors.title}
      />
      <InputField
        label='Description'
        placeholder="Enter description"
        iconName='subtitles'
        value={formik.values.description}
        onChangeText={formik.handleChange("description")}
        onBlur={formik.handleBlur("description")}
        error={formik.errors.description}
      />
      <Button mode="contained" onPress={() => formik.handleSubmit()} className="mt-4">
        {isEdit ? 'Update' : 'Create'}
      </Button>
    </>
  );
};

export default CategoryForm;