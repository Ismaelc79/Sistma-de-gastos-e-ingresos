import { Page, Locator } from '@playwright/test';

export class TransactionsPage {
  readonly page: Page;
  readonly pageTitle: Locator;
  readonly nameInput: Locator;
  readonly amountInput: Locator;
  readonly categorySelect: Locator;
  readonly descriptionInput: Locator;
  readonly saveButton: Locator;
  readonly transactionsTable: Locator;
  readonly filterAllButton: Locator;
  readonly filterIncomeButton: Locator;
  readonly filterExpenseButton: Locator;
  readonly totalIncomeCard: Locator;
  readonly totalExpenseCard: Locator;
  readonly balanceCard: Locator;

  constructor(page: Page) {
    this.page = page;
    this.pageTitle = page.getByRole('heading', { name: /^transactions$/i });
    this.nameInput = page.getByLabel(/name/i).first();
    this.amountInput = page.locator('input[type="number"]').first();
    this.categorySelect = page.locator('select').first();
    this.descriptionInput = page.getByLabel(/description/i).first();
    this.saveButton = page.getByRole('button', { name: /save transaction/i });
    this.transactionsTable = page.locator('table');
    this.filterAllButton = page.getByRole('button', { name: /^all$/i });
    this.filterIncomeButton = page.getByRole('button', { name: /^income$/i });
    this.filterExpenseButton = page.getByRole('button', { name: /^expense$/i });
    this.totalIncomeCard = page.locator('text=Total Income').locator('..');
    this.totalExpenseCard = page.locator('text=Total Expense').locator('..');
    this.balanceCard = page.locator('text=Balance').locator('..');
  }

  async goto() {
    await this.page.goto('/transactions');
  }

  async createTransaction(data: {
    name: string;
    amount: number;
    category?: string;
    description?: string;
  }) {
    await this.nameInput.fill(data.name);
    await this.amountInput.fill(data.amount.toString());
    
    if (data.category) {
      await this.categorySelect.selectOption({ label: data.category });
    }
    
    if (data.description) {
      await this.descriptionInput.fill(data.description);
    }
    
    await this.saveButton.click();
    await this.transactionsTable
      .locator('td')
      .filter({ hasText: data.name })
      .first()
      .waitFor({ timeout: 5000 });
  }

  async filterBy(type: 'all' | 'income' | 'expense') {
    const buttonMap = {
      all: this.filterAllButton,
      income: this.filterIncomeButton,
      expense: this.filterExpenseButton,
    };
    await buttonMap[type].click();
  }

  async getTransactionCount(): Promise<number> {
    const rows = await this.transactionsTable.locator('tbody tr').count();
    return rows;
  }

  async getTotalIncome(): Promise<string> {
    const text = await this.totalIncomeCard.locator('.text-2xl').textContent();
    return text?.trim() || '';
  }

  async getTotalExpense(): Promise<string> {
    const text = await this.totalExpenseCard.locator('.text-2xl').textContent();
    return text?.trim() || '';
  }

  async getBalance(): Promise<string> {
    const text = await this.balanceCard.locator('.text-2xl').textContent();
    return text?.trim() || '';
  }

  async getTransactionByName(name: string): Promise<boolean> {
    const cell = this.transactionsTable.locator(`td:has-text("${name}")`);
    return await cell.count() > 0;
  }
}
