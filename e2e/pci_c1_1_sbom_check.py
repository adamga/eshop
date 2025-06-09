# PCI C.1.1 SBOM Validation Script
# This script parses sbom.json and summarizes the types of components for PCI evidence.
import json
from collections import Counter

SBOM_PATH = 'c:/Repos/eshop/sbom.json'  # Adjust path if needed

def main():
    with open(SBOM_PATH, 'r', encoding='utf-8') as f:
        sbom = json.load(f)
    components = sbom.get('components', [])
    print(f"Total components in SBOM: {len(components)}\n")
    type_counter = Counter()
    suppliers = set()
    for comp in components:
        ctype = comp.get('type', 'unknown')
        type_counter[ctype] += 1
        if 'author' in comp:
            suppliers.add(comp['author'])
        elif 'authors' in comp:
            for a in comp['authors']:
                suppliers.add(a.get('name', 'unknown'))
    print("Component types:")
    for t, count in type_counter.items():
        print(f"  {t}: {count}")
    print("\nUnique suppliers/authors:")
    for s in sorted(suppliers):
        print(f"  {s}")
    # Optionally, print a sample component
    if components:
        print("\nSample component:")
        print(json.dumps(components[0], indent=2))
    # --- PCI C.1.2: Check for transitive dependencies ---
    # CycloneDX SBOMs may include 'dependencies' section for transitive relationships
    dependencies = sbom.get('dependencies', [])
    if dependencies:
        print(f"\nSBOM includes a 'dependencies' section with {len(dependencies)} entries (transitive relationships).")
        # Print a sample dependency mapping
        print("Sample dependency mapping:")
        print(json.dumps(dependencies[0], indent=2))
    else:
        print("\nWARNING: No 'dependencies' section found in SBOM. Transitive dependencies may not be explicitly listed.")
        print("If your build tooling does not support this, document this as a justified gap for PCI C.1.2.b.")
    # Optionally, check if all components are referenced in dependencies
    referenced = set()
    for dep in dependencies:
        for ref in dep.get('dependsOn', []):
            referenced.add(ref)
    missing = []
    for comp in components:
        if comp.get('bom-ref') not in referenced:
            missing.append(comp.get('name'))
    if missing:
        print(f"\nComponents not referenced as dependencies (may be top-level or missing relationships): {missing[:10]}{'...' if len(missing)>10 else ''}")
    else:
        print("\nAll components are referenced in dependency relationships.")
    # --- PCI C.1.3: Check for environment-level dependencies ---
    # List of common environment-level dependencies to check for
    env_deps = [
        'PostgreSQL', 'Redis', 'RabbitMQ', 'Azure', 'SQL Server', 'MySQL', 'MongoDB',
        'CosmosDB', 'Kafka', 'Elasticsearch', 'OpenAI', 'Duende IdentityServer',
        'YARP', 'gRPC', 'Polly', 'Nginx', 'Traefik', 'Key Vault', 'Blob Storage',
        'Service Bus', 'App Insights', 'Application Insights', 'Maui', 'Ollama', 'Pgvector'
    ]
    found_env_deps = set()
    for comp in components:
        name = comp.get('name', '').lower()
        for dep in env_deps:
            if dep.lower() in name:
                found_env_deps.add(dep)
    print("\nEnvironment-level dependencies detected in SBOM:")
    if found_env_deps:
        for dep in sorted(found_env_deps):
            print(f"  {dep}")
    else:
        print("  None detected. If your production environment includes databases, caches, message brokers, or cloud services, ensure these are documented in the SBOM or justify their absence for PCI C.1.3.")
    # Optionally, print a warning for missing expected dependencies
    missing_env_deps = [dep for dep in env_deps if dep not in found_env_deps]
    if missing_env_deps:
        print("\nWARNING: The following common environment-level dependencies were NOT detected in the SBOM:")
        for dep in missing_env_deps:
            print(f"  {dep}")
        print("If any of these are used in production, document them or justify their absence in the SBOM for PCI C.1.3 compliance.")

if __name__ == '__main__':
    main()
