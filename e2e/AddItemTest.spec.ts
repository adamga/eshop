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