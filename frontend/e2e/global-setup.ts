import { chromium, FullConfig } from '@playwright/test';

async function globalSetup(config: FullConfig) {
  console.log('🚀 Starting E2E test suite setup...');
  
  const { baseURL } = config.projects[0].use;
  
  if (!baseURL) {
    console.warn('⚠️  No baseURL configured, skipping health check');
    return;
  }

  // Wait for the application to be ready
  const browser = await chromium.launch();
  const page = await browser.newPage();
  
  try {
    console.log(`🔍 Checking if application is ready at ${baseURL}...`);
    await page.goto(baseURL, { waitUntil: 'networkidle', timeout: 30000 });
    console.log('✅ Application is ready!');
  } catch (error) {
    console.error('❌ Application is not ready:', error);
    throw new Error('Application failed to start. Please ensure the dev server is running.');
  } finally {
    await browser.close();
  }

  console.log('✅ Global setup completed successfully');
}

export default globalSetup;