import type { SummaryReportDto } from '../../../shared/types/summary.types';
import axiosInstance from '../../../shared/lib/axios';

export async function getCategorySummary(
  categoryType: 'Income' | 'Expense',
  startDate: Date,
  endDate: Date
): Promise<SummaryReportDto> {
  const { data } = await axiosInstance.get('/reports/summary', {
    params: {
      categoryType,
      startDate: startDate.toISOString(),
      endDate: endDate.toISOString(),
    },
  });
  return data;
}

export async function getSavingsRateForMonth(date: Date): Promise<{ rate: number; income: number; expense: number }>
{
  const start = new Date(date.getFullYear(), date.getMonth(), 1);
  const end = new Date(date.getFullYear(), date.getMonth() + 1, 0, 23, 59, 59, 999);
  const [incomeSummary, expenseSummary] = await Promise.all([
    getCategorySummary('Income', start, end),
    getCategorySummary('Expense', start, end),
  ]);
  const income = Number(incomeSummary?.totalAmount ?? 0);
  const expense = Number(expenseSummary?.totalAmount ?? 0);
  const rate = income > 0 ? ((income - expense) / income) * 100 : 0;
  return { rate, income, expense };
}
