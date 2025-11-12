import { test, expect } from '@playwright/test';
import { DashboardPage } from './page-objects/dashboard.page';
import { ensureLoggedIn } from './utils/session';

test.describe('Dashboard', () => {
  let dashboardPage: DashboardPage;

  test.beforeEach(async ({ page }) => {
    dashboardPage = new DashboardPage(page);
    await ensureLoggedIn(page);
  });

  test('should display dashboard page title and description', async ({ page }) => {
    await dashboardPage.goto();
    
    await expect(dashboardPage.pageTitle).toBeVisible();
    await expect(page.getByText(/welcome back/i)).toBeVisible();
  });

  test('should display all stat cards', async ({ page }) => {
    await dashboardPage.goto();
    
    await expect(dashboardPage.totalBalanceCard).toBeVisible();
    await expect(dashboardPage.totalIncomeCard).toBeVisible();
    await expect(dashboardPage.totalExpensesCard).toBeVisible();
    await expect(dashboardPage.savingsRateCard).toBeVisible();
  });

  test('should display Total Balance card with value', async () => {
    await dashboardPage.goto();
    
    const balance = await dashboardPage.getTotalBalance();
    expect(balance).toContain('$');
  });

  test('should display Total Income card with value', async () => {
    await dashboardPage.goto();
    
    const income = await dashboardPage.getTotalIncome();
    expect(income).toContain('$');
  });

  test('should display Total Expenses card with value', async () => {
    await dashboardPage.goto();
    
    const expenses = await dashboardPage.getTotalExpenses();
    expect(expenses).toContain('$');
  });

  test('should display Savings Rate card', async ({ page }) => {
    await dashboardPage.goto();
    
    await expect(page.getByText(/savings rate/i)).toBeVisible();
    const rate = await dashboardPage.getSavingsRate();
    // Rate can be "—" or a percentage
    expect(rate.length).toBeGreaterThan(0);
  });

  test('should display Recent Transactions section', async ({ page }) => {
    await dashboardPage.goto();
    
    await expect(page.getByText(/recent transactions/i)).toBeVisible();
  });

  test('should display Spending by Category section', async ({ page }) => {
    await dashboardPage.goto();
    
    await expect(page.getByText(/spending by category/i)).toBeVisible();
  });

  test.describe('Period Filters', () => {
    test('should have all period filter buttons', async ({ page }) => {
      await dashboardPage.goto();
      
      await expect(dashboardPage.periodDayButton).toBeVisible();
      await expect(dashboardPage.periodWeekButton).toBeVisible();
      await expect(dashboardPage.periodMonthButton).toBeVisible();
      await expect(dashboardPage.periodAllButton).toBeVisible();
    });

    test('should select day period filter', async ({ page }) => {
      await dashboardPage.goto();
      
      await dashboardPage.selectPeriod('day');
      
      // Check if button has active state (bg-dark-900 text-white)
      await expect(dashboardPage.periodDayButton).toHaveClass(/bg-dark-900/);
    });

    test('should select week period filter', async ({ page }) => {
      await dashboardPage.goto();
      
      await dashboardPage.selectPeriod('week');
      
      await expect(dashboardPage.periodWeekButton).toHaveClass(/bg-dark-900/);
    });

    test('should select month period filter', async ({ page }) => {
      await dashboardPage.goto();
      
      await dashboardPage.selectPeriod('month');
      
      await expect(dashboardPage.periodMonthButton).toHaveClass(/bg-dark-900/);
    });

    test('should select all period filter', async ({ page }) => {
      await dashboardPage.goto();
      
      await dashboardPage.selectPeriod('all');
      
      await expect(dashboardPage.periodAllButton).toHaveClass(/bg-dark-900/);
    });

    test('should display period balance section', async ({ page }) => {
      await dashboardPage.goto();
      
      await expect(page.getByText(/balance por periodo/i)).toBeVisible();
      await expect(page.getByText(/periodo seleccionado/i)).toBeVisible();
      await expect(page.getByText(/acumulado/i)).toBeVisible();
    });

    test('should show income, expense, and balance for selected period', async ({ page }) => {
      await dashboardPage.goto();
      
      await dashboardPage.selectPeriod('month');
      
      // Check that the period section shows the three values
      const periodSection = page.locator('text=Periodo seleccionado').locator('..');
      await expect(periodSection.getByText(/ingresos/i)).toBeVisible();
      await expect(periodSection.getByText(/gastos/i)).toBeVisible();
      await expect(periodSection.getByText(/balance/i)).toBeVisible();
    });
  });

  test.describe('Navigation', () => {
    test('should navigate to transactions history from recent transactions', async ({ page }) => {
      await dashboardPage.goto();
      
      const historyLink = page.getByRole('link', { name: /ver histórico de transacciones/i });
      
      // Only check if link exists when there are more than 5 transactions
      const linkCount = await historyLink.count();
      if (linkCount > 0) {
        await historyLink.click();
        await expect(page).toHaveURL(/.*transactions\/history/);
      }
    });
  });

  test.describe('Responsive Design', () => {
    test('should display correctly on mobile viewport', async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
      await dashboardPage.goto();
      
      await expect(dashboardPage.pageTitle).toBeVisible();
      await expect(dashboardPage.totalBalanceCard).toBeVisible();
    });

    test('should display correctly on tablet viewport', async ({ page }) => {
      await page.setViewportSize({ width: 768, height: 1024 });
      await dashboardPage.goto();
      
      await expect(dashboardPage.pageTitle).toBeVisible();
      await expect(dashboardPage.totalBalanceCard).toBeVisible();
    });
  });
});
