import axios from '../../../shared/lib/axios';
import type { Transaction, TransactionInput, TransactionFilters, TransactionCategory } from '../../../shared/types/transaction.types';

export async function listCategories(): Promise<TransactionCategory[]> {
  const { data } = await axios.get('/categories');
  return data;
}

export async function getTransactions(_filters?: TransactionFilters): Promise<Transaction[]> {
  const { data } = await axios.get('/transactions', { params: _filters });
  return data;
}

export async function createTransaction(input: TransactionInput): Promise<Transaction> {
  const { data } = await axios.post('/transactions', input);
  return data;
}

// export async function updateTransaction(id: string, input: Partial<TransactionInput>): Promise<Transaction> {
//   if (USE_MOCKS) {
//     await delay(200);
//     const list = readJSON<Transaction[]>(STORAGE_KEY, []);
//     const idx = list.findIndex((t) => t.id === id);
//     if (idx === -1) throw new Error('Not found');
//     const categories = readJSON<TransactionCategory[]>(CATEGORY_KEY, []);
//     const next = { ...list[idx], ...input } as Transaction;
//     if (input.categoryId) {
//       const cat = categories.find((c) => c.id === input.categoryId);
//       if (cat) next.categoryName = cat.name;
//     }
//     list[idx] = next;
//     writeJSON(STORAGE_KEY, list);
//     return next;
//   }
//   const { data } = await axios.put(`/transactions/${id}`, input);
//   return data;
// }

// export async function deleteTransaction(id: string): Promise<void> {
//   if (USE_MOCKS) {
//     await delay(200);
//     const list = readJSON<Transaction[]>(STORAGE_KEY, []);
//     writeJSON(STORAGE_KEY, list.filter((t) => t.id !== id));
//     return;
//   }
//   await axios.delete(`/transactions/${id}`);
// }
