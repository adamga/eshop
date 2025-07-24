/**
 * RemoveItemTest.spec.ts
 * 
 * Test Purpose:
 * This end-to-end test validates the complete shopping cart item removal functionality by testing
 * the full user flow from adding an item to the cart to removing it, ensuring the cart properly
 * updates to an empty state.
 * 
 * How the Test Works:
 * 1. Navigates to the eShop homepage and verifies it loads correctly
 * 2. Browses to a specific product ("Adventurer GPS Watch") and verifies the product page loads
 * 3. Adds the product to the shopping bag using the "Add to shopping bag" button
 * 4. Navigates to the shopping bag and verifies it contains the added item
 * 5. Validates that at least one product quantity element exists (confirming item was added)
 * 6. Removes the item by:
 *    - Setting the product quantity to "0" using the quantity input field
 *    - Clicking the "Update" button to apply the change
 * 7. Verifies the cart is now empty by checking for the "Your shopping bag is empty" message
 * 
 * This test ensures the complete lifecycle of cart management works correctly, including both
 * adding and removing items, which is essential for e-commerce functionality and user experience.
 */
import { test, expect } from '@playwright/test';

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

test.use({ baseURL: BASE_URL });

test('Remove item from cart', async ({ page }) => {
  await page.goto('/');
  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();
  
  await page.getByRole('link', { name: 'Adventurer GPS Watch' }).click();
  await expect(page.getByRole('heading', { name: 'Adventurer GPS Watch' })).toBeVisible();
  
  await page.getByRole('button', { name: 'Add to shopping bag' }).click();
  await page.getByRole('link', { name: 'shopping bag' }).click();
  await expect(page.getByRole('heading', { name: 'Shopping bag' })).toBeVisible();

  await expect.poll(() => page.getByLabel('product quantity').count()).toBeGreaterThan(0);
  
  await page.getByLabel('product quantity').fill('0');

  await page.getByRole('button', { name: 'Update' }).click();

  await expect(page.getByText('Your shopping bag is empty')).toBeVisible();
});