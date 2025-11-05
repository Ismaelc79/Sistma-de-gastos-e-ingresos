import axios from '../../../shared/lib/axios';
import type { Category, CategoryInput } from '../../../shared/types/category.types';

export async function getCategories(): Promise<Category[]> {

  const { data } = await axios.get('/categories');
  return data;
  
}

export async function createCategory(input: CategoryInput): Promise<Category> {
  const { data } = await axios.post('/categories', input);
  return data;
}

