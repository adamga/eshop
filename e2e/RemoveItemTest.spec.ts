/**
 * RemoveItemTest.spec.ts
 * 
 * End-to-end test suite for removing items from the shopping cart.
 * 
 * This test file validates the complete user journey of:
 * - Adding an item to the shopping cart
 * - Navigating to the shopping cart
 * - Removing the item by setting quantity to 0
 * - Verifying the cart becomes empty
 * 
 * Prerequisites: User must be authenticated (uses login setup)
 * Target Application: eShop web application
 */

import { test, expect } from '@playwright/test';

// Base URL for the eShop application
const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

test.use({ baseURL: BASE_URL });

test('Remove item from cart', async ({ page }) => {
  // Navigate to the eShop homepage
  await page.goto('/');
  
  // Verify the homepage loaded correctly
  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();
  
  // Click on the "Adventurer GPS Watch" product to view details
  await page.getByRole('link', { name: 'Adventurer GPS Watch' }).click();
  
  // Verify we're on the product detail page
  await expect(page.getByRole('heading', { name: 'Adventurer GPS Watch' })).toBeVisible();
  
  // Add the product to the shopping cart
  await page.getByRole('button', { name: 'Add to shopping bag' }).click();
  
  // Navigate to the shopping cart
  await page.getByRole('link', { name: 'shopping bag' }).click();
  
  // Verify we're on the shopping cart page
  await expect(page.getByRole('heading', { name: 'Shopping bag' })).toBeVisible();

  // Ensure the item was added to the cart (wait for product quantity to be available)
  await expect.poll(() => page.getByLabel('product quantity').count()).toBeGreaterThan(0);
  
  // Remove the item by setting quantity to 0
  await page.getByLabel('product quantity').fill('0');

  // Click the update button to apply the quantity change
  await page.getByRole('button', { name: 'Update' }).click();

  // Verify the cart is now empty
  await expect(page.getByText('Your shopping bag is empty')).toBeVisible();
});