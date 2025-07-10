import { test, expect } from '@playwright/test';

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

// List of input fields/pages to test for XSS/SQLi (expand as needed)
const inputTests = [
  {
    name: 'Login username field',
    url: BASE_URL + 'user/login',
    selector: 'input[name="Username"], input[label="Username"]',
    submit: async (page) => {
      await page.getByLabel('Password').fill('Pass123$');
      await page.getByRole('button', { name: 'Login' }).click();
    },
  },
  {
    name: 'Product search',
    url: BASE_URL,
    selector: 'input[type="search"], input[placeholder*="search" i]',
    submit: async (page) => {
      await page.keyboard.press('Enter');
    },
  },
  // Add more fields/pages as needed
];

const xssPayload = '<script>alert(1)</script>';
const sqliPayload = "' OR 1=1--";

// Helper to check for reflected payload or error
async function checkNoReflectionOrError(page, payload) {
  const content = await page.content();
  expect(content).not.toContain(payload);
  expect(content.toLowerCase()).not.toMatch(/syntax error|sql|exception|stack trace|unexpected token|parse error/);
}

test.describe('PCI C.3.2 - Input Validation and Injection/XSS Protection', () => {
  for (const testCase of inputTests) {
    test(`should not be vulnerable to XSS in ${testCase.name}`, async ({ page }) => {
      await page.goto(testCase.url);
      const input = await page.$(testCase.selector);
      if (!input) return test.skip();
      await input.fill(xssPayload);
      await testCase.submit(page);
      await checkNoReflectionOrError(page, xssPayload);
    });
    test(`should not be vulnerable to SQLi in ${testCase.name}`, async ({ page }) => {
      await page.goto(testCase.url);
      const input = await page.$(testCase.selector);
      if (!input) return test.skip();
      await input.fill(sqliPayload);
      await testCase.submit(page);
      await checkNoReflectionOrError(page, sqliPayload);
    });
  }
});
