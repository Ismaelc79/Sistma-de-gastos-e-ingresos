import { useEffect, useMemo, useState } from 'react';
import { Card, Button } from '../../../shared/ui';
import { getTransactions, listCategories } from '../api/transactions.api';
import type { Transaction, TransactionCategory } from '../../../shared/types/transaction.types';

function toCSV(rows: string[][]) {
  return rows.map(r => r.map(v => '"' + String(v).replaceAll('"', '""') + '"').join(',')).join('\n');
}

export const TransactionsHistoryPage = () => {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [categories, setCategories] = useState<TransactionCategory[]>([]);

  useEffect(() => {
    (async () => {
      const [tx, cats] = await Promise.all([getTransactions(), listCategories()]);
      setTransactions(tx);
      setCategories(cats);
    })();
  }, []);

  const threeMonthsAgo = useMemo(() => {
    const d = new Date();
    d.setMonth(d.getMonth() - 3);
    d.setHours(0, 0, 0, 0);
    return d;
  }, []);

  const filtered = useMemo(() => {
    return transactions
      .filter(t => new Date(t.createdAt) >= threeMonthsAgo)
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
  }, [transactions, threeMonthsAgo]);

  const exportCSV = () => {
    const rows: string[][] = [];
    rows.push(['Date', 'Name', 'Category', 'Type', 'Description', 'Amount']);
    for (const t of filtered) {
      const cat = categories.find(c => c.id === t.categoryId);
      const type = cat?.type || '';
      rows.push([
        new Date(t.createdAt).toISOString(),
        t.name,
        cat?.name || '',
        type,
        t.description || '',
        String(t.amount),
      ]);
    }
    const csv = toCSV(rows);
    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `transactions_last_3_months.csv`;
    a.click();
    URL.revokeObjectURL(url);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-dark-900">Transactions History</h1>
          <p className="text-dark-600">Últimos 3 meses</p>
        </div>
        <Button onClick={exportCSV}>Exportar CSV</Button>
      </div>

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
              </tr>
            </thead>
            <tbody>
              {filtered.map((t) => {
                const cat = categories.find(c => c.id === t.categoryId);
                const income = cat?.type === 'Income';
                return (
                  <tr key={t.id} className="border-t border-dark-200 text-sm">
                    <td className="py-2">{new Date(t.createdAt).toLocaleString()}</td>
                    <td>{t.name}</td>
                    <td className="py-2">
                      <span className={`px-2 py-1 rounded text-white text-xs ${income ? 'bg-green-600' : 'bg-red-600'}`}>
                        {income ? 'Income' : 'Expense'}
                      </span>
                    </td>
                    <td className="py-2">{cat?.name || '-'}</td>
                    <td className="py-2">{t.description || '-'}</td>
                    <td className={`py-2 text-right font-semibold ${income ? 'text-green-600' : 'text-red-600'}`}>
                      {income ? '+' : '-'}${t.amount.toFixed(2)}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
          {filtered.length === 0 && (
            <p className="text-center text-dark-500 py-6">No hay transacciones en los últimos 3 meses</p>
          )}
        </div>
      </Card>
    </div>
  );
};

