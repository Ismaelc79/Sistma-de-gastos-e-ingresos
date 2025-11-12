import { test, expect } from '@playwright/test';
import { CategoriesPage } from './page-objects/categories.page';
import { ensureLoggedIn } from './utils/session';

test.describe('Categories Management', () => {
  let categoriesPage: CategoriesPage;

  test.beforeEach(async ({ page }) => {
    categoriesPage = new CategoriesPage(page);
    await ensureLoggedIn(page);
    await categoriesPage.goto();
  });

  test('should display categories page with title', async ({ page }) => {
    await expect(categoriesPage.pageTitle).toBeVisible();
    await expect(page.getByText(/add and manage your transactions/i)).toBeVisible();
  });

  test('should display category creation form', async () => {
    await expect(categoriesPage.nameInput).toBeVisible();
    await expect(categoriesPage.typeSelect).toBeVisible();
    await expect(categoriesPage.saveButton).toBeVisible();
  });

  test('should display categories table', async () => {
    await expect(categoriesPage.categoriesTable).toBeVisible();
  });

  test('should have Name and Type columns in table', async ({ page }) => {
    await expect(page.getByRole('columnheader', { name: /name/i })).toBeVisible();
    await expect(page.getByRole('columnheader', { name: /type/i })).toBeVisible();
  });

  test('should create a new Income category', async () => {
    const initialCount = await categoriesPage.getCategoryCount();
    
    await categoriesPage.createCategory('Salary E2E', 'Income');
    
    const newCount = await categoriesPage.getCategoryCount();
    expect(newCount).toBe(initialCount + 1);
    
    const exists = await categoriesPage.getCategoryByName('Salary E2E');
    expect(exists).toBe(true);
  });

  test('should create a new Expense category', async () => {
    const initialCount = await categoriesPage.getCategoryCount();
    
    await categoriesPage.createCategory('Food E2E', 'Expense');
    
    const newCount = await categoriesPage.getCategoryCount();
    expect(newCount).toBe(initialCount + 1);
    
    const exists = await categoriesPage.getCategoryByName('Food E2E');
    expect(exists).toBe(true);
  });

  test('should clear form after creating category', async () => {
    await categoriesPage.createCategory('Test Category', 'Income');
    
    const nameValue = await categoriesPage.nameInput.inputValue();
    expect(nameValue).toBe('');
  });

  test('should require name field', async () => {
    await categoriesPage.saveButton.click();
    
    const nameValidity = await categoriesPage.nameInput.evaluate((el: HTMLInputElement) => el.validity.valid);
    expect(nameValidity).toBe(false);
  });

  test('should have Income as default type', async () => {
    const selectedValue = await categoriesPage.typeSelect.inputValue();
    expect(selectedValue).toBe('Income');
  });

  test('should allow switching between Income and Expense types', async () => {
    await categoriesPage.typeSelect.selectOption('Expense');
    let selectedValue = await categoriesPage.typeSelect.inputValue();
    expect(selectedValue).toBe('Expense');
    
    await categoriesPage.typeSelect.selectOption('Income');
    selectedValue = await categoriesPage.typeSelect.inputValue();
    expect(selectedValue).toBe('Income');
  });

  test('should display created categories in table', async () => {
    await categoriesPage.createCategory('Utilities E2E', 'Expense');
    
    const categories = await categoriesPage.getAllCategories();
    const found = categories.find(c => c.name === 'Utilities E2E' && c.type === 'Expense');
    
    expect(found).toBeDefined();
  });

  test('should create multiple categories', async () => {
    const initialCount = await categoriesPage.getCategoryCount();
    
    await categoriesPage.createCategory('Freelance E2E', 'Income');
    await categoriesPage.createCategory('Transport E2E', 'Expense');
    await categoriesPage.createCategory('Investment E2E', 'Income');
    
    const newCount = await categoriesPage.getCategoryCount();
    expect(newCount).toBe(initialCount + 3);
  });

  test.describe('Responsive Design', () => {
    test('should display correctly on mobile viewport', async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
      
      await expect(categoriesPage.pageTitle).toBeVisible();
      await expect(categoriesPage.nameInput).toBeVisible();
      await expect(categoriesPage.saveButton).toBeVisible();
    });

    test('should display correctly on tablet viewport', async ({ page }) => {
      await page.setViewportSize({ width: 768, height: 1024 });
      
      await expect(categoriesPage.pageTitle).toBeVisible();
      await expect(categoriesPage.categoriesTable).toBeVisible();
    });
  });
});
