import { createApi } from "@reduxjs/toolkit/query/react";
import {fetchBaseQuery} from "@reduxjs/toolkit/query";
import { Root } from "./types";

export const barcodeApi = createApi({
  reducerPath: 'barcodeApi',
  baseQuery: fetchBaseQuery({baseUrl: "https://world.openfoodfacts.org/api/v0/"}),
  tagTypes: ['barcode'],
  endpoints: (builder) => ({
    getInfoAboutProduct: builder.query<Root, string>({
      query: (id) => `product/${id}.json`,
    }),
  }),
});

export const {
} = barcodeApi;