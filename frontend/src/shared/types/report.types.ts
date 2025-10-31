export interface BalanceSummary {
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

export interface CategorySummary {
  categoryId: string;
  categoryName: string;
  amount: number; // expense as negative, income as positive or separated
}

export interface MonthlyPoint {
  month: string; // e.g., '2025-01'
  income: number;
  expense: number;
}
