export interface Category {
  categoryId?: number;
  title: string;
  description?: string;
}

export interface CategoryQueryParams {
  orderBy?: string;
  filterBy?: string;
  filterValue?: string;
  pageNum?: number;
  pageStart?: number;
}

