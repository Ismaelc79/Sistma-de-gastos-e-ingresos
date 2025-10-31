import { useEffect } from 'react';
import { Card } from '../../../shared/ui';
import { getTransactions } from '../../transactions/api/transactions.api';
import type { Transaction } from '../../../shared/types/transaction.types';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  ArcElement,
  Tooltip,
  Legend,
  Filler,
} from 'chart.js';
import { Bar, Doughnut, Line } from 'react-chartjs-2';
import { useState } from 'react';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, BarElement, ArcElement, Tooltip, Legend, Filler);

function toMonthKey(d: string) {
  const date = new Date(d);
  return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
}

export const ReportsPage = () => {
  const [data, setData] = useState<Transaction[]>([]);
  // Removed unused loading state to satisfy noUnusedLocals

  useEffect(() => {
    (async () => {
      const tx = await getTransactions();
      setData(tx);
    })();
  }, []);

  const months = Array.from(new Set(data.map((t) => toMonthKey(t.date)))).sort();
  const byMonthIncome = months.map((m) => data.filter((t) => t.type === 'income' && toMonthKey(t.date) === m).reduce((s, t) => s + t.amount, 0));
  const byMonthExpense = months.map((m) => data.filter((t) => t.type === 'expense' && toMonthKey(t.date) === m).reduce((s, t) => s + t.amount, 0));

  const categories = Array.from(new Set(data.map((t) => t.categoryName)));
  const byCategory = categories.map((c) => data.filter((t) => t.type === 'expense' && t.categoryName === c).reduce((s, t) => s + t.amount, 0));

  const totalIncome = data.filter((t) => t.type === 'income').reduce((s, t) => s + t.amount, 0);
  const totalExpense = data.filter((t) => t.type === 'expense').reduce((s, t) => s + t.amount, 0);

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-dark-900">Reports</h1>
        <p className="text-dark-600">Visualize your finances</p>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card>
          <h2 className="text-lg font-semibold text-dark-900 mb-4">Income vs Expense (Bar)</h2>
          <Bar
            data={{
              labels: months,
              datasets: [
                { label: 'Income', data: byMonthIncome, backgroundColor: 'rgba(34,197,94,0.6)' },
                { label: 'Expense', data: byMonthExpense, backgroundColor: 'rgba(239,68,68,0.6)' },
              ],
            }}
            options={{ responsive: true, maintainAspectRatio: false }}
            height={220}
          />
        </Card>

        <Card>
          <h2 className="text-lg font-semibold text-dark-900 mb-4">Spending by Category (Doughnut)</h2>
          <Doughnut
            data={{
              labels: categories,
              datasets: [
                {
                  label: 'Expenses',
                  data: byCategory,
                  backgroundColor: ['#3b82f6', '#ef4444', '#10b981', '#f59e0b', '#8b5cf6', '#06b6d4'],
                },
              ],
            }}
            options={{ responsive: true, maintainAspectRatio: false }}
            height={220}
          />
        </Card>

        <Card>
          <h2 className="text-lg font-semibold text-dark-900 mb-4">Trend (Line)</h2>
          <Line
            data={{
              labels: months,
              datasets: [
                { label: 'Income', data: byMonthIncome, borderColor: '#22c55e', backgroundColor: 'rgba(34,197,94,0.2)', fill: true },
                { label: 'Expense', data: byMonthExpense, borderColor: '#ef4444', backgroundColor: 'rgba(239,68,68,0.2)', fill: true },
              ],
            }}
            options={{ responsive: true, maintainAspectRatio: false }}
            height={260}
          />
        </Card>

        <Card>
          <div className="grid grid-cols-2 gap-6">
            <div>
              <p className="text-sm text-dark-600">Total Income</p>
              <p className="text-2xl font-bold text-green-600 mt-1">${totalIncome.toFixed(2)}</p>
            </div>
            <div>
              <p className="text-sm text-dark-600">Total Expense</p>
              <p className="text-2xl font-bold text-red-600 mt-1">${totalExpense.toFixed(2)}</p>
            </div>
          </div>
        </Card>
      </div>
    </div>
  );
};
