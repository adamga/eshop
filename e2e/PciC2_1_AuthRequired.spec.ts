/**
 * PciC2_1_AuthRequired.spec.ts
 * 
 * PCI DSS Compliance Test Suite - Requirement C.2.1
 * 
 * This test file validates PCI DSS compliance requirement C.2.1:
 * "User access to sensitive functions and resources is authenticated"
 * 
 * Test Strategy:
 * - Attempts to access sensitive endpoints without authentication
 * - Expects HTTP 401 (Unauthorized), 403 (Forbidden), or 404 (Not Found) responses
 * - Validates that the application properly protects sensitive resources
 * 
 * Note: This test runs against a remote production URL and does not require
 * the local webServer to be running.
 * 
 * Prerequisites: None (specifically tests unauthenticated access)
 * Target Application: eShop web application
 */

import { test, expect } from '@playwright/test';

// Base URL for the eShop application (production environment)
const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

// List of sensitive endpoints that should require authentication
// These endpoints typically contain user data, administrative functions, or business logic
const sensitiveEndpoints = [
  'api/orders',    // User order history and details
  'api/basket',    // Shopping cart contents  
  'api/catalog',   // Product catalog management
  'account',       // User account information
  'admin',         // Administrative functions
];

// NOTE: This test file is intended to run against a remote production URL.
// If you have a 'webServer' property in your playwright.config.ts, ensure it is disabled or removed for this test suite.
// You can use 'test.use({ baseURL: BASE_URL })' if you want to set a base URL for all tests.

test.describe('PCI C.2.1 - Authentication Required for Sensitive Endpoints', () => {
  // Create a test for each sensitive endpoint
  for (const endpoint of sensitiveEndpoints) {
    test(`should deny unauthenticated access to ${endpoint}`, async ({ request }) => {
      // Construct the full URL for the endpoint
      const url = BASE_URL + endpoint;
      
      // Make an unauthenticated GET request to the sensitive endpoint
      const response = await request.get(url);
      
      // Verify that access is denied with appropriate HTTP status codes:
      // - 401 Unauthorized: Authentication is required
      // - 403 Forbidden: Access is explicitly denied
      // - 404 Not Found: Endpoint doesn't exist or is hidden from unauthenticated users
      expect([401, 403, 404]).toContain(response.status());
    });
  }
});
