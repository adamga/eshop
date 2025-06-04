# PCI C.3.2 Input Validation & Attack Mitigation Documentation

## Example: Checkout Form (WebApp)

### Interface
- **Path:** `/checkout` (Blazor WebApp)
- **Model:** `BasketCheckoutInfo`
- **Fields:** Street, City, State, Country, ZipCode, CardNumber, CardHolderName, CardSecurityNumber, CardExpiration
- **Data Format:** Form POST (Blazor model binding)
- **Parser/Interpreter:** ASP.NET Core model binding, DataAnnotations

### Input Validation
- All address fields: `[Required]`
- Card fields: validated in Ordering API via FluentValidation (`CreateOrderCommandValidator`)
- Custom validation for expiration date and order items

### Security Controls
- Server-side validation (DataAnnotations, FluentValidation)
- No direct SQL construction (uses EF Core)
- Razor/Blazor auto-encodes output
- No dynamic code execution

### Threat Model
- Injection (SQLi, XSS, Command): Mitigated by validation, parameterized queries, and output encoding
- Malformed input: Rejected by model validation
- Overly large input: Not explicitly limited (recommend adding max length)

### Automated Evidence
- Playwright e2e test will:
  1. Attempt to submit XSS payload in address fields
  2. Attempt SQLi payload in card fields
  3. Verify that the app rejects or safely handles malicious input (no script execution, no SQL error, validation error shown)

### Configuration Guidance
- All input fields should be validated server-side
- Do not trust client-side validation alone
- Consider adding max length constraints for all fields

---

Repeat this template for each API endpoint and form that accepts untrusted input.
