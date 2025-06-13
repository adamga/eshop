import { test, expect } from '@playwright/test';

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

// This test logs in as a regular user and attempts to access an admin-only page/endpoint
// Update 'admin' endpoint if you have a more specific admin-only path
const adminEndpoints = [
  'admin',
  'admin/dashboard',
  'admin/users',
  'admin/settings',
];

test.describe('PCI C.2.2/C.2.3 - Authorization for Admin Endpoints', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto(BASE_URL + 'user/login?returnUrl=');
    await page.getByLabel('Username').fill('alice');
    await page.getByLabel('Password').fill('Pass123$');
    await page.getByRole('button', { name: 'Login' }).click();
    await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();
  });

  for (const endpoint of adminEndpoints) {
    test(`should deny regular user access to admin endpoint: ${endpoint}`, async ({ page }) => {
      const response = await page.goto(BASE_URL + endpoint);
      // Accept 403 Forbidden, 401 Unauthorized, or 404 Not Found as valid denied responses
      const deniedStatus = [401, 403, 404];
      const status = response ? response.status() : undefined;
      let statusDenied = status && deniedStatus.includes(status);
      // If using UI, check for access denied message or redirect
      const bodyText = await page.content();
      let contentDenied = (
        bodyText.toLowerCase().includes('access denied') ||
        bodyText.toLowerCase().includes('not authorized') ||
        bodyText.toLowerCase().includes('forbidden') ||
        bodyText.toLowerCase().includes('not found') ||
        page.url().toLowerCase().includes('login')
      );
      expect(statusDenied || contentDenied).toBeTruthy();
    });
  }
});
