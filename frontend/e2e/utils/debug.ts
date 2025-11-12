import type { Page, TestInfo } from '@playwright/test';

const resolveDelay = () => {
  const candidates = [
    process.env.E2E_DEBUG_DELAY,
    process.env.PLAYWRIGHT_DEBUG_DELAY,
  ];

  for (const candidate of candidates) {
    if (candidate !== undefined && candidate !== '') {
      const parsed = Number(candidate);
      if (!Number.isNaN(parsed)) {
        return parsed;
      }
    }
  }

  return 0;
};

const DEBUG_DELAY = resolveDelay();

export async function captureDebugScreenshot(
  page: Page,
  testInfo: TestInfo,
  label?: string
) {
  if (DEBUG_DELAY > 0) {
    await page.waitForTimeout(DEBUG_DELAY);
  }

  try {
    const screenshot = await page.screenshot({ fullPage: true });
    await testInfo.attach(label ?? 'debug-screenshot', {
      body: screenshot,
      contentType: 'image/png',
    });
  } catch (error) {
    console.warn(`Failed to capture screenshot for ${testInfo.title}`, error);
  }
}
