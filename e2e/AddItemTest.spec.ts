import { test, expect } from '@playwright/test';

test('Add item to the cart', async ({ page }) => {
  await page.goto('/');

  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible({ timeout: 20000 });
  await page.getByRole('link', { name: 'Adventurer GPS Watch' }).click();
  await page.getByRole('button', { name: 'Add to shopping bag' }).click();
  await page.getByRole('link', { name: 'shopping bag' }).click();
  await page.getByRole('heading', { name: 'Shopping bag' }).click();

  // Debug: check how many 'Total' elements exist and take a screenshot
  const totalElements = await page.getByText('Total').all();
  console.log('Number of Total elements:', totalElements.length);
  await page.screenshot({ path: 'additemtest-debug.png', fullPage: true });

  // Only click if there is a second 'Total' element, otherwise skip
  if (totalElements.length > 1) {
    await totalElements[1].click();
  } else if (totalElements.length === 1) {
    await totalElements[0].click();
  } else {
    throw new Error("No 'Total' element found on Shopping bag page.");
  }

  await page.getByLabel('product quantity').getByText('1');

  await expect.poll(() => page.getByLabel('product quantity').count()).toBeGreaterThan(0);
});