import { useEffect, useMemo, useState } from "react";
import { Button, Card, Input } from "../../../shared/ui";
import {
  createTransaction,

  getTransactions,
  listCategories,
} from "../api/transactions.api";
import type {
  Transaction,
  TransactionCategory,
  TransactionInput,
  TransactionType,
} from "../../../shared/types/transaction.types";

export const TransactionsPage = () => {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [categories, setCategories] = useState<TransactionCategory[]>([]);
  const [, setLoading] = useState(false);
  const [form, setForm] = useState<TransactionInput>({
    categoryId: 0,
    name: "",
    description: "",
    amount: 0,
  });
  const [filterType, setFilterType] = useState<"all" | TransactionType>("all");

  useEffect(() => {
    (async () => {
      setLoading(true);
      const [tx, cats] = await Promise.all([
        getTransactions(),
        listCategories(),
      ]);
      setTransactions(tx);
      setCategories(cats);
      setLoading(false);
      if (!form.categoryId && cats[0]) {
        setForm((f) => ({ ...f, categoryId: cats[0].id }));
      }
    })();
  }, []);

  const filtered = useMemo(() => {

    if (filterType === "all") return transactions;
    return transactions.filter((t) => categories.find(f=> f.id == t.categoryId)?.type == filterType);
  }, [transactions, filterType]);

  const totals = useMemo(() => {
    const income = filtered
      .filter((t) => categories.find(f=> f.id == t.categoryId)?.type === "Income")
      .reduce((s, t) => s + t.amount, 0);
    const expense = filtered
      .filter((t) => categories.find(f=> f.id == t.categoryId)?.type === "Expense")
      .reduce((s, t) => s + t.amount, 0);
    return { income, expense, balance: income - expense };
  }, [filtered]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.categoryId || form.amount <= 0 || !form.name) return;
    const created = await createTransaction(form);
    setTransactions((prev) => [created, ...prev]);
    setForm((f) => ({ ...f, amount: 0, description: "", name: "" }));
  };

  // const handleDelete = async (id: string) => {
  //   await deleteTransaction(id);
  //   setTransactions((prev) => prev.filter((t) => t.id !== id));
  // };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-dark-900">Transactions</h1>
          <p className="text-dark-600">
            Add, filter and manage your transactions
          </p>
        </div>
        <div className="flex items-center gap-2">
          <button
            onClick={() => setFilterType("all")}
            className={`px-3 py-1.5 rounded-lg text-sm border ${
              filterType === "all"
                ? "bg-primary-600 text-white border-primary-600"
                : "border-dark-300 text-dark-700 hover:bg-dark-100"
            }`}
          >
            All
          </button>
          <button
            onClick={() => setFilterType("Income")}
            className={`px-3 py-1.5 rounded-lg text-sm border ${
              filterType === "Income"
                ? "bg-green-600 text-white border-green-600"
                : "border-dark-300 text-dark-700 hover:bg-dark-100"
            }`}
          >
            Income
          </button>
          <button
            onClick={() => setFilterType("Expense")}
            className={`px-3 py-1.5 rounded-lg text-sm border ${
              filterType === "Expense"
                ? "bg-red-600 text-white border-red-600"
                : "border-dark-300 text-dark-700 hover:bg-dark-100"
            }`}
          >
            Expense
          </button>
        </div>
      </div>

      {/* Summary */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Card>
          <p className="text-sm text-dark-600">Total Income</p>
          <p className="text-2xl font-bold text-green-600 mt-1">
            ${totals.income.toFixed(2)}
          </p>
        </Card>
        <Card>
          <p className="text-sm text-dark-600">Total Expense</p>
          <p className="text-2xl font-bold text-red-600 mt-1">
            ${totals.expense.toFixed(2)}
          </p>
        </Card>
        <Card>
          <p className="text-sm text-dark-600">Balance</p>
          <p className="text-2xl font-bold text-dark-900 mt-1">
            ${totals.balance.toFixed(2)}
          </p>
        </Card>
      </div>

      {/* Create Form */}
      <Card>
        <form
          onSubmit={handleSubmit}
          className="grid grid-cols-1 md:grid-cols-3 gap-4 items-end"
        >
           <div className="md:col-span-1">
            <Input
              label="Name"
              type="name"
              value={form.name}
              onChange={(e) =>
                setForm((f) => ({ ...f, name: e.target.value }))
              }
              required
            />
          </div>
          <div className="md:col-span-1">
            <Input
              label="Amount"
              type="number"
              value={form.amount}
              onChange={(e) =>
                setForm((f) => ({ ...f, amount: Number(e.target.value) }))
              }
              required
            />
          </div>
          <div className="md:col-span-1">
            <label className="block text-sm font-medium text-dark-700 mb-1">
              Category
            </label>
            <select
              className="w-full px-3 py-2 border border-dark-300 rounded-lg"
              value={form.categoryId}
              onChange={(e) =>
                setForm((f) => ({ ...f, categoryId: parseInt(e.target.value) }))
              }
            >
              {categories.map((c) => (
                <option value={c.id} key={c.id}>
                  {c.name}
                </option>
              ))}
            </select>
          </div>
          {/* <div className="md:col-span-1">
            <Input
              label="Date"
              type="date"
              value={form.date}
              onChange={(e) => setForm((f) => ({ ...f, date: e.target.value }))}
              required
            />
          </div> */}
          <div className="md:col-span-2">
            <Input
              label="Description"
              value={form.description}
              onChange={(e) =>
                setForm((f) => ({ ...f, description: e.target.value }))
              }
            />
          </div>
          <div className="md:col-span-1">
            <Button type="submit" fullWidth>
              Save Transaction
            </Button>
          </div>
        </form>
      </Card>

      {/* List */}
      <Card>
        <div className="overflow-x-auto">
          <table className="min-w-full">
            <thead>
              <tr className="text-left text-sm text-dark-600">
                <th className="py-2">Date</th>
                <th className="py-2">Name</th>
                <th className="py-2">Type</th>
                <th className="py-2">Category</th>
                <th className="py-2">Description</th>
                <th className="py-2 text-right">Amount</th>
                {/* <th className="py-2 text-right">Actions</th> */}
              </tr>
            </thead>
            <tbody>
              {filtered.map((t) => (
                <tr key={t.id} className="border-t border-dark-200 text-sm">
                  <td className="py-2">
                    {new Date(t.createdAt).toLocaleDateString()}
                  </td>
                  <td>{t.name}</td>
                  <td className="py-2">
                    <span
                      className={`px-2 py-1 rounded text-white text-xs ${
                        categories.find(f => f.id == t.categoryId)?.type == "Income" ? "bg-green-600" : "bg-red-600"
                      }`}
                    >
                      {/* {t.type} */}
                    </span>
                  </td>
                  <td className="py-2">{categories.find(f=> f.id == t.categoryId)?.name}</td>
                  <td className="py-2">{t.description || "-"}</td>
                  <td className="py-2 text-right font-semibold">
                    {categories.find(f => f.id == t.categoryId)?.type == "Income" ? "+" : "-"}${t.amount.toFixed(2)}
                  </td>
                  {/* <td className="py-2 text-right">
                    <button
                      onClick={() => handleDelete(t.id)}
                      className="px-3 py-1.5 rounded bg-red-50 text-red-600 hover:bg-red-100"
                    >
                      Delete
                    </button>
                  </td> */}
                </tr>
              ))}
            </tbody>
          </table>
          {filtered.length === 0 && (
            <p className="text-center text-dark-500 py-6">
              No transactions to display
            </p>
          )}
        </div>
      </Card>
    </div>
  );
};
