import { Page, Locator } from '@playwright/test';

export class DashboardPage {
  readonly page: Page;
  readonly pageTitle: Locator;
  readonly totalBalanceCard: Locator;
  readonly totalIncomeCard: Locator;
  readonly totalExpensesCard: Locator;
  readonly savingsRateCard: Locator;
  readonly recentTransactions: Locator;
  readonly periodDayButton: Locator;
  readonly periodWeekButton: Locator;
  readonly periodMonthButton: Locator;
  readonly periodAllButton: Locator;
  readonly spendingByCategory: Locator;

  constructor(page: Page) {
    this.page = page;
    this.pageTitle = page.getByRole('heading', { name: /dashboard/i });
    this.totalBalanceCard = page.locator('text=Total Balance').locator('..');
    this.totalIncomeCard = page.locator('text=Total Income').locator('..');
    this.totalExpensesCard = page.locator('text=Total Expenses').locator('..');
    this.savingsRateCard = page.locator('text=Savings Rate').locator('..');
    this.recentTransactions = page.locator('text=Recent Transactions').locator('..');
    this.periodDayButton = page.getByRole('button', { name: /día/i });
    this.periodWeekButton = page.getByRole('button', { name: /semana/i });
    this.periodMonthButton = page.getByRole('button', { name: /mes/i });
    this.periodAllButton = page.getByRole('button', { name: /acumulado/i });
    this.spendingByCategory = page.locator('text=Spending by Category').locator('..');
  }

  async goto() {
    await this.page.goto('/dashboard');
  }

  async selectPeriod(period: 'day' | 'week' | 'month' | 'all') {
    const buttonMap = {
      day: this.periodDayButton,
      week: this.periodWeekButton,
      month: this.periodMonthButton,
      all: this.periodAllButton,
    };
    await buttonMap[period].click();
  }

  async getTotalBalance(): Promise<string> {
    const text = await this.totalBalanceCard.locator('.text-2xl').textContent();
    return text?.trim() || '';
  }

  async getTotalIncome(): Promise<string> {
    const text = await this.totalIncomeCard.locator('.text-2xl').textContent();
    return text?.trim() || '';
  }

  async getTotalExpenses(): Promise<string> {
    const text = await this.totalExpensesCard.locator('.text-2xl').textContent();
    return text?.trim() || '';
  }

  async getSavingsRate(): Promise<string> {
    const text = await this.savingsRateCard.locator('.text-2xl').textContent();
    return text?.trim() || '';
  }

  async getRecentTransactionsCount(): Promise<number> {
    const items = await this.recentTransactions.locator('.flex.items-center.justify-between').count();
    return items;
  }
}