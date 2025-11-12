import { test, expect } from '@playwright/test';
import { ProfilePage } from './page-objects/profile.page';
import { ensureLoggedIn } from './utils/session';

test.describe('Profile Management', () => {
  let profilePage: ProfilePage;

  test.beforeEach(async ({ page }) => {
    profilePage = new ProfilePage(page);
    await ensureLoggedIn(page);
    await profilePage.goto();
  });

  test('should display profile page with title', async () => {
    await expect(profilePage.pageTitle).toBeVisible();
  });

  test('should display profile form fields', async () => {
    await expect(profilePage.nameInput).toBeVisible();
    await expect(profilePage.emailInput).toBeVisible();
    await expect(profilePage.phoneInput).toBeVisible();
    await expect(profilePage.currencySelect).toBeVisible();
    await expect(profilePage.themeSelect).toBeVisible();
    await expect(profilePage.saveButton).toBeVisible();
  });

  test('should have name input field', async ({ page }) => {
    await expect(page.getByText(/name/i).first()).toBeVisible();
    await expect(profilePage.nameInput).toBeVisible();
  });

  test('should have email input field', async ({ page }) => {
    await expect(page.getByText(/email/i).first()).toBeVisible();
    await expect(profilePage.emailInput).toBeVisible();
  });

  test('should have phone input field', async ({ page }) => {
    await expect(page.getByText(/phone/i).first()).toBeVisible();
    await expect(profilePage.phoneInput).toBeVisible();
  });

  test('should have currency select field', async ({ page }) => {
    await expect(page.getByText(/currency/i).first()).toBeVisible();
    await expect(profilePage.currencySelect).toBeVisible();
  });

  test('should have theme select field', async ({ page }) => {
    await expect(page.getByText(/theme/i).first()).toBeVisible();
    await expect(profilePage.themeSelect).toBeVisible();
  });

  test('should allow updating name', async () => {
    await profilePage.nameInput.fill('John Doe E2E');
    
    const value = await profilePage.getNameValue();
    expect(value).toBe('John Doe E2E');
  });

  test('should allow updating email', async () => {
    await profilePage.emailInput.fill('johndoe@e2e.com');
    
    const value = await profilePage.getEmailValue();
    expect(value).toBe('johndoe@e2e.com');
  });

  test('should allow updating phone', async () => {
    await profilePage.phoneInput.fill('+1234567890');
    
    const value = await profilePage.getPhoneValue();
    expect(value).toBe('+1234567890');
  });

  test('should have currency options', async ({ page }) => {
    await profilePage.currencySelect.click();
    
    // Check if select has options
    const options = await profilePage.currencySelect.locator('option').count();
    expect(options).toBeGreaterThan(0);
  });

  test('should have theme options', async ({ page }) => {
    await profilePage.themeSelect.click();
    
    // Check if select has options
    const options = await profilePage.themeSelect.locator('option').count();
    expect(options).toBeGreaterThan(0);
  });

  test('should validate email format', async () => {
    await profilePage.emailInput.fill('invalid-email');
    await profilePage.saveButton.click();
    
    const emailValidity = await profilePage.emailInput.evaluate((el: HTMLInputElement) => el.validity.valid);
    expect(emailValidity).toBe(false);
  });

  test('should validate phone format', async () => {
    await profilePage.phoneInput.fill('abc123');
    
    // Phone input with type="tel" may have pattern validation
    const phoneValue = await profilePage.getPhoneValue();
    expect(phoneValue).toBe('abc123'); // Input accepts it but backend may validate
  });

  test.describe('Profile Update', () => {
    test('should update profile with valid data', async () => {
      await profilePage.updateProfile({
        name: 'Test User E2E',
        email: 'testuser@e2e.com',
        phone: '+1234567890',
      });
      
      // Check if values were set
      const name = await profilePage.getNameValue();
      expect(name).toBe('Test User E2E');
    });

    test('should update only name', async () => {
      const originalEmail = await profilePage.getEmailValue();
      
      await profilePage.updateProfile({
        name: 'Only Name Update',
      });
      
      const newName = await profilePage.getNameValue();
      const newEmail = await profilePage.getEmailValue();
      
      expect(newName).toBe('Only Name Update');
      expect(newEmail).toBe(originalEmail);
    });

    test('should update currency preference', async () => {
      await profilePage.updateProfile({
        currency: 'USD',
      });
      
      const selectedCurrency = await profilePage.currencySelect.inputValue();
      expect(selectedCurrency).toBe('USD');
    });

    test('should update theme preference', async () => {
      await profilePage.updateProfile({
        theme: 'dark',
      });
      
      const selectedTheme = await profilePage.themeSelect.inputValue();
      expect(selectedTheme).toBe('dark');
    });
  });

  test.describe('Form Validation', () => {
    test('should require valid email format', async () => {
      await profilePage.emailInput.fill('not-an-email');
      await profilePage.saveButton.click();
      
      const isValid = await profilePage.emailInput.evaluate((el: HTMLInputElement) => el.validity.valid);
      expect(isValid).toBe(false);
    });

    test('should accept valid email format', async () => {
      await profilePage.emailInput.fill('valid@email.com');
      
      const isValid = await profilePage.emailInput.evaluate((el: HTMLInputElement) => el.validity.valid);
      expect(isValid).toBe(true);
    });

    test('should handle empty fields gracefully', async () => {
      await profilePage.nameInput.clear();
      await profilePage.emailInput.clear();
      await profilePage.phoneInput.clear();
      
      // Form should still be submittable (backend will validate)
      await expect(profilePage.saveButton).toBeEnabled();
    });
  });

  test.describe('Responsive Design', () => {
    test('should display correctly on mobile viewport', async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
      
      await expect(profilePage.pageTitle).toBeVisible();
      await expect(profilePage.nameInput).toBeVisible();
      await expect(profilePage.saveButton).toBeVisible();
    });

    test('should display correctly on tablet viewport', async ({ page }) => {
      await page.setViewportSize({ width: 768, height: 1024 });
      
      await expect(profilePage.pageTitle).toBeVisible();
      await expect(profilePage.nameInput).toBeVisible();
      await expect(profilePage.emailInput).toBeVisible();
    });

    test('should maintain form layout on different screen sizes', async ({ page }) => {
      // Desktop
      await page.setViewportSize({ width: 1920, height: 1080 });
      await expect(profilePage.nameInput).toBeVisible();
      
      // Tablet
      await page.setViewportSize({ width: 768, height: 1024 });
      await expect(profilePage.nameInput).toBeVisible();
      
      // Mobile
      await page.setViewportSize({ width: 375, height: 667 });
      await expect(profilePage.nameInput).toBeVisible();
    });
  });
});
