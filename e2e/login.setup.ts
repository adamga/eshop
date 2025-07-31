/**
 * login.setup.ts
 * 
 * Authentication Setup for Playwright Tests
 * 
 * This setup file handles user authentication and stores the authentication state
 * for reuse in subsequent tests. It performs the following actions:
 * - Logs in a user using credentials from environment variables
 * - Saves the authentication state to a file for reuse
 * - Validates successful login by checking homepage visibility
 * 
 * Environment Variables Required:
 * - USERNAME1: The username for authentication
 * - PASSWORD: The password for authentication
 * 
 * Output: Creates authentication state file at playwright/.auth/user.json
 * Usage: Referenced by other tests that require authenticated user sessions
 */

import { test as setup, expect } from '@playwright/test';

// Base URL for the eShop application
const BASE_URL = 'https://webapp.ambitiouswave-fa8284ee.canadacentral.azurecontainerapps.io/';

setup.use({ baseURL: BASE_URL });

import { STORAGE_STATE } from '../playwright.config';
import { assert } from 'console';

// Validate that required environment variables are set
assert(process.env.USERNAME1, 'USERNAME1 is not set');
assert(process.env.PASSWORD, 'PASSWORD is not set');

setup('Login', async ({ page }) => {
  // Navigate to the eShop homepage
  await page.goto('/');
  
  // Verify the homepage loaded correctly
  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();

  // Click the "Sign in" button to navigate to the login page
  await page.getByLabel('Sign in').click();
  
  // Verify we're on the login page
  await expect(page.getByRole('heading', { name: 'Login' })).toBeVisible();

  // Fill in the username from environment variable
  await page.getByPlaceholder('Username').fill(process.env.USERNAME1!);
  
  // Fill in the password from environment variable
  await page.getByPlaceholder('Password').fill(process.env.PASSWORD!);
  
  // Submit the login form
  await page.getByRole('button', { name: 'Login' }).click();
  
  // Verify successful login by checking we're back on the homepage
  await expect(page.getByRole('heading', { name: 'Ready for a new adventure?' })).toBeVisible();
  
  // Save the authentication state to file for reuse in other tests
  await page.context().storageState({ path: STORAGE_STATE });
})
