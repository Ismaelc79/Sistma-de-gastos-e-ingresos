export type TransactionType = 'Income' | 'Expense';

export interface TransactionCategory {
  id: number;
  name: string;
  type: string;
}

export interface Transaction {
   id: number,
    categoryId: number,
    userId: string,
    name: string,
    description: string,
    amount: number,
    createdAt: string
}

export interface TransactionInput {
  categoryId: number,
  name: string,
  description: string,
  amount: number
}

export interface TransactionFilters {
  type?: TransactionType | 'all';
  from?: string; // ISO string
  to?: string;   // ISO string
  categoryId?: string;
}
