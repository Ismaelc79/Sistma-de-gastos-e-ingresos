import axios from '../../../shared/lib/axios';
import { delay, readJSON, writeJSON } from '../../../shared/lib/mock';
import type { Transaction, TransactionInput, TransactionFilters, TransactionCategory } from '../../../shared/types/transaction.types';

const USE_MOCKS = import.meta.env.VITE_USE_MOCKS === 'true';
const STORAGE_KEY = 'mock-transactions';
const CATEGORY_KEY = 'mock-categories';

function seed() {
  const seeded = readJSON<Transaction[]>(STORAGE_KEY, []);
  if (seeded.length === 0) {
    // seed categories
    const categories: TransactionCategory[] = [
      { id: 'cat-food', name: 'Food' },
      { id: 'cat-salary', name: 'Salary' },
      { id: 'cat-rent', name: 'Rent' },
      { id: 'cat-transport', name: 'Transport' },
      { id: 'cat-entertainment', name: 'Entertainment' },
    ];
    writeJSON(CATEGORY_KEY, categories);

    const now = new Date();
    const list: Transaction[] = [
      { id: crypto.randomUUID(), type: 'income', amount: 2500, categoryId: 'cat-salary', categoryName: 'Salary', date: new Date(now.getFullYear(), now.getMonth(), 1).toISOString() },
      { id: crypto.randomUUID(), type: 'expense', amount: 650, categoryId: 'cat-rent', categoryName: 'Rent', date: new Date(now.getFullYear(), now.getMonth(), 2).toISOString(), description: 'Monthly rent' },
      { id: crypto.randomUUID(), type: 'expense', amount: 120, categoryId: 'cat-food', categoryName: 'Food', date: new Date(now.getFullYear(), now.getMonth(), 5).toISOString() },
      { id: crypto.randomUUID(), type: 'expense', amount: 60, categoryId: 'cat-transport', categoryName: 'Transport', date: new Date(now.getFullYear(), now.getMonth(), 6).toISOString() },
      { id: crypto.randomUUID(), type: 'income', amount: 300, categoryId: 'cat-salary', categoryName: 'Salary', date: new Date(now.getFullYear(), now.getMonth(), 15).toISOString(), description: 'Bonus' },
      { id: crypto.randomUUID(), type: 'expense', amount: 90, categoryId: 'cat-entertainment', categoryName: 'Entertainment', date: new Date(now.getFullYear(), now.getMonth(), 18).toISOString() },
    ];
    writeJSON(STORAGE_KEY, list);
  }
}
seed();

export async function listCategories(): Promise<TransactionCategory[]> {
  if (USE_MOCKS) {
    await delay(200);
    return readJSON<TransactionCategory[]>(CATEGORY_KEY, []);
  }
  const { data } = await axios.get('/categories');
  return data;
}

export async function getTransactions(filters?: TransactionFilters): Promise<Transaction[]> {
  if (USE_MOCKS) {
    await delay(200);
    const all = readJSON<Transaction[]>(STORAGE_KEY, []);
    let result = all;
    if (filters?.type && filters.type !== 'all') {
      result = result.filter((t) => t.type === filters.type);
    }
    if (filters?.from) {
      result = result.filter((t) => new Date(t.date) >= new Date(filters.from!));
    }
    if (filters?.to) {
      result = result.filter((t) => new Date(t.date) <= new Date(filters.to!));
    }
    if (filters?.categoryId) {
      result = result.filter((t) => t.categoryId === filters.categoryId);
    }
    return result.sort((a, b) => +new Date(b.date) - +new Date(a.date));
  }
  const { data } = await axios.get('/transactions', { params: filters });
  return data;
}

export async function createTransaction(input: TransactionInput): Promise<Transaction> {
  if (USE_MOCKS) {
    await delay(200);
    const list = readJSON<Transaction[]>(STORAGE_KEY, []);
    const categories = readJSON<TransactionCategory[]>(CATEGORY_KEY, []);
    const category = categories.find((c) => c.id === input.categoryId);
    const item: Transaction = { id: crypto.randomUUID(), categoryName: category?.name || 'Unknown', ...input };
    list.push(item);
    writeJSON(STORAGE_KEY, list);
    return item;
  }
  const { data } = await axios.post('/transactions', input);
  return data;
}

export async function updateTransaction(id: string, input: Partial<TransactionInput>): Promise<Transaction> {
  if (USE_MOCKS) {
    await delay(200);
    const list = readJSON<Transaction[]>(STORAGE_KEY, []);
    const idx = list.findIndex((t) => t.id === id);
    if (idx === -1) throw new Error('Not found');
    const categories = readJSON<TransactionCategory[]>(CATEGORY_KEY, []);
    const next = { ...list[idx], ...input } as Transaction;
    if (input.categoryId) {
      const cat = categories.find((c) => c.id === input.categoryId);
      if (cat) next.categoryName = cat.name;
    }
    list[idx] = next;
    writeJSON(STORAGE_KEY, list);
    return next;
  }
  const { data } = await axios.put(`/transactions/${id}`, input);
  return data;
}

export async function deleteTransaction(id: string): Promise<void> {
  if (USE_MOCKS) {
    await delay(200);
    const list = readJSON<Transaction[]>(STORAGE_KEY, []);
    writeJSON(STORAGE_KEY, list.filter((t) => t.id !== id));
    return;
  }
  await axios.delete(`/transactions/${id}`);
}
