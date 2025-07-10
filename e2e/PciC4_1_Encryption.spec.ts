import { test, expect } from '@playwright/test';
import https from 'https';
import { TLSSocket } from 'tls';

const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';
const HTTP_BASE_URL = 'http://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

const sensitiveEndpoints = [
  '', // home
  'user/login',
  'account',
  'api/orders',
  'api/basket',
];

test.describe('PCI C.4.1 - Sensitive Data Transmissions are Encrypted', () => {
  for (const endpoint of sensitiveEndpoints) {
    test(`should redirect HTTP to HTTPS for endpoint: ${endpoint}`, async ({ request }) => {
      const response = await request.get(HTTP_BASE_URL + endpoint, { maxRedirects: 0 });
      expect([301, 302, 307, 308, 403, 404]).toContain(response.status());
      // If redirected, should be to HTTPS
      if (response.status() >= 300 && response.status() < 400) {
        const location = response.headers()['location'];
        expect(location).toBeDefined();
        expect(location.startsWith('https://')).toBeTruthy();
      }
    });

    test(`should use valid TLS certificate for HTTPS endpoint: ${endpoint}`, async () => {
      await new Promise((resolve, reject) => {
        const req = https.get(BASE_URL + endpoint, (res) => {
          const tlsSocket = res.socket as TLSSocket;
          const cert = tlsSocket.getPeerCertificate();
          try {
            expect(cert).toBeDefined();
            expect(cert.valid_to).toBeDefined();
            // Optionally, check for issuer, subject, etc.
            resolve(true);
          } catch (e) {
            reject(e);
          }
        });
        req.on('error', reject);
      });
    });
  }

  test('should send HSTS header on main page', async ({ request }) => {
    const response = await request.get(BASE_URL);
    const hsts = response.headers()['strict-transport-security'];
    expect(hsts).toBeDefined();
    expect(hsts).toMatch(/max-age=/);
  });
});
