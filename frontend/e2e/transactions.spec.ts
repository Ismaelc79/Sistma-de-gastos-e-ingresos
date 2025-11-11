import { test, expect } from '@playwright/test';
import { TransactionsPage } from './page-objects/transactions.page';

test.describe('Transactions Management', () => {
  let transactionsPage: TransactionsPage;

  test.beforeEach(async ({ page }) => {
    transactionsPage = new TransactionsPage(page);
    await transactionsPage.goto();
  });

  test('should display transactions page with title', async ({ page }) => {
    await expect(transactionsPage.pageTitle).toBeVisible();
    await expect(page.getByText(/add, filter and manage your transactions/i)).toBeVisible();
  });

  test('should display transaction creation form', async () => {
    await expect(transactionsPage.nameInput).toBeVisible();
    await expect(transactionsPage.amountInput).toBeVisible();
    await expect(transactionsPage.categorySelect).toBeVisible();
    await expect(transactionsPage.descriptionInput).toBeVisible();
    await expect(transactionsPage.saveButton).toBeVisible();
  });

  test('should display filter buttons', async () => {
    await expect(transactionsPage.filterAllButton).toBeVisible();
    await expect(transactionsPage.filterIncomeButton).toBeVisible();
    await expect(transactionsPage.filterExpenseButton).toBeVisible();
  });

  test('should display summary cards', async () => {
    await expect(transactionsPage.totalIncomeCard).toBeVisible();
    await expect(transactionsPage.totalExpenseCard).toBeVisible();
    await expect(transactionsPage.balanceCard).toBeVisible();
  });

  test('should display transactions table', async () => {
    await expect(transactionsPage.transactionsTable).toBeVisible();
  });

  test('should have all required table columns', async ({ page }) => {
    await expect(page.getByRole('columnheader', { name: /date/i })).toBeVisible();
    await expect(page.getByRole('columnheader', { name: /name/i })).toBeVisible();
    await expect(page.getByRole('columnheader', { name: /type/i })).toBeVisible();
    await expect(page.getByRole('columnheader', { name: /category/i })).toBeVisible();
    await expect(page.getByRole('columnheader', { name: /description/i })).toBeVisible();
    await expect(page.getByRole('columnheader', { name: /amount/i })).toBeVisible();
  });

  test('should create a new transaction', async () => {
    const initialCount = await transactionsPage.getTransactionCount();
    
    await transactionsPage.createTransaction({
      name: 'Test Transaction E2E',
      amount: 100,
      description: 'E2E test transaction',
    });
    
    const newCount = await transactionsPage.getTransactionCount();
    expect(newCount).toBe(initialCount + 1);
  });

  test('should require name field', async () => {
    await transactionsPage.amountInput.fill('100');
    await transactionsPage.saveButton.click();
    
    const nameValidity = await transactionsPage.nameInput.evaluate((el: HTMLInputElement) => el.validity.valid);
    expect(nameValidity).toBe(false);
  });

  test('should require amount field', async () => {
    await transactionsPage.nameInput.fill('Test');
    await transactionsPage.saveButton.click();
    
    const amountValidity = await transactionsPage.amountInput.evaluate((el: HTMLInputElement) => el.validity.valid);
    expect(amountValidity).toBe(false);
  });

  test('should clear form after creating transaction', async () => {
    await transactionsPage.createTransaction({
      name: 'Clear Test',
      amount: 50,
      description: 'Should clear',
    });
    
    const nameValue = await transactionsPage.nameInput.inputValue();
    const amountValue = await transactionsPage.amountInput.inputValue();
    const descValue = await transactionsPage.descriptionInput.inputValue();
    
    expect(nameValue).toBe('');
    expect(amountValue).toBe('0');
    expect(descValue).toBe('');
  });

  test.describe('Filtering', () => {
    test('should filter by All transactions', async ({ page }) => {
      await transactionsPage.filterBy('all');
      
      await expect(transactionsPage.filterAllButton).toHaveClass(/bg-primary-600/);
    });

    test('should filter by Income transactions', async ({ page }) => {
      await transactionsPage.filterBy('income');
      
      await expect(transactionsPage.filterIncomeButton).toHaveClass(/bg-green-600/);
    });

    test('should filter by Expense transactions', async ({ page }) => {
      await transactionsPage.filterBy('expense');
      
      await expect(transactionsPage.filterExpenseButton).toHaveClass(/bg-red-600/);
    });

    test('should update transaction count when filtering', async () => {
      const allCount = await transactionsPage.getTransactionCount();
      
      await transactionsPage.filterBy('income');
      const incomeCount = await transactionsPage.getTransactionCount();
      
      await transactionsPage.filterBy('expense');
      const expenseCount = await transactionsPage.getTransactionCount();
      
      // At least one filter should show different count (unless no data)
      const countsAreDifferent = 
        incomeCount !== allCount || 
        expenseCount !== allCount || 
        allCount === 0;
      
      expect(countsAreDifferent).toBe(true);
    });
  });

  test.describe('Summary Cards', () => {
    test('should display Total Income with currency', async () => {
      const income = await transactionsPage.getTotalIncome();
      expect(income).toContain('$');
    });

    test('should display Total Expense with currency', async () => {
      const expense = await transactionsPage.getTotalExpense();
      expect(expense).toContain('$');
    });

    test('should display Balance with currency', async () => {
      const balance = await transactionsPage.getBalance();
      expect(balance).toContain('$');
    });
  });

  test.describe('Transaction Creation with Different Data', () => {
    test('should create income transaction', async () => {
      await transactionsPage.createTransaction({
        name: 'Salary Payment',
        amount: 5000,
        description: 'Monthly salary',
      });
      
      const exists = await transactionsPage.getTransactionByName('Salary Payment');
      expect(exists).toBe(true);
    });

    test('should create expense transaction', async () => {
      await transactionsPage.createTransaction({
        name: 'Grocery Shopping',
        amount: 150,
        description: 'Weekly groceries',
      });
      
      const exists = await transactionsPage.getTransactionByName('Grocery Shopping');
      expect(exists).toBe(true);
    });

    test('should create transaction without description', async () => {
      await transactionsPage.createTransaction({
        name: 'Quick Purchase',
        amount: 25,
      });
      
      const exists = await transactionsPage.getTransactionByName('Quick Purchase');
      expect(exists).toBe(true);
    });

    test('should create multiple transactions', async () => {
      const initialCount = await transactionsPage.getTransactionCount();
      
      await transactionsPage.createTransaction({
        name: 'Transaction 1',
        amount: 100,
      });
      
      await transactionsPage.createTransaction({
        name: 'Transaction 2',
        amount: 200,
      });
      
      await transactionsPage.createTransaction({
        name: 'Transaction 3',
        amount: 300,
      });
      
      const newCount = await transactionsPage.getTransactionCount();
      expect(newCount).toBe(initialCount + 3);
    });
  });

  test.describe('Responsive Design', () => {
    test('should display correctly on mobile viewport', async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
      
      await expect(transactionsPage.pageTitle).toBeVisible();
      await expect(transactionsPage.nameInput).toBeVisible();
      await expect(transactionsPage.saveButton).toBeVisible();
    });

    test('should display correctly on tablet viewport', async ({ page }) => {
      await page.setViewportSize({ width: 768, height: 1024 });
      
      await expect(transactionsPage.pageTitle).toBeVisible();
      await expect(transactionsPage.transactionsTable).toBeVisible();
    });
  });
});