import { useEffect, useState } from "react";
import { Card } from "../../../shared/ui";
import { getCategories } from "../../categories/api/categories.api";
import { getTransactions } from "../../transactions/api/transactions.api";
import type { Transaction } from "../../../shared/types/transaction.types";
import type { Category } from "../../../shared/types/category.types";

export const DashboardPage = () => {
  const [transacciones, setTransacciones] = useState<Transaction[]>([]);
  const [categorias, setCategorias] = useState<Category[]>([]);

  useEffect(() => {
    init();
  }, []);

  const init = async () => {
    setCategorias(await getCategories());
    setTransacciones(await getTransactions());
  };

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
              <p className="text-2xl font-bold text-dark-900 mt-1">43%</p>
              <p className="text-sm text-green-600 mt-1">+5% from last month</p>
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
            {transacciones.map((t, i) => (
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
                    <p className="text-xs text-dark-500">Today, 3:30 PM</p>
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
          </div>
        </Card>

        <Card>
          <h2 className="text-lg font-semibold text-dark-900 mb-4">
            Spending by Category
          </h2>
          <div className="space-y-3">
            {categorias.filter(f=> f.type == 'Expense')
              .map((category,i) => (
                <div key={category.name}>
                  <div className="flex items-center justify-between mb-1">
                    <span className="text-sm font-medium text-dark-700">
                      {category.name}
                    </span>
                    <span className="text-sm font-semibold text-dark-900">
                      ${transacciones.filter(f=> f.categoryId == category.id).reduce((acc,item)=>{
                          return acc + item.amount
                      },0)}
                    </span>
                  </div>
                  <div className="w-full bg-dark-200 rounded-full h-2">
                    <div
                      // className={`${category.color} h-2 rounded-full`}
                       className={`bg-[#50d71${i}] h-2 rounded-full`}
                      // style={{ width: `${category.percent}%` }}
                    />
                  </div>
                </div>
              ))}
          </div>
        </Card>
      </div>
    </div>
  );
};
