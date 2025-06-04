# C.3.2 Input Validation & Attack Mitigation – Threat Model & Guidance

## Threat Model Coverage
- The threat model for the eShop application explicitly acknowledges attacks on all input parsers and interpreters, including:
  - SQL Injection (SQLi)
  - Cross-Site Scripting (XSS)
  - Command Injection
  - Deserialization attacks (not applicable as no custom deserialization is used)
- All user input is considered untrusted and is validated server-side.
- All API endpoints and forms are reviewed for input validation and sanitization.

## Configuration Guidance
- All input fields must be validated on the server using DataAnnotations or FluentValidation.
- Do not rely solely on client-side validation.
- Use parameterized queries (as enforced by EF Core) for all database access.
- Output encoding is handled by Razor/Blazor by default, but any dynamic HTML should be encoded.
- For any new endpoints or forms, follow the same validation and threat modeling process.

## Evidence
- See Playwright test: `e2e/CheckoutInputAttackTest.spec.ts` for automated evidence of input validation and attack mitigation.
- See `pci-c3.2-checkout-example.md` for a full documentation template.
