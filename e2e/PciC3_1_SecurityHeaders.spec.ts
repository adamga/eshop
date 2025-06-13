import { test, expect } from '@playwright/test';

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

// List of headers to check and their recommended values (where applicable)
const securityHeaders = [
  { name: 'strict-transport-security', required: true },
  { name: 'content-security-policy', required: true },
  { name: 'x-frame-options', required: true },
  { name: 'x-content-type-options', required: true },
  { name: 'referrer-policy', required: true },
  { name: 'permissions-policy', required: false },
  { name: 'cross-origin-opener-policy', required: false },
  { name: 'cross-origin-resource-policy', required: false },
];

test('PCI C.3.1 - HTTP Security Headers present on main page', async ({ request }) => {
  const response = await request.get(BASE_URL);
  expect(response.ok()).toBeTruthy();
  const headers = response.headers();
  for (const header of securityHeaders) {
    const value = headers[header.name];
    if (header.required) {
      expect(value, `Header ${header.name} should be present`).toBeTruthy();
    } else {
      // Optional headers: log if missing
      if (!value) {
        console.warn(`Optional security header missing: ${header.name}`);
      }
    }
  }
});
