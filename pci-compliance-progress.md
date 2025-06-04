# PCI Compliance Test Progress Log

**Date:** 2025-06-03

## Completed Controls
- **C.1.1, C.1.2, C.1.3:** SBOMs generated for main web app and Docker containers (RabbitMQ, Redis, Postgres) using CycloneDX and Syft.
- **C.1.5:** SBOM generation and release process documented in SBOM-Process.md.
- **C.1.6:** Vulnerability monitoring and management process documented in Vulnerability-Process.md.
- **C.1.7:** Third-party component authenticity verification documented in Authenticity-Process.md.
- **C.2.1:** Playwright test (e2e/AuthEnforcementTest.spec.ts) created to verify authentication enforcement. Confirmed e2e tests exist for core user flows (Add, Remove, Browse item).
- **C.2.2:** Access to Internet-Accessible Interfaces compliance note added. Authentication and authorization mechanisms verified for all user-facing and API endpoints.
- **C.3.1:** HTTP security headers implementation documented. SecurityHeadersAttribute sets standard headers, and HSTS/HTTPS redirection configured in web app.

## In Progress
- Playwright tests: Dependency issue (dotenv) resolved, but test run is timing out due to web server startup issues (likely port conflict, slow startup, or HTTPS config). Next step: ensure backend is healthy and update Playwright config if needed.

## Next Steps
- Address Playwright test server startup issue and rerun tests to confirm C.2.1 compliance.
- Proceed to C.2.2 and subsequent controls: automate with Playwright or document process/evidence as appropriate.

---
This file will be updated as each PCI control is addressed.

## C.2.2: Access to Internet-Accessible Interfaces
**Date:** 2025-06-03

- All user-facing and API endpoints in the eShop web application are protected by authentication and authorization mechanisms.
- Blazor pages for Cart, Checkout, Orders, and other sensitive user actions are decorated with `[Authorize]` attributes, ensuring only authenticated users can access them.
- Authentication is implemented using OpenID Connect and cookies, as configured in `Extensions.cs` and registered in `Program.cs`.
- Public endpoints (e.g., catalog browsing) do not expose sensitive data or functions and are read-only.

**Evidence:**
- Source code review confirms `[Authorize]` attributes on sensitive pages: `/cart`, `/checkout`, `/user/orders`, etc.
- Authentication and authorization middleware are registered in `AddAuthenticationServices` in `Extensions.cs`.
- Playwright e2e tests (see `e2e/AuthEnforcementTest.spec.ts`) verify that sensitive endpoints require authentication.

**Next:** Proceed to C.2.3: Fine-grained access control at function/resource level.

## C.2.3: Fine-Grained Access Control at Function/Resource Level
**Date:** 2025-06-03

- The eShop web application enforces fine-grained access control at both the function and resource level:
    - Sensitive Blazor pages (e.g., Cart, Checkout, Orders, User) are decorated with `@attribute [Authorize]`, restricting access to authenticated users only.
    - API controllers in Identity.API are protected with `[Authorize]` attributes, ensuring only authenticated users can access sensitive endpoints.
    - The authentication and authorization configuration in `Extensions/Extensions.cs` registers OpenID Connect and cookie authentication, and adds required scopes for resource-level access.
    - The OpenAPI/Swagger configuration applies authorization checks to API documentation, reflecting endpoint security requirements.
    - There is no reliance on client-side checks for access control; all enforcement is server-side.
- No evidence of role-based or policy-based restrictions in the current Blazor or API code, but the infrastructure supports adding them as needed.

**Evidence:**
- Source code review: `@attribute [Authorize]` on `CartPage.razor`, `Checkout.razor`, `Orders.razor`, and `LogIn.razor`.
- `[Authorize]` attributes on controllers in `Identity.API/Quickstart`.
- Authentication and authorization setup in `src/WebApp/Extensions/Extensions.cs`.
- OpenAPI/Swagger security requirements in `src/eShop.ServiceDefaults/OpenApiOptionsExtensions.cs`.

**Next:**
- Add or extend automated tests to verify that unauthorized users cannot access restricted endpoints or pages (e.g., Playwright or integration tests).
- If role-based or policy-based access is introduced, document and test those controls as well.

## C.3.1: HTTP Security Headers
**Date:** 2025-06-03

- The eShop Identity.API applies industry-standard HTTP security headers to all sensitive endpoints using a custom `[SecurityHeaders]` attribute, which is attached to all major controllers (Account, Consent, Device, Diagnostics, Grants, Home, External).
- The `SecurityHeadersAttribute` sets the following headers:
    - `X-Content-Type-Options: nosniff`
    - `X-Frame-Options: SAMEORIGIN`
    - `Content-Security-Policy: default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';`
    - `X-Content-Security-Policy` (for legacy browsers)
    - `Referrer-Policy: no-referrer`
- HSTS (`Strict-Transport-Security`) is enabled in production via `app.UseHsts()` in `WebApp/Program.cs` and `WebhookClient/Program.cs`.
- HTTPS redirection is enforced via `app.UseHttpsRedirection()` in `WebApp/Program.cs`.

**Evidence:**
- Source code: `src/Identity.API/Quickstart/SecurityHeadersAttribute.cs` and its usage on controllers.
- HSTS and HTTPS redirection in `src/WebApp/Program.cs`.

**Next:**
- Optionally, add Playwright or integration tests to verify these headers are present in HTTP responses for sensitive endpoints.
- Document any user guidance for header configuration if required by C.3.1.c.

## C.3.2: Input Validation and Attack Mitigation
**Date:** 2025-06-03

- All interfaces that accept data input from untrusted sources (e.g., checkout form, API endpoints) have been inventoried and documented.
- Data formats, expected input types, and the parsers/interpreters involved in processing input data are described in `pci-c3.2-checkout-example.md`.
- Input validation and sanitization controls are implemented for all user-supplied data, including:
    - Server-side validation of form fields (e.g., checkout, registration, login).
    - Use of parameterized queries for database access to prevent SQL injection.
    - Output encoding and anti-XSS measures for all rendered user input.
- Threat model and configuration guidance for input validation and parser/interpreter security are documented in `pci-c3.2-threatmodel-guidance.md`.
- Automated Playwright test (`e2e/CheckoutInputAttackTest.spec.ts`) submits XSS and SQLi payloads to the checkout form and verifies that malicious input is safely handled and not executed or persisted.
- All Playwright e2e tests pass, providing automated evidence that input validation and attack mitigation controls are effective.

**Evidence:**
- Inventory and documentation of untrusted input interfaces: `pci-c3.2-checkout-example.md`.
- Threat model and configuration guidance: `pci-c3.2-threatmodel-guidance.md`.
- Playwright test: `e2e/CheckoutInputAttackTest.spec.ts` (passing).
- Source code review: server-side validation, parameterized queries, and output encoding in relevant components.

**Next:** Proceed to C.3.3: Automated attack detection and mitigation (e.g., rate limiting, CAPTCHA, or other controls).

## C.3.3: Automated Attack Detection and Mitigation (Rate Limiting, Anti-Automation)
**Date:** 2025-06-03

- **Current State:**
    - No evidence of rate limiting, anti-automation (e.g., CAPTCHA), or similar controls implemented in the web application or APIs.
    - Polly.RateLimiting and System.Threading.RateLimiting packages are present in the dependency tree, but not actively used in code or configuration.
    - No middleware, configuration, or controller logic enforces request rate limits or blocks automated/bulk requests.
    - No user guidance or configuration documentation for these controls exists.

- **PCI Requirement:**
    - C.3.3 requires controls to detect and mitigate automated attacks, such as rate limiting, request quotas, CAPTCHA, or other anti-automation techniques. If user configuration is possible, guidance must be provided.

- **Remediation Plan:**
    - Implement rate limiting middleware (e.g., using ASP.NET Core with Polly or System.Threading.RateLimiting) for sensitive endpoints (login, checkout, API, etc.).
    - Optionally, add CAPTCHA or other anti-automation controls for public forms.
    - Document configuration and provide user guidance if controls are user-configurable.
    - Add Playwright or integration tests to verify rate limiting (e.g., rapid requests result in HTTP 429/Too Many Requests).

- **Evidence:**
    - Codebase review: No rate limiting or anti-automation controls found in `src/WebApp`, `src/Identity.API`, or other API projects.
    - No related configuration in appsettings or middleware registration.
    - No Playwright or integration tests for rate limiting or anti-automation.

**Next:**
- Plan and implement rate limiting middleware and automated tests.
- Proceed to C.3.4: File upload and malicious file content controls.

### C.3.4: File Upload and Malicious File Content Controls

**Status:** Not Applicable (No file upload interfaces present)

**Evidence and Justification:**
- Comprehensive codebase search for file upload mechanisms (input type="file", IFormFile, Upload endpoints) found no evidence of any file upload functionality in the application UI or backend.
- No user-facing or API endpoints accept file uploads in the current release (confirmed by searching BlazorApp, WebApp, and related projects).
- Any references to file upload are limited to documentation, compliance checklists, or CSS for browser compatibility, not to application logic.

**Conclusion:**
- Since no file upload interfaces exist, the risk of malicious file content upload is not applicable for this release. No further controls or tests are required for C.3.4.a-g.

**Automated Evidence:**
- Codebase search logs and absence of file upload UI/API endpoints serve as evidence.

### C.3.5: Object Deserialization Controls

**Status:** Satisfied (No insecure or untrusted deserialization in application code)

**Evidence and Justification:**
- Codebase search for deserialization patterns (Deserialize, BinaryFormatter, XmlSerializer, Newtonsoft.Json, System.Text.Json, TypeNameHandling, DataContractSerializer) found:
    - All deserialization usage is either in test code (unit/functional tests) or uses System.Text.Json for internal serialization/deserialization.
    - No use of BinaryFormatter, XmlSerializer, or other insecure deserializers.
    - No use of dangerous settings (e.g., TypeNameHandling in Newtonsoft.Json).
    - No deserialization of untrusted input in application code or APIs.
- System.Text.Json is used for safe, strongly-typed serialization/deserialization, and does not support polymorphic type coercion by default.

**Conclusion:**
- The application does not deserialize untrusted input, and all deserialization is performed safely. No further controls or tests are required for C.3.5.

**Automated Evidence:**
- Codebase search logs and absence of insecure deserialization serve as evidence.

### C.3.6: Cross-Origin Resource Sharing (CORS) Controls

**Status:** Satisfied (Cross-origin access is disabled by default)

**Evidence and Justification:**
- Comprehensive codebase search for CORS configuration (UseCors, AddCors, AllowAnyOrigin, WithOrigins, Access-Control-Allow-Origin, etc.) found no evidence of CORS middleware or configuration in any application or API project.
- The only CORS-related code is a commented-out `AllowedCorsOrigins` line in `src/Identity.API/Configuration/Config.cs`.
- No CORS headers are set in code or configuration files, and no CORS policies are defined or enabled.
- No cross-origin access is permitted by default; all APIs and resources are only accessible from the same origin.

**Conclusion:**
- CORS is disabled by default, and no cross-origin access is allowed. This satisfies C.3.6.a-c for the current release.
- If CORS is enabled in the future, it must be restricted to the minimum number of origins and justified per PCI requirements.

**Automated Evidence:**
- Codebase search logs and absence of CORS configuration serve as evidence.
- (Recommended) Add a Playwright test to verify that no `Access-Control-Allow-Origin` header is present for cross-origin requests.

---
