import { createCategory, getCategories } from './categories.api';

const axiosMock = vi.hoisted(() => ({
  get: vi.fn(),
  post: vi.fn(),
}));

vi.mock('../../../shared/lib/axios', () => ({
  __esModule: true,
  default: axiosMock,
}));

describe('categories api', () => {
  beforeEach(() => {
    axiosMock.get.mockReset();
    axiosMock.post.mockReset();
  });

  it('retrieves categories from backend', async () => {
    const categories = [{ id: 1, name: 'Salary', type: 'Income' }];
    axiosMock.get.mockResolvedValue({ data: categories });

    const result = await getCategories();

    expect(axiosMock.get).toHaveBeenCalledWith('/categories');
    expect(result).toEqual(categories);
  });

  it('creates a category with provided payload', async () => {
    const payload = { name: 'Food', type: 'Expense' };
    const created = { id: 10, ...payload };
    axiosMock.post.mockResolvedValue({ data: created });

    const result = await createCategory(payload);

    expect(axiosMock.post).toHaveBeenCalledWith('/categories', payload);
    expect(result).toEqual(created);
  });
});
