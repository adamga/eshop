import { test, expect } from '@playwright/test';

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

test.use({ baseURL: BASE_URL });

test.describe('Add item to the cart', () => {
  test('Add item to the cart', async ({ page }) => {
    await page.goto(BASE_URL);

    // Login before interacting with the product list or cart
    await page.goto(BASE_URL + 'user/login?returnUrl=');
    await page.getByLabel('Username').fill('alice');
    await page.getByLabel('Password').fill('Pass123$');
    await page.getByRole('button', { name: 'Login' }).click();
    // Wait for login to complete (e.g., by checking for homepage heading)
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