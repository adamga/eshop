# SBOM Generation and Release Process Documentation

## Purpose
To comply with PCI Secure Software Standard C.1.5, a new Software Bill of Materials (SBOM) is generated for every new release of the eShop web application and its supporting infrastructure components.

## Process
- An SBOM is generated for the main web application using CycloneDX:
  - Command: `dotnet cyclonedx src/WebAppComponents/WebAppComponents.csproj --output . --filename sbom-webappcomponents.xml --recursive`
- For each supporting Docker container (e.g., RabbitMQ, Redis, Postgres), an SBOM is generated using Syft:
  - Example: `syft docker:rabbitmq:latest -o cyclonedx-xml > sbom-rabbitmq.xml`
  - Example: `syft docker:redis:latest -o cyclonedx-xml > sbom-redis.xml`
  - Example: `syft docker:postgres:latest -o cyclonedx-xml > sbom-postgres.xml`
- These SBOM files are versioned and archived with each release build.
- The SBOMs are reviewed to ensure they accurately reflect all direct and transitive dependencies for each release.

## Evidence
- SBOM files (e.g., `sbom-webappcomponents.xml`, `sbom-rabbitmq.xml`, `sbom-redis.xml`, `sbom-postgres.xml`) are stored in the repository and/or release artifacts for each version.
- The presence of these files for each release serves as evidence of compliance with C.1.5.

## Review
- The process is reviewed periodically to ensure SBOM generation is automated and up-to-date with current tooling and project structure.

---
_Last updated: 2025-06-03_
