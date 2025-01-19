import {baseQuery, baseQueryWithReauth} from "@/behavior/baseQuery";
import { createApi } from "@reduxjs/toolkit/query/react";
import {Category, CategoryQueryParams} from "@/behavior/category/types";
import {PingResp} from "@/behavior/auth/types";

export const categoryApi = createApi({
  reducerPath: 'categoryApi',
  baseQuery: baseQueryWithReauth,
  tagTypes: ['Category'],
  endpoints: (builder) => ({
    getCategories: builder.query<Category[], CategoryQueryParams>({
      query: (params) => ({
        url: '/Category',
        method: 'GET',
        params,
      }),
      //providesTags: ['Category'],
    }),
    getCategory: builder.query<Category, number>({
      query: (id) => `category/${id}`,
      providesTags: (result, error, id) => [{ type: 'Category', id }],
    }),
    addCategory: builder.mutation<Category, Omit<Category, 'categoryId'>>({
      query: (body) => ({
        url: 'category',
        method: 'POST',
        body,
      }),
      invalidatesTags: ['Category'],
    }),
    updateCategory: builder.mutation<Category, Category>({
      query: ({ categoryId, ...body }) => ({
        url: `category/${categoryId}`,
        method: 'PUT',
        body: {...body, categoryId}
      }),
      invalidatesTags: (result, error, { categoryId }) => [{ type: 'Category', id: categoryId }],
    }),
    deleteCategory: builder.mutation<void, number>({
      query: (id) => ({
        url: `category/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Category'],
    }),
  }),
});

export const {
  useGetCategoriesQuery,
  useGetCategoryQuery,
  useAddCategoryMutation,
  useUpdateCategoryMutation,
  useDeleteCategoryMutation
} = categoryApi;