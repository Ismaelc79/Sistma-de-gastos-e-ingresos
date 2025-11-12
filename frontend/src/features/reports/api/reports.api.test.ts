import { getCategorySummary, getSavingsRateForMonth } from './reports.api';

const axiosMock = vi.hoisted(() => ({
  get: vi.fn(),
}));

vi.mock('../../../shared/lib/axios', () => ({
  __esModule: true,
  default: axiosMock,
}));

describe('reports api', () => {
  beforeEach(() => {
    axiosMock.get.mockReset();
  });

  it('requests summary with the correct params', async () => {
    const summary = { totalAmount: 1000 };
    axiosMock.get.mockResolvedValue({ data: summary });
    const start = new Date('2024-01-01T00:00:00Z');
    const end = new Date('2024-01-31T23:59:59Z');

    const result = await getCategorySummary('Income', start, end);

    expect(axiosMock.get).toHaveBeenCalledWith('/reports/summary', {
      params: {
        categoryType: 'Income',
        startDate: start.toISOString(),
        endDate: end.toISOString(),
      },
    });
    expect(result).toEqual(summary);
  });

  it('computes savings rate for the provided month', async () => {
    axiosMock.get.mockResolvedValueOnce({ data: { totalAmount: 200 } });
    axiosMock.get.mockResolvedValueOnce({ data: { totalAmount: 50 } });

    const result = await getSavingsRateForMonth(new Date('2024-02-15'));

    expect(axiosMock.get).toHaveBeenNthCalledWith(
      1,
      '/reports/summary',
      expect.objectContaining({ params: expect.objectContaining({ categoryType: 'Income' }) })
    );
    expect(axiosMock.get).toHaveBeenNthCalledWith(
      2,
      '/reports/summary',
      expect.objectContaining({ params: expect.objectContaining({ categoryType: 'Expense' }) })
    );
    expect(result.rate).toBeCloseTo(75);
    expect(result.income).toBe(200);
    expect(result.expense).toBe(50);
  });

  it('returns zero rate when there is no income', async () => {
    axiosMock.get.mockResolvedValueOnce({ data: { totalAmount: 0 } });
    axiosMock.get.mockResolvedValueOnce({ data: { totalAmount: 120 } });

    const result = await getSavingsRateForMonth(new Date('2024-05-01'));

    expect(result.rate).toBe(0);
    expect(result.income).toBe(0);
    expect(result.expense).toBe(120);
  });
});
