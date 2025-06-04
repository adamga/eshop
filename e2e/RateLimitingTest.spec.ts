import { test, expect } from '@playwright/test';

// This test will rapidly send requests to the home page to trigger the rate limiter.
test('Rate limiting returns 429 after exceeding limit', async ({ request }) => {
  const url = process.env.CALLBACKURL || 'https://localhost:7298/';
  let lastResponse;
  for (let i = 0; i < 7; i++) {
    lastResponse = await request.get(url, { ignoreHTTPSErrors: true });
  }
  // The 6th or 7th request should be rate limited (HTTP 429)
  expect([429, 200]).toContain(lastResponse.status());
  if (lastResponse.status() !== 429) {
    // Try one more to ensure we hit the limit
    const resp = await request.get(url, { ignoreHTTPSErrors: true });
    expect(resp.status()).toBe(429);
  }
});
