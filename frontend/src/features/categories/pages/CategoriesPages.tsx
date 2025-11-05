import { useEffect, useState } from "react";
import { Button, Card, Input } from "../../../shared/ui";
import type { Category, CategoryInput } from "../../../shared/types/category.types";
import { createCategory, getCategories } from "../api/categories.api";

export const CategoriesPages = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [types] = useState(["Income", "Expense"]);

  //   const [categories, setCategories] = useState<TransactionCategory[]>([]);
  const [, setLoading] = useState(false);
  const [form, setForm] = useState<CategoryInput>({
    name: "",
    type: types[0],
  });
  //   const [filterType, setFilterType] = useState<'all' | TransactionType>('all');

  useEffect(() => {
    (async () => {
      setLoading(true);

      const cats = await getCategories();

      setCategories(cats);
      setLoading(false);
    })();
  }, []);

  //   const filtered = useMemo(() => {
  //     if (filterType === 'all') return transactions;
  //     return transactions.filter((t) => t.type === filterType);
  //   }, [transactions, filterType]);

  //   const totals = useMemo(() => {
  //     const income = filtered.filter((t) => t.type === 'income').reduce((s, t) => s + t.amount, 0);
  //     const expense = filtered.filter((t) => t.type === 'expense').reduce((s, t) => s + t.amount, 0);
  //     return { income, expense, balance: income - expense };
  //   }, [filtered]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.name || !form.type) return;
    const created = await createCategory(form);

    setCategories((s) => [...s, created]);

    setForm((f) => ({ ...f, name: "", type: types[0] }));
  };

  //   const handleDelete = async (id: string) => {
  //     await deleteTransaction(id);
  //     setTransactions((prev) => prev.filter((t) => t.id !== id));
  //   };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-dark-900">Transactions</h1>
          <p className="text-dark-600">Add and manage your transactions</p>
        </div>
      </div>

      {/* Summary */}
      {/* <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Card>
          <p className="text-sm text-dark-600">Total Income</p>
          <p className="text-2xl font-bold text-green-600 mt-1">${totals.income.toFixed(2)}</p>
        </Card>
        <Card>
          <p className="text-sm text-dark-600">Total Expense</p>
          <p className="text-2xl font-bold text-red-600 mt-1">${totals.expense.toFixed(2)}</p>
        </Card>
        <Card>
          <p className="text-sm text-dark-600">Balance</p>
          <p className="text-2xl font-bold text-dark-900 mt-1">${totals.balance.toFixed(2)}</p>
        </Card>
      </div> */}

      {/* Create Form */}
      <Card>
        <form
          onSubmit={handleSubmit}
          className="grid grid-cols-1 md:grid-cols-6 gap-4 items-end"
        >
          <div className="md:col-span-1">
            <Input
              label="Name"
              type="text"
              value={form.name}
              onChange={(e) => setForm((f) => ({ ...f, name: e.target.value }))}
              required
            />
          </div>

          <div className="md:col-span-1">
            {/* <Input
              label="Type"
              type="text"
              value={form.type}
              onChange={(e) => setForm((f) => ({ ...f, type: e.target.value }))}
              required
            /> */}
            <label className="block text-sm font-medium text-dark-700 mb-1">
              Type
            </label>
            <select
              required
              onChange={(e) => setForm((f) => ({ ...f, type: e.target.value }))}
              value={form.type}
              className="w-full px-3 py-2 border border-dark-300 rounded-lg"
            >
              {types.map((t) => (
                <option key={t} value={t}>{t}</option>
              ))}
            </select>
          </div>

          <div className="md:col-span-2">
            <Button type="submit" fullWidth>
              Save Category
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
                <th className="py-2">Name</th>
                <th className="py-2">Type</th>
              </tr>
            </thead>
            <tbody>
              {categories.map((t) => (
                <tr key={t.id} className="border-t border-dark-200 text-sm">
                  <td className="py-2">{t.name}</td>
                  <td className="py-2">{t.type || "-"}</td>
                </tr>
              ))}
            </tbody>
          </table>
          {/* {filtered.length === 0 && (
            <p className="text-center text-dark-500 py-6">No transactions to display</p>
          )} */}
        </div>
      </Card>
    </div>
  );
};
