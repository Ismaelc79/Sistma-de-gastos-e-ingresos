import { useEffect, useMemo, useState } from "react";
import { Card } from "../../../shared/ui";
import { getCategories } from "../../categories/api/categories.api";
import { getTransactions } from "../../transactions/api/transactions.api";
import type { Transaction } from "../../../shared/types/transaction.types";
import type { Category } from "../../../shared/types/category.types";
import { Link } from "react-router-dom";
import { getSavingsRateForMonth } from "../../reports/api/reports.api";

export const DashboardPage = () => {
  const [transacciones, setTransacciones] = useState<Transaction[]>([]);
  const [categorias, setCategorias] = useState<Category[]>([]);
  const [savingsRate, setSavingsRate] = useState<number | null>(null);
  const [savingsDelta, setSavingsDelta] = useState<number | null>(null);

  useEffect(() => {
    init();
  }, []);

  const init = async () => {
    setCategorias(await getCategories());
    setTransacciones(await getTransactions());
  };

  useEffect(() => {
    (async () => {
      try {
        const now = new Date();
        const lastMonth = new Date(now.getFullYear(), now.getMonth() - 1, 1);
        const prevMonth = new Date(now.getFullYear(), now.getMonth() - 2, 1);
        const [last, prev] = await Promise.all([
          getSavingsRateForMonth(lastMonth),
          getSavingsRateForMonth(prevMonth),
        ]);
        setSavingsRate(Number(last.rate));
        setSavingsDelta(Number(last.rate) - Number(prev.rate));
      } catch (e) {
        // In case summary API is unreachable, fallback to compute from last month data
        try {
          const now = new Date();
          const start = new Date(now.getFullYear(), now.getMonth() - 1, 1);
          const end = new Date(now.getFullYear(), now.getMonth(), 0, 23, 59, 59, 999);
          const inRange = (d: string) => {
            const dt = new Date(d);
            return dt >= start && dt <= end;
          };
          const income = transacciones
            .filter((t) => inRange(t.createdAt) && categorias.find((c) => c.id === t.categoryId)?.type === "Income")
            .reduce((s, t) => s + t.amount, 0);
          const expense = transacciones
            .filter((t) => inRange(t.createdAt) && categorias.find((c) => c.id === t.categoryId)?.type === "Expense")
            .reduce((s, t) => s + t.amount, 0);
          const rate = income > 0 ? ((income - expense) / income) * 100 : 0;
          setSavingsRate(rate);
          setSavingsDelta(0);
        } catch {
          setSavingsRate(null);
          setSavingsDelta(null);
        }
      }
    })();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [categorias.length, transacciones.length]);

  const recent = useMemo(() => {
    return [...transacciones]
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
      .slice(0, 5);
  }, [transacciones]);

  const totalExpense = useMemo(() =>
    transacciones
      .filter((t) => categorias.find((c) => c.id === t.categoryId)?.type === "Expense")
      .reduce((s, t) => s + t.amount, 0),
    [transacciones, categorias]
  );

  const expenseByCategory = useMemo(() => {
    const map = new Map<number, number>();
    for (const c of categorias.filter((x) => x.type === "Expense")) map.set(c.id, 0);
    for (const t of transacciones) {
      const cat = categorias.find((c) => c.id === t.categoryId);
      if (cat?.type === "Expense") {
        map.set(t.categoryId, (map.get(t.categoryId) || 0) + t.amount);
      }
    }
    return Array.from(map.entries());
  }, [transacciones, categorias]);

  const colorPalette = [
    "#EF4444", "#F59E0B", "#10B981", "#3B82F6", "#8B5CF6", "#06B6D4", "#F43F5E", "#A3E635",
  ];

  const formatDateTime = (iso: string) => new Date(iso).toLocaleString();

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold text-dark-900">Dashboard</h1>
        <p className="text-dark-600 mt-1">
          Welcome back! Here's your financial overview.
        </p>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <Card>
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-dark-600">Total Balance</p>
              <p className="text-2xl font-bold text-dark-900 mt-1">
                $
                {transacciones.reduce((acc, item) => {
                  if (
                    categorias.find((c) => c.id == item.categoryId)?.type ==
                    "Expense"
                  ) {
                    return acc - item.amount;
                  } else {
                    return acc + item.amount;
                  }
                }, 0.0)}
                .00
              </p>
              <p className="text-sm text-green-600 mt-1">
                +12.5% from last month
              </p>
            </div>
            <div className="w-12 h-12 bg-primary-100 rounded-lg flex items-center justify-center">
              <svg
                className="w-6 h-6 text-primary-600"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </div>
          </div>
        </Card>

        <Card>
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-dark-600">Total Income</p>
              <p className="text-2xl font-bold text-dark-900 mt-1">
                $
                {transacciones.reduce((acc, item) => {
                  if (
                    categorias.find((f) => f.id == item.categoryId)?.type ==
                    "Income"
                  ) {
                    return acc + item.amount;
                  }
                  return acc;
                }, 0)}
                .00
              </p>
              <p className="text-sm text-green-600 mt-1">
                +8.2% from last month
              </p>
            </div>
            <div className="w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center">
              <svg
                className="w-6 h-6 text-green-600"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M7 11l5-5m0 0l5 5m-5-5v12"
                />
              </svg>
            </div>
          </div>
        </Card>

        <Card>
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-dark-600">
                Total Expenses
              </p>
              <p className="text-2xl font-bold text-dark-900 mt-1">
                $
                {transacciones.reduce((acc, item) => {
                  if (
                    categorias.find((f) => f.id == item.categoryId)?.type ==
                    "Expense"
                  ) {
                    return acc + item.amount;
                  }
                  return acc;
                }, 0)}
                .00
              </p>
              <p className="text-sm text-red-600 mt-1">+3.1% from last month</p>
            </div>
            <div className="w-12 h-12 bg-red-100 rounded-lg flex items-center justify-center">
              <svg
                className="w-6 h-6 text-red-600"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M17 13l-5 5m0 0l-5-5m5 5V6"
                />
              </svg>
            </div>
          </div>
        </Card>

        <Card>
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-dark-600">Savings Rate</p>
              <p className="text-2xl font-bold text-dark-900 mt-1">
                {savingsRate === null ? '—' : `${savingsRate.toFixed(1)}%`}
              </p>
              {savingsDelta !== null && (
                <p className={`text-sm mt-1 ${savingsDelta >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                  {savingsDelta >= 0 ? '+' : ''}{savingsDelta.toFixed(1)}% from previous month
                </p>
              )}
            </div>
            <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
              <svg
                className="w-6 h-6 text-blue-600"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"
                />
              </svg>
            </div>
          </div>
        </Card>
      </div>

      {/* Recent Transactions */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card>
          <h2 className="text-lg font-semibold text-dark-900 mb-4">
            Recent Transactions
          </h2>
          <div className="space-y-4">
            {recent.map((t, i) => (
              <div key={i} className="flex items-center justify-between">
                <div className="flex items-center space-x-3">
                  <div className="w-10 h-10 bg-primary-100 rounded-lg flex items-center justify-center">
                    <svg
                      className="w-5 h-5 text-primary-600"
                      fill="none"
                      stroke="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"
                      />
                    </svg>
                  </div>
                  <div>
                    <p className="text-sm font-medium text-dark-900">
                      {t.name}
                    </p>
                    <p className="text-xs text-dark-500">{formatDateTime(t.createdAt)}</p>
                  </div>
                </div>
                {categorias.find((c) => c.id == t.categoryId)?.type ==
                "Expense" ? (
                  <p className="text-sm font-semibold text-red-600">
                    -${t.amount}.00
                  </p>
                ) : (
                  <p className="text-sm font-semibold text-green-600">
                    +${t.amount}.00
                  </p>
                )}
              </div>
            ))}
            {transacciones.length > 5 && (
              <div className="pt-2 text-right">
                <Link to="/transactions/history" className="text-primary-600 hover:underline text-sm">
                  Ver histórico de transacciones
                </Link>
              </div>
            )}
          </div>
        </Card>

        <Card>
          <h2 className="text-lg font-semibold text-dark-900 mb-4">
            Spending by Category
          </h2>
          <div className="space-y-3">
            {categorias
              .filter((f) => f.type === 'Expense')
              .map((category, i) => {
                const amount = expenseByCategory.find(([id]) => id === category.id)?.[1] || 0;
                const percent = totalExpense > 0 ? (amount / totalExpense) * 100 : 0;
                const color = colorPalette[i % colorPalette.length];
                return (
                  <div key={category.id}>
                    <div className="flex items-center justify-between mb-1">
                      <span className="text-sm font-medium text-dark-700">{category.name}</span>
                      <span className="text-sm font-semibold text-dark-900">${amount.toFixed(2)}</span>
                    </div>
                    <div className="w-full bg-dark-200 rounded-full h-2">
                      <div className="h-2 rounded-full" style={{ width: `${percent}%`, backgroundColor: color }} />
                    </div>
                  </div>
                );
              })}
          </div>
        </Card>
      </div>
    </div>
  );
};
