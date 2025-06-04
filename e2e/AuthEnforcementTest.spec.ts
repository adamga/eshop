import { test, expect } from '@playwright/test';

// This test verifies that sensitive endpoints require authentication
// Adjust the URLs and expected redirects/messages as needed for your app

test('Sensitive endpoints require authentication', async ({ page }) => {
  // Example: Try to access the shopping bag page directly without logging in
  await page.goto('/shopping-bag');

  // Expect to be redirected to a login page or see an authentication prompt
  // Adjust the selector/text below to match your actual login page
  await expect(
    page.getByRole('heading', { name: /sign in|login/i })
  ).toBeVisible();

  // Example: Try to access an order history page (if exists)
  await page.goto('/orders');
  await expect(
    page.getByRole('heading', { name: /sign in|login/i })
  ).toBeVisible();
});
