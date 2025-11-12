import { test, expect, Page } from '@playwright/test';
import { AuthPage } from './page-objects/auth.page';
import { captureDebugScreenshot } from './utils/debug';

test.describe('Authentication', () => {
  let authPage: AuthPage;

  test.beforeEach(async ({ page }) => {
    authPage = new AuthPage(page);
  });

  test.afterEach(async ({ page }, testInfo) => {
    await captureDebugScreenshot(page, testInfo, `auth-${testInfo.title}`);
  });

  test.describe('Login Page', () => {
    test('should display login form with all elements', async ({ page }) => {
      await authPage.goto('/login');
      
      await expect(page.getByRole('heading', { name: /welcome back/i })).toBeVisible();
      await expect(authPage.emailInput).toBeVisible();
      await expect(authPage.passwordInput).toBeVisible();
      await expect(authPage.submitButton).toBeVisible();
      await expect(page.getByText(/sign in to manage your finances/i)).toBeVisible();
    });

    test('should show validation error for empty email', async ({ page }) => {
      await authPage.goto('/login');
      
      await authPage.passwordInput.fill('password123');
      await authPage.submitButton.click();
      
      // HTML5 validation will prevent submission
      const emailValidity = await authPage.emailInput.evaluate((el: HTMLInputElement) => el.validity.valid);
      expect(emailValidity).toBe(false);
    });

    test('should show validation error for invalid email format', async ({ page }) => {
      await authPage.goto('/login');
      
      await authPage.emailInput.fill('invalid-email');
      await authPage.passwordInput.fill('password123');
      await authPage.submitButton.click();

      const isValid = await authPage.emailInput.evaluate((el: HTMLInputElement) => el.validity.valid);
      expect(isValid).toBe(false);
      const validationMsg = await authPage.emailInput.evaluate((el: HTMLInputElement) => el.validationMessage);
      expect(validationMsg.length).toBeGreaterThan(0);
    });

    test('should show validation error for empty password', async ({ page }) => {
      await authPage.goto('/login');
      
      await authPage.emailInput.fill('testfinance@yopmail.com');
      await authPage.submitButton.click();
      
      const passwordValidity = await authPage.passwordInput.evaluate((el: HTMLInputElement) => el.validity.valid);
      expect(passwordValidity).toBe(false);
    });

    test('should have password toggle functionality', async ({ page }) => {
      await authPage.goto('/login');
      
      await authPage.passwordInput.fill('mypassword');
      
      // Initially password should be hidden
      await expect(authPage.passwordInput).toHaveAttribute('type', 'password');
      
      // Click toggle button
      await authPage.passwordToggle.click();
      
      // Password should now be visible
      await expect(authPage.passwordInput).toHaveAttribute('type', 'text');
    });

    test('should navigate to register page', async ({ page }) => {
      await authPage.goto('/login');
      
      await authPage.createAccountButton.click();
      
      await expect(page).toHaveURL(/.*register/);
    });

    test('should display forgot password link', async ({ page }) => {
      await authPage.goto('/login');
      
      const forgotPasswordLink = page.getByRole('link', { name: /forgot password/i });
      await expect(forgotPasswordLink).toBeVisible();
    });
  });

  test.describe('Register Page', () => {
    test('should display register form with all elements', async ({ page }) => {
      await authPage.goto('/register');
      
      await expect(page.getByRole('heading', { name: /create account/i })).toBeVisible();
      await expect(authPage.emailInput).toBeVisible();
      await expect(authPage.passwordInput).toBeVisible();
      await expect(authPage.submitButton).toBeVisible();
    });

    test('should show validation error for empty fields', async ({ page }) => {
      await authPage.goto('/register');
      
      await authPage.submitButton.click();
      
      const emailValidity = await authPage.emailInput.evaluate((el: HTMLInputElement) => el.validity.valid);
      expect(emailValidity).toBe(false);
    });

    test('should show validation error for invalid email', async ({ page }) => {
      await authPage.goto('/register');
      
      await authPage.emailInput.fill('invalid-email');
      await authPage.passwordInput.fill('password123');
      await authPage.submitButton.click();
      
      const isValid = await authPage.emailInput.evaluate((el: HTMLInputElement) => el.validity.valid);
      expect(isValid).toBe(false);
      const validationMsg = await authPage.emailInput.evaluate((el: HTMLInputElement) => el.validationMessage);
      expect(validationMsg.length).toBeGreaterThan(0);
    });

    test('should navigate to login page', async ({ page }) => {
      await authPage.goto('/register');
      
      const signInLink = page.getByRole('link', { name: /sign in/i });
      await signInLink.click();
      
      await expect(page).toHaveURL(/.*login/);
    });

    test('should have password toggle functionality', async ({ page }) => {
      await authPage.goto('/register');
      
      await authPage.passwordInput.fill('mypassword');
      
      // Initially password should be hidden
      await expect(authPage.passwordInput).toHaveAttribute('type', 'password');
      
      // Click toggle button
      await authPage.passwordToggle.click();
      
      // Password should now be visible
      await expect(authPage.passwordInput).toHaveAttribute('type', 'text');
    });
  });

  test.describe('Protected Routes', () => {
    test('should redirect to login when accessing protected route without auth', async ({ page }) => {
      await page.goto('/dashboard');
      
      // Should redirect to login
      await expect(page).toHaveURL(/.*login/);
    });

    test('should redirect to login when accessing categories without auth', async ({ page }) => {
      await page.goto('/categories');
      
      await expect(page).toHaveURL(/.*login/);
    });

    test('should redirect to login when accessing transactions without auth', async ({ page }) => {
      await page.goto('/transactions');
      
      await expect(page).toHaveURL(/.*login/);
    });

    test('should redirect to login when accessing reports without auth', async ({ page }) => {
      await page.goto('/reports');
      
      await expect(page).toHaveURL(/.*login/);
    });

    test('should redirect to login when accessing profile without auth', async ({ page }) => {
      await page.goto('/profile');
      
      await expect(page).toHaveURL(/.*login/);
    });
  });
});
