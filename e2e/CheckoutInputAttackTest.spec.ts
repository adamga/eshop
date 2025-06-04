import { test, expect } from '@playwright/test';

// This test attempts to submit malicious input to the checkout form and verifies the app's response.
test.describe('Checkout Form Input Validation & Attack Mitigation', () => {
  test('should reject XSS and SQLi payloads in checkout form', async ({ page }) => {
    // Login first (reuse login.setup.ts if needed)
    await page.goto('/');
    await page.getByLabel('Sign in').click();
    await expect(page.getByRole('heading', { name: 'Login' })).toBeVisible();
    await page.getByPlaceholder('Username').fill(process.env.USERNAME1!);
    await page.getByPlaceholder('Password').fill(process.env.PASSWORD!);
    await page.getByRole('button', { name: 'Login' }).click();
    await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();

    // Go to checkout
    await page.goto('/checkout');
    await expect(page.getByRole('heading', { name: 'Checkout' })).toBeVisible();

    // Fill form with XSS and SQLi payloads
    await page.getByLabel('Address').fill('<script>alert(1)</script>');
    await page.getByLabel('City').fill("' OR 1=1--");
    await page.getByLabel('State').fill('test');
    await page.getByLabel('Zip code').fill('12345');
    await page.getByLabel('Country').fill('USA');
    // Card fields may be on a different step or handled server-side, so we focus on address for UI XSS

    // Submit the form
    await page.getByRole('button', { name: 'Place order' }).click();

    // Expect validation error or safe handling (no script execution, no SQL error)
    // Check for validation summary or error message
    await expect(page.getByText(/error|invalid|not allowed|required/i)).toBeVisible();
    // Optionally, check that the script tag is not rendered
    await expect(page.locator('text=<script>alert(1)</script>')).toHaveCount(0);
  });
});
