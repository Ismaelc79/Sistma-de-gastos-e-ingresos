export type TransactionType = 'income' | 'expense';

export interface TransactionCategory {
  id: string;
  name: string;
}

export interface Transaction {
  id: string;
  type: TransactionType;
  amount: number;
  categoryId: string;
  categoryName: string;
  description?: string;
  date: string; // ISO string
}

export interface TransactionInput {
  type: TransactionType;
  amount: number;
  categoryId: string;
  description?: string;
  date: string; // ISO string
}

export interface TransactionFilters {
  type?: TransactionType | 'all';
  from?: string; // ISO string
  to?: string;   // ISO string
  categoryId?: string;
}
