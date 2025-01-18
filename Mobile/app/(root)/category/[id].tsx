import {useLocalSearchParams} from "expo-router";
import {SafeAreaView} from "react-native-safe-area-context";
import CategoryForm from "@/components/category/CategoryForm";
import React from "react";

const CategoryFormScreen = () => {
  const params = useLocalSearchParams();
  const categoryId = +params.id;
  const isEdit = !!categoryId;
  
  return (
    <>
      <SafeAreaView className="flex-1 p-4">
        <CategoryForm isEdit={isEdit} categoryId={categoryId}/>
      </SafeAreaView>
    </>
  );
};

export default CategoryFormScreen;
