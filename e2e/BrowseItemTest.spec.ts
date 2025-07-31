/**
 * BrowseItemTest.spec.ts
 * 
 * End-to-end test suite for browsing and viewing product details.
 * 
 * This test file validates the product browsing functionality:
 * - Navigating to the eShop homepage
 * - Clicking on a specific product to view its details
 * - Verifying the product detail page loads correctly
 * 
 * Prerequisites: No authentication required
 * Target Application: eShop web application
 */

import { test, expect } from '@playwright/test';

// Base URL for the eShop application
const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

test.use({ baseURL: BASE_URL });

test('Browse Items', async ({ page }) => {
  // Navigate to the homepage using the configured baseURL
  await page.goto('/');

  // Verify the homepage loaded correctly by checking the main heading
  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();

  // Click on the "Adventurer GPS Watch" product link to view its details
  await page.getByRole('link', { name: 'Adventurer GPS Watch' }).click(); 
  
  // Verify the product detail page loaded by checking for the product name heading
  await page.getByRole('heading', { name: 'Adventurer GPS Watch' }).click();
  
  // Assertion: Ensure the product detail page displays the correct product heading
  await expect(page.getByRole('heading', { name: 'Adventurer GPS Watch' })).toBeVisible();
});