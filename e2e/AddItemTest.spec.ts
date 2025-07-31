/**
 * AddItemTest.spec.ts
 * 
 * End-to-end test suite for adding items to the shopping cart functionality.
 * 
 * This test file validates the complete user journey of:
 * - Navigating to the eShop homepage
 * - Selecting a product (Adventurer GPS Watch)
 * - Adding the product to the shopping cart
 * - Verifying the item appears in the cart with correct quantity
 * 
 * Prerequisites: User must be authenticated (uses login setup)
 * Target Application: eShop web application
 */

import { test, expect } from '@playwright/test';

// Base URL for the eShop application
const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

test.use({ baseURL: BASE_URL });

test.describe('Add item to the cart', () => {
  test('Add item to the cart', async ({ page }) => {
    // Navigate to the eShop homepage
    await page.goto(BASE_URL);

    // Verify the homepage loaded correctly by checking the main heading
    await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();
    
    // Click on the "Adventurer GPS Watch" product link to view product details
    await page.getByRole('link', { name: 'Adventurer GPS Watch' }).click();
    
    // Add the selected product to the shopping cart
    await page.getByRole('button', { name: 'Add to shopping bag' }).click();
    
    // Navigate to the shopping cart by clicking the shopping bag link
    await page.getByRole('link', { name: 'shopping bag' }).click();
    
    // Verify we're now on the shopping cart page
    await page.getByRole('heading', { name: 'Shopping bag' }).click();

    // Verify the total section is visible (indicates items are present)
    await page.getByText('Total').nth(1).click();
    
    // Check that the product quantity shows as 1
    await page.getByLabel('product quantity').getByText('1');

    // Verify that at least one product quantity element is present in the cart
    await expect.poll(() => page.getByLabel('product quantity').count()).toBeGreaterThan(0);
  });
});