import { createTransaction, getTransactions, listCategories } from './transactions.api';

const axiosMock = vi.hoisted(() => ({
  get: vi.fn(),
  post: vi.fn(),
}));

vi.mock('../../../shared/lib/axios', () => ({
  __esModule: true,
  default: axiosMock,
}));

describe('transactions api', () => {
  beforeEach(() => {
    axiosMock.get.mockReset();
    axiosMock.post.mockReset();
  });

  it('lists transaction categories', async () => {
    const data = [{ id: 1, name: 'Salary', type: 'Income' }];
    axiosMock.get.mockResolvedValue({ data });

    const result = await listCategories();

    expect(axiosMock.get).toHaveBeenCalledWith('/categories');
    expect(result).toEqual(data);
  });

  it('fetches transactions with filters', async () => {
    const transactions = [{ id: 1, amount: 100 }];
    axiosMock.get.mockResolvedValue({ data: transactions });

    const filters = { type: 'Income', categoryId: 2 };
    const result = await getTransactions(filters as any);

    expect(axiosMock.get).toHaveBeenCalledWith('/transactions', { params: filters });
    expect(result).toEqual(transactions);
  });

  it('creates a transaction', async () => {
    const payload = { amount: 50, categoryId: 1 };
    const created = { id: 5, ...payload };
    axiosMock.post.mockResolvedValue({ data: created });

    const result = await createTransaction(payload as any);

    expect(axiosMock.post).toHaveBeenCalledWith('/transactions', payload);
    expect(result).toEqual(created);
  });
});
