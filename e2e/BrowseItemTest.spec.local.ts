import { test, expect } from '@playwright/test';

// Use localhost in CI, otherwise use the configured remote URL
const BASE_URL = process.env.CI ? 'http://localhost:5045' : 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

test.use({ baseURL: BASE_URL });

test('Browse Items', async ({ page }) => {
  await page.goto('/');

  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();

  await page.getByRole('link', { name: 'Adventurer GPS Watch' }).click(); 
  await page.getByRole('heading', { name: 'Adventurer GPS Watch' }).click();
  
  //Expect
  await expect(page.getByRole('heading', { name: 'Adventurer GPS Watch' })).toBeVisible();
});