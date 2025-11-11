import { Page, Locator } from '@playwright/test';

export class ProfilePage {
  readonly page: Page;
  readonly pageTitle: Locator;
  readonly nameInput: Locator;
  readonly emailInput: Locator;
  readonly phoneInput: Locator;
  readonly currencySelect: Locator;
  readonly themeSelect: Locator;
  readonly saveButton: Locator;
  readonly successMessage: Locator;

  constructor(page: Page) {
    this.page = page;
    this.pageTitle = page.getByRole('heading', { name: /profile/i });
    this.nameInput = page.locator('input[type="text"]').first();
    this.emailInput = page.locator('input[type="email"]');
    this.phoneInput = page.locator('input[type="tel"]');
    this.currencySelect = page.locator('select').first();
    this.themeSelect = page.locator('select').nth(1);
    this.saveButton = page.getByRole('button', { name: /save/i });
    this.successMessage = page.locator('.bg-green-50, .text-green-600');
  }

  async goto() {
    await this.page.goto('/profile');
  }

  async updateProfile(data: {
    name?: string;
    email?: string;
    phone?: string;
    currency?: string;
    theme?: string;
  }) {
    if (data.name) {
      await this.nameInput.fill(data.name);
    }
    if (data.email) {
      await this.emailInput.fill(data.email);
    }
    if (data.phone) {
      await this.phoneInput.fill(data.phone);
    }
    if (data.currency) {
      await this.currencySelect.selectOption(data.currency);
    }
    if (data.theme) {
      await this.themeSelect.selectOption(data.theme);
    }
    
    await this.saveButton.click();
    await this.page.waitForTimeout(500);
  }

  async getNameValue(): Promise<string> {
    return await this.nameInput.inputValue();
  }

  async getEmailValue(): Promise<string> {
    return await this.emailInput.inputValue();
  }

  async getPhoneValue(): Promise<string> {
    return await this.phoneInput.inputValue();
  }
}