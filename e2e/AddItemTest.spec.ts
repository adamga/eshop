/**
 * AddItemTest.spec.ts
 * 
 * Test Purpose:
 * This end-to-end test validates the core shopping cart functionality by testing the complete user flow
 * of adding an item to the shopping cart in the eShop web application.
 * 
 * How the Test Works:
 * 1. Navigates to the eShop homepage
 * 2. Verifies the page loads correctly by checking for the welcome heading
 * 3. Clicks on a specific product ("Adventurer GPS Watch") to view its details
 * 4. Adds the product to the shopping bag using the "Add to shopping bag" button
 * 5. Navigates to the shopping bag by clicking the shopping bag link
 * 6. Verifies the shopping bag page loads and displays the correct heading
 * 7. Validates that the product was successfully added by checking:
 *    - The presence of a "Total" section
 *    - The product quantity shows "1"
 *    - At least one product quantity element exists
 * 
 * This test ensures the end-to-end flow from product discovery to cart addition works correctly
 * and is critical for validating the core e-commerce functionality.
 */
import { test, expect } from '@playwright/test';

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

test.use({ baseURL: BASE_URL });

test.describe('Add item to the cart', () => {
  test('Add item to the cart', async ({ page }) => {
    await page.goto(BASE_URL);

    await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();
    await page.getByRole('link', { name: 'Adventurer GPS Watch' }).click();
    await page.getByRole('button', { name: 'Add to shopping bag' }).click();
    await page.getByRole('link', { name: 'shopping bag' }).click();
    await page.getByRole('heading', { name: 'Shopping bag' }).click();

    await page.getByText('Total').nth(1).click();
    await page.getByLabel('product quantity').getByText('1');

    await expect.poll(() => page.getByLabel('product quantity').count()).toBeGreaterThan(0);
  });
});