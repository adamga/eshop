# PCI C.3.2 Input Validation Compliance Gap: Login Username Reflection

**Date:** 2025-06-13

## Finding
- The login form at `/user/login` reflects unsanitized user input (including XSS and SQLi payloads) back into the `Username` field's value in the HTML after a failed login attempt.
- Example payloads reflected:
  - `<script>alert(1)</script>`
  - `' OR 1=1--`
- This behavior does not immediately result in code execution, but it is a compliance gap and a potential XSS risk if the value is ever rendered unsafely elsewhere.

## Evidence
- Automated Playwright test (`PciC3_2_InputValidation.spec.ts`) submitted the above payloads and observed them reflected in the HTML response.
- No stack traces or error messages were exposed, but the raw input was present in the returned HTML.

## PCI DSS Requirement
- **C.3.2:** Input data from untrusted sources is never trusted and software security controls are implemented to mitigate the exploitation of vulnerabilities through the manipulation of input data.
- **C.3.2.1:** Industry-standard methods (parameterization, output escaping, input validation) must be used to protect software inputs from attacks.

## Recommendation
- Encode or sanitize all user input before reflecting it in HTML responses, especially in form fields.
- Use server-side HTML encoding libraries or frameworks to ensure special characters are not interpreted as code.
- Review all input fields for similar issues.

---

_This file documents a compliance gap for tracking and remediation._
