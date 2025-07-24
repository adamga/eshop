/**
 * BrowseItemTest.spec.ts
 * 
 * Test Purpose:
 * This end-to-end test validates the product browsing functionality by testing the user's ability
 * to navigate from the homepage to individual product detail pages in the eShop web application.
 * 
 * How the Test Works:
 * 1. Navigates to the eShop homepage (root path)
 * 2. Verifies the homepage loads correctly by checking for the welcome heading "Ready for a new adventure?"
 * 3. Clicks on a specific product link ("Adventurer GPS Watch") to navigate to its detail page
 * 4. Validates that the product detail page loads correctly by:
 *    - Clicking on the product heading to ensure it's interactive
 *    - Verifying the product heading "Adventurer GPS Watch" is visible on the detail page
 * 
 * This test ensures that users can successfully browse from the product catalog to individual
 * product pages, which is fundamental to the e-commerce user experience.
 */
import { test, expect } from '@playwright/test';

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

test.use({ baseURL: BASE_URL });

test('Browse Items', async ({ page }) => {
  await page.goto('/');

  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();

  await page.getByRole('link', { name: 'Adventurer GPS Watch' }).click(); 
  await page.getByRole('heading', { name: 'Adventurer GPS Watch' }).click();
  
  //Expect
  await expect(page.getByRole('heading', { name: 'Adventurer GPS Watch' })).toBeVisible();
});