import { Page, Locator } from '@playwright/test';

export class AuthPage {
  readonly page: Page;
  readonly emailInput: Locator;
  readonly passwordInput: Locator;
  readonly submitButton: Locator;
  readonly errorMessage: Locator;
  readonly createAccountButton: Locator;
  readonly loginLink: Locator;
  readonly passwordToggle: Locator;

  constructor(page: Page) {
    this.page = page;
    this.emailInput = page.getByLabel(/email address/i).first();
    this.passwordInput = page.locator('input[name="password"]').first();
    this.submitButton = page.locator('button[type="submit"]');
    this.errorMessage = page.locator('.bg-red-50');
    this.createAccountButton = page.getByRole('button', { name: /create account/i });
    this.loginLink = page.getByRole('link', { name: /sign in/i });
    this.passwordToggle = page.getByTestId('password-toggle').first();
  }

  async goto(path: '/login' | '/register') {
    await this.page.goto(path);
  }

  async login(email: string, password: string) {
    await this.goto('/login');
    await this.emailInput.fill(email);
    await this.passwordInput.fill(password);
    await this.submitButton.click();
  }

  async register(email: string, password: string) {
    await this.goto('/register');
    await this.emailInput.fill(email);
    await this.passwordInput.fill(password);
    await this.submitButton.click();
  }

  async waitForDashboard() {
    await this.page.waitForURL('**/dashboard', { timeout: 10000 });
  }

  async isLoggedIn(): Promise<boolean> {
    try {
      await this.page.waitForURL('**/dashboard', { timeout: 5000 });
      return true;
    } catch {
      return false;
    }
  }
}
