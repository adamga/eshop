import { test as setup, expect } from '@playwright/test';

// Use localhost in CI, otherwise use the configured remote URL
const BASE_URL = process.env.CI ? 'http://localhost:5045' : 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

setup.use({ baseURL: BASE_URL });

import { STORAGE_STATE } from '../playwright.config';

// For CI environment, use default test credentials
const USERNAME = process.env.USERNAME1 || 'test@example.com';
const PASSWORD = process.env.PASSWORD || 'Pass123$';

setup('Login', async ({ page }) => {
  await page.goto('/');
  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();

  await page.getByLabel('Sign in').click();
  await expect(page.getByRole('heading', { name: 'Login' })).toBeVisible();

  await page.getByPlaceholder('Username').fill(USERNAME);
  await page.getByPlaceholder('Password').fill(PASSWORD);
  await page.getByRole('button', { name: 'Login' }).click();
  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();
  await page.context().storageState({ path: STORAGE_STATE });
})