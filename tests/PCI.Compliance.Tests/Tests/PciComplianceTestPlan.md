# PCI DSS Web Software Module C - Test Plan

This document maps each PCI DSS Module C requirement to the relevant eShop application area and test type. Each requirement will have a corresponding test file in this directory.

| Requirement | Area | Test Type | Test File |
|-------------|------|-----------|----------|
| C.1 SBOM completeness & accuracy | All projects, build pipeline | Static analysis, manual review | SBOMTests.cs |
| C.2 Authentication & Access Control | WebApp, APIs, Identity | Functional, integration | AuthenticationAndAccessControlTests.cs |
| C.3 Input Validation & Attack Mitigation | WebApp, APIs | Unit, integration, DAST | InputValidationAndAttackMitigationTests.cs |
| C.4 Secure Communications (TLS, mutual auth) | WebApp, APIs, config | Integration, config review | SecureCommunicationsTests.cs |
| CORS & HTTP Headers | WebApp, APIs | Integration, config review | CorsAndHttpHeadersTests.cs |
| File Upload Security | WebApp (if applicable) | Integration, DAST | FileUploadSecurityTests.cs |
| Object Deserialization | WebApp, APIs | Unit, integration | ObjectDeserializationTests.cs |
| Rate Limiting & Resource Starvation | APIs, WebApp | Integration, DAST | RateLimitingTests.cs |

Each test file will contain stubs for the relevant PCI requirements. See the PCI DSS Module C for details.
