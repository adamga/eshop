import { test, expect } from '@playwright/test';

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

test.use({ baseURL: BASE_URL });

test('Remove item from cart', async ({ page }) => {
  // Login before interacting with the product list or cart
  await page.goto(BASE_URL + 'user/login?returnUrl=');
  await page.getByLabel('Username').fill('alice');
  await page.getByLabel('Password').fill('Pass123$');
  await page.getByRole('button', { name: 'Login' }).click();
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