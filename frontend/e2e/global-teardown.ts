import { FullConfig } from '@playwright/test';

async function globalTeardown(config: FullConfig) {
  console.log('🧹 Starting E2E test suite teardown...');
  
  // Perform any cleanup operations here
  // For example: clearing test data, closing connections, etc.
  
  console.log('✅ Global teardown completed successfully');
}

export default globalTeardown;