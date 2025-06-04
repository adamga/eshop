import { test, expect } from '@playwright/test';

// This test attempts a cross-origin request and verifies that no CORS headers are present (CORS is disabled by default)
test.describe('CORS Policy', () => {
  test('should not allow cross-origin requests (no Access-Control-Allow-Origin header)', async ({ request }) => {
    // Use a public endpoint (e.g., /) for the test
    const response = await request.get('/', {
      headers: {
        Origin: 'https://evil.com',
      },
    });
    // The header should not be present, or should not echo the origin
    const corsHeader = response.headers()['access-control-allow-origin'];
    expect(corsHeader).toBeFalsy();
  });
});
