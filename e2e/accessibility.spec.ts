// Automated WCAG 2.1 accessibility test using Playwright and axe-core
// This test will scan the homepage for accessibility issues

const { test, expect } = require('@playwright/test');
const { injectAxe, checkA11y } = require('@axe-core/playwright');

test.describe('Accessibility (WCAG 2.1) Compliance', () => {
  test('Home page should have no critical accessibility violations', async ({ page }) => {
    await page.goto('/'); // Adjust this URL if your app uses a different base path
    await injectAxe(page);
    await checkA11y(page, null, {
      detailedReport: true,
      detailedReportOptions: { html: true },
      axeOptions: {
        runOnly: {
          type: 'tag',
          values: ['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa']
        }
      }
    });
  });
});
