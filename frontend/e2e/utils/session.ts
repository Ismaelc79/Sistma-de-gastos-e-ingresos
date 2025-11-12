import { Page, expect } from '@playwright/test';

const TEST_USER_EMAIL =
  process.env.E2E_TEST_USER_EMAIL || 'testfinance@yopmail.com';
const TEST_USER_PASSWORD =
  process.env.E2E_TEST_USER_PASSWORD || 'Prueba123';

async function performLogin(page: Page) {
  await page.goto('/login', { waitUntil: 'networkidle' });
  await page.fill('input[type="email"]', TEST_USER_EMAIL);
  const passwordInput = page.locator('input[type="password"]').first();
  await passwordInput.fill(TEST_USER_PASSWORD);
  await page.locator('button[type="submit"]').click();
  await page.waitForURL('**/dashboard', { timeout: 20000 });
  await expect(page).toHaveURL(/.*dashboard/);
}

export async function ensureLoggedIn(page: Page) {
  await page.goto('/dashboard', { waitUntil: 'domcontentloaded' });

  if (page.url().includes('/login')) {
    await performLogin(page);
  }
}
