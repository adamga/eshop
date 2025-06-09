import { test, expect } from '@playwright/test';

// PCI C.2.1: User access to sensitive functions and resources is authenticated
// This test attempts to access a sensitive endpoint without authentication and expects a 401/403
// Update the endpoint paths as needed for your application

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

// List of sensitive endpoints to test (add more as needed)
const sensitiveEndpoints = [
  'api/orders',
  'api/basket',
  'api/catalog',
  'account',
  'admin',
];

// NOTE: This test file is intended to run against a remote production URL.
// If you have a 'webServer' property in your playwright.config.ts, ensure it is disabled or removed for this test suite.
// You can use 'test.use({ baseURL: BASE_URL })' if you want to set a base URL for all tests.

test.describe('PCI C.2.1 - Authentication Required for Sensitive Endpoints', () => {
  for (const endpoint of sensitiveEndpoints) {
    test(`should deny unauthenticated access to ${endpoint}`, async ({ request }) => {
      const url = BASE_URL + endpoint;
      const response = await request.get(url);
      // Accept 401, 403, or 404 as valid unauthenticated responses
      expect([401, 403, 404]).toContain(response.status());
    });
  }
});
