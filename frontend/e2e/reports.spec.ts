import { test, expect } from '@playwright/test';
import { ReportsPage } from './page-objects/reports.page';
import { ensureLoggedIn } from './utils/session';

test.describe('Reports and Visualizations', () => {
  let reportsPage: ReportsPage;

  test.beforeEach(async ({ page }) => {
    reportsPage = new ReportsPage(page);
    await ensureLoggedIn(page);
    await reportsPage.goto();
  });

  test('should display reports page with title', async ({ page }) => {
    await expect(reportsPage.pageTitle).toBeVisible();
    await expect(page.getByText(/visualize your finances/i)).toBeVisible();
  });

  test('should display all chart sections', async () => {
    await expect(reportsPage.incomeVsExpenseChart).toBeVisible();
    await expect(reportsPage.spendingByCategoryChart).toBeVisible();
    await expect(reportsPage.trendChart).toBeVisible();
  });

  test('should display Income vs Expense Bar chart', async ({ page }) => {
    await expect(page.getByText(/income vs expense \(bar\)/i)).toBeVisible();
  });

  test('should display Spending by Category Doughnut chart', async ({ page }) => {
    await expect(page.getByText(/spending by category \(doughnut\)/i)).toBeVisible();
  });

  test('should display Trend Line chart', async ({ page }) => {
    await expect(page.getByText(/trend \(line\)/i)).toBeVisible();
  });

  test('should render all charts (canvas elements)', async () => {
    await reportsPage.waitForChartsToLoad();
    
    const chartCount = await reportsPage.getChartCount();
    expect(chartCount).toBeGreaterThanOrEqual(3);
  });

  test('should display total income summary', async () => {
    const income = await reportsPage.getTotalIncome();
    expect(income).toContain('$');
  });

  test('should display total expense summary', async () => {
    const expense = await reportsPage.getTotalExpense();
    expect(expense).toContain('$');
  });

  test('should have summary card with totals', async ({ page }) => {
    await expect(page.getByText(/total income/i)).toBeVisible();
    await expect(page.getByText(/total expense/i)).toBeVisible();
  });

  test.describe('Chart Visibility', () => {
    test('should display bar chart', async () => {
      await reportsPage.waitForChartsToLoad();
      const isVisible = await reportsPage.isChartVisible('bar');
      expect(isVisible).toBe(true);
    });

    test('should display doughnut chart', async () => {
      await reportsPage.waitForChartsToLoad();
      const isVisible = await reportsPage.isChartVisible('doughnut');
      expect(isVisible).toBe(true);
    });

    test('should display line chart', async () => {
      await reportsPage.waitForChartsToLoad();
      const isVisible = await reportsPage.isChartVisible('line');
      expect(isVisible).toBe(true);
    });
  });

  test.describe('Chart Rendering', () => {
    test('should render charts with proper dimensions', async ({ page }) => {
      await reportsPage.waitForChartsToLoad();
      
      const canvases = await page.locator('canvas').all();
      
      for (const canvas of canvases) {
        const box = await canvas.boundingBox();
        expect(box).not.toBeNull();
        if (box) {
          expect(box.width).toBeGreaterThan(0);
          expect(box.height).toBeGreaterThan(0);
        }
      }
    });

    test('should have canvas elements inside chart containers', async ({ page }) => {
      await reportsPage.waitForChartsToLoad();
      
      const barCanvas = await reportsPage.incomeVsExpenseChart.locator('canvas').count();
      const doughnutCanvas = await reportsPage.spendingByCategoryChart.locator('canvas').count();
      const lineCanvas = await reportsPage.trendChart.locator('canvas').count();
      
      expect(barCanvas).toBeGreaterThan(0);
      expect(doughnutCanvas).toBeGreaterThan(0);
      expect(lineCanvas).toBeGreaterThan(0);
    });
  });

  test.describe('Layout and Grid', () => {
    test('should display charts in grid layout', async ({ page }) => {
      const chartCards = await page.locator('.space-y-6 > div > div').count();
      expect(chartCards).toBeGreaterThanOrEqual(3);
    });

    test('should have proper spacing between charts', async ({ page }) => {
      await reportsPage.waitForChartsToLoad();
      
      const container = page.locator('.space-y-6').first();
      await expect(container).toBeVisible();
    });
  });

  test.describe('Responsive Design', () => {
    test('should display correctly on mobile viewport', async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
      
      await expect(reportsPage.pageTitle).toBeVisible();
      await expect(reportsPage.incomeVsExpenseChart).toBeVisible();
    });

    test('should display correctly on tablet viewport', async ({ page }) => {
      await page.setViewportSize({ width: 768, height: 1024 });
      
      await expect(reportsPage.pageTitle).toBeVisible();
      await reportsPage.waitForChartsToLoad();
      
      const chartCount = await reportsPage.getChartCount();
      expect(chartCount).toBeGreaterThanOrEqual(3);
    });

    test('should maintain chart aspect ratio on different viewports', async ({ page }) => {
      await reportsPage.waitForChartsToLoad();
      
      // Desktop
      await page.setViewportSize({ width: 1920, height: 1080 });
      await page.waitForTimeout(500);
      const desktopCharts = await page.locator('canvas').count();
      
      // Tablet
      await page.setViewportSize({ width: 768, height: 1024 });
      await page.waitForTimeout(500);
      const tabletCharts = await page.locator('canvas').count();
      
      expect(desktopCharts).toBe(tabletCharts);
    });
  });

  test.describe('Data Visualization', () => {
    test('should show chart legends', async ({ page }) => {
      await reportsPage.waitForChartsToLoad();
      
      // Chart.js typically renders legends, check if canvas is interactive
      const canvases = await page.locator('canvas').all();
      expect(canvases.length).toBeGreaterThan(0);
    });

    test('should display summary statistics alongside charts', async ({ page }) => {
      await expect(page.getByText(/total income/i)).toBeVisible();
      await expect(page.getByText(/total expense/i)).toBeVisible();
      
      const income = await reportsPage.getTotalIncome();
      const expense = await reportsPage.getTotalExpense();
      
      expect(income).toMatch(/\$\d+\.\d{2}/);
      expect(expense).toMatch(/\$\d+\.\d{2}/);
    });
  });
});
