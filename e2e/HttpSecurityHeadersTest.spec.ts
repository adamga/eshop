import { test, expect } from '@playwright/test';

// This test verifies that HTTP security headers are present on sensitive endpoints (e.g., login page)
test('Sensitive endpoints return required HTTP security headers', async ({ request }) => {
  // Adjust the URL to a sensitive endpoint (e.g., login page)
  const response = await request.get('/Account/Login');
  expect(response.status()).toBe(200);

  // Check for standard security headers
  expect(response.headers()['x-content-type-options']).toBe('nosniff');
  expect(response.headers()['x-frame-options']).toBe('SAMEORIGIN');
  expect(response.headers()['content-security-policy']).toContain("default-src 'self'");
  expect(response.headers()['referrer-policy']).toBe('no-referrer');

  // HSTS should be present on HTTPS endpoints
  if (response.url().startsWith('https://')) {
    expect(response.headers()['strict-transport-security']).toBeDefined();
  }
});

// This test tries the root endpoint to see if the main web app is reachable and returns security headers
test('Root endpoint returns required HTTP security headers', async ({ request }) => {
  const response = await request.get('/');
  expect(response.status()).toBe(200);

  // Check for standard security headers
  expect(response.headers()['x-content-type-options']).toBe('nosniff');
  expect(response.headers()['x-frame-options']).toBe('SAMEORIGIN');
  expect(response.headers()['content-security-policy']).toContain("default-src 'self'");
  expect(response.headers()['referrer-policy']).toBe('no-referrer');

  // HSTS should be present on HTTPS endpoints
  if (response.url().startsWith('https://')) {
    expect(response.headers()['strict-transport-security']).toBeDefined();
  }
});
