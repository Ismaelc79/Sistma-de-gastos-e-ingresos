import { Page, Locator } from '@playwright/test';

export class CategoriesPage {
  readonly page: Page;
  readonly pageTitle: Locator;
  readonly nameInput: Locator;
  readonly typeSelect: Locator;
  readonly saveButton: Locator;
  readonly categoriesTable: Locator;

  constructor(page: Page) {
    this.page = page;
    this.pageTitle = page.getByRole('heading', { name: /transactions/i }).first();
    this.nameInput = page.locator('input[type="text"]').first();
    this.typeSelect = page.locator('select').first();
    this.saveButton = page.getByRole('button', { name: /save category/i });
    this.categoriesTable = page.locator('table');
  }

  async goto() {
    await this.page.goto('/categories');
  }

  async createCategory(name: string, type: 'Income' | 'Expense') {
    await this.nameInput.fill(name);
    await this.typeSelect.selectOption(type);
    await this.saveButton.click();
    await this.categoriesTable
      .locator('td')
      .filter({ hasText: name })
      .first()
      .waitFor({ timeout: 5000 });
  }

  async getCategoryCount(): Promise<number> {
    const rows = await this.categoriesTable.locator('tbody tr').count();
    return rows;
  }

  async getCategoryByName(name: string): Promise<boolean> {
    const cell = this.categoriesTable.locator(`td:has-text("${name}")`);
    return await cell.count() > 0;
  }

  async getAllCategories(): Promise<Array<{ name: string; type: string }>> {
    const rows = await this.categoriesTable.locator('tbody tr').all();
    const categories = [];
    
    for (const row of rows) {
      const cells = await row.locator('td').allTextContents();
      if (cells.length >= 2) {
        categories.push({
          name: cells[0].trim(),
          type: cells[1].trim(),
        });
      }
    }
    
    return categories;
  }
}
