import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { CategoriesPages } from './CategoriesPages';

const mockGetCategories = vi.fn();
const mockCreateCategory = vi.fn();

vi.mock('../api/categories.api', () => ({
  getCategories: () => mockGetCategories(),
  createCategory: (input: unknown) => mockCreateCategory(input),
}));

describe('CategoriesPages', () => {
  beforeEach(() => {
    mockGetCategories.mockReset();
    mockCreateCategory.mockReset();
  });

  it('loads and displays categories fetched from API', async () => {
    mockGetCategories.mockResolvedValue([
      { id: 1, name: 'Salary', type: 'Income' },
      { id: 2, name: 'Groceries', type: 'Expense' },
    ]);

    render(<CategoriesPages />);

    expect(await screen.findByText('Salary')).toBeInTheDocument();
    expect(screen.getByText('Groceries')).toBeInTheDocument();
  });

  it('submits the form and appends the newly created category', async () => {
    const user = userEvent.setup();
    mockGetCategories.mockResolvedValue([]);
    mockCreateCategory.mockResolvedValue({ id: 99, name: 'Bonus', type: 'Income' });

    render(<CategoriesPages />);

    const nameInput = await screen.findByLabelText(/name/i);
    await user.clear(nameInput);
    await user.type(nameInput, 'Bonus');

    const typeSelect = screen.getByRole('combobox');
    await user.selectOptions(typeSelect, 'Income');

    await user.click(screen.getByRole('button', { name: /save category/i }));

    await waitFor(() => {
      expect(mockCreateCategory).toHaveBeenCalledWith({ name: 'Bonus', type: 'Income' });
    });

    expect(await screen.findByText('Bonus')).toBeInTheDocument();
    expect((nameInput as HTMLInputElement).value).toBe('');
  });
});
