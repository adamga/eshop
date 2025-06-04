# Third-Party Component Authenticity Verification Process Documentation

## Purpose
To comply with PCI Secure Software Standard C.1.7, the authenticity of all software components and resources fetched from third-party systems or repositories is verified each time they are fetched by the eShop web application and its supporting infrastructure.

## Process
- All .NET dependencies are fetched from the official NuGet.org repository, as defined in `nuget.config`.
- NuGet package signature verification is enabled by default. Only packages signed by trusted authorities are accepted.
- Docker images are pulled from official sources (Docker Hub or trusted vendor registries). Image tags are pinned to specific versions to prevent supply chain attacks.
- For each release, SBOMs are generated for all application and infrastructure components, providing a record of the exact versions and sources of all dependencies.
- The development team reviews and approves all new third-party dependencies and updates via Pull Requests.
- Automated CI/CD checks ensure only trusted sources are used for package and image retrieval.

## Evidence
- `nuget.config` restricts package sources to NuGet.org.
- CI/CD logs and PR history show package and image source verification.
- SBOM files document the origin and version of all dependencies for each release.

## Review
- The process is reviewed periodically to ensure authenticity verification is effective and up-to-date with current tooling and project structure.

## User Guidance for Authenticity Verification

- All developers must ensure new dependencies are only added from trusted sources (NuGet.org for .NET, official Docker Hub or vendor registries for containers).
- When adding a new dependency, verify the package is signed (NuGet) or the image is from a verified publisher (Docker).
- Pin Docker image tags to specific versions; avoid using `latest`.
- Review SBOM output for each release to confirm all dependencies are accounted for and sourced from trusted repositories.
- If a new source or registry is required, update `nuget.config` or CI/CD configuration and document the change in the PR.
- For any questions or exceptions, consult the security lead before merging.

---
_Last updated: 2025-06-03_
