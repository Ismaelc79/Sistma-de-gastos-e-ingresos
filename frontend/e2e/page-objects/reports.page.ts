import { Page, Locator } from '@playwright/test';

export class ReportsPage {
  readonly page: Page;
  readonly pageTitle: Locator;
  readonly incomeVsExpenseChart: Locator;
  readonly spendingByCategoryChart: Locator;
  readonly trendChart: Locator;
  readonly totalIncomeValue: Locator;
  readonly totalExpenseValue: Locator;

  constructor(page: Page) {
    this.page = page;
    this.pageTitle = page.getByRole('heading', { name: /^reports$/i });
    this.incomeVsExpenseChart = page.locator('text=Income vs Expense (Bar)').locator('..');
    this.spendingByCategoryChart = page.locator('text=Spending by Category (Doughnut)').locator('..');
    this.trendChart = page.locator('text=Trend (Line)').locator('..');
    this.totalIncomeValue = page.locator('text=Total Income').locator('..').locator('.text-2xl');
    this.totalExpenseValue = page.locator('text=Total Expense').locator('..').locator('.text-2xl');
  }

  async goto() {
    await this.page.goto('/reports');
  }

  async waitForChartsToLoad() {
    // Wait for canvas elements to be present (charts render on canvas)
    await this.page.waitForSelector('canvas', { timeout: 5000 });
    // Give charts time to render
    await this.page.waitForTimeout(1000);
  }

  async getChartCount(): Promise<number> {
    return await this.page.locator('canvas').count();
  }

  async getTotalIncome(): Promise<string> {
    const text = await this.totalIncomeValue.textContent();
    return text?.trim() || '';
  }

  async getTotalExpense(): Promise<string> {
    const text = await this.totalExpenseValue.textContent();
    return text?.trim() || '';
  }

  async isChartVisible(chartName: 'bar' | 'doughnut' | 'line'): Promise<boolean> {
    const chartMap = {
      bar: this.incomeVsExpenseChart,
      doughnut: this.spendingByCategoryChart,
      line: this.trendChart,
    };
    return await chartMap[chartName].isVisible();
  }
}