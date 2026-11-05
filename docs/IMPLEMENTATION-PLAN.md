# Implementation plan and evidence checklist

## Milestone A - HAPI
- [ ] Docker Desktop running with Linux containers
- [ ] PostgreSQL healthy
- [ ] HAPI container healthy
- [ ] `GET /fhir/metadata` returns a FHIR R4 CapabilityStatement
- [ ] Record exact image tag/digest and server version

## Milestone B - .NET
- [ ] `dotnet restore` succeeds
- [ ] API starts and Swagger is available
- [ ] Create Patient through .NET API
- [ ] Read Patient through .NET API
- [ ] Search Patient by identifier
- [ ] Create Encounter referencing Patient/{id}
- [ ] Create Observation referencing Patient/{id} and Encounter/{id}
- [ ] Add negative tests (bad reference, missing identifier, HAPI unavailable)

## Milestone C - eCRNow
- [ ] Clone official eCRNow and UI repositories
- [ ] Read deployment guide and supported configuration
- [ ] Identify EHR FHIR endpoint and auth configuration
- [ ] Configure HAPI only where compatible with the eCRNow EHR interface
- [ ] Configure eRSD/reporting artifacts using authorized sources
- [ ] Demonstrate a synthetic trigger workflow and capture logs/artifacts
- [ ] Document gaps between the demo HAPI data and eCRNow required EHR capabilities

## Milestone D - Inferno and UDS+
- [ ] Record UDS+ IG canonical URL and version
- [ ] List required UDS+ profiles, value sets, extensions and operations
- [ ] Determine which Inferno test kit covers each requirement
- [ ] Run applicable US Core tests separately from UDS+ validation
- [ ] Build a traceability matrix for UDS+ requirements not covered by a test kit
- [ ] Save test logs, version numbers and screenshots (synthetic data only)

## Non-functional acceptance
- [ ] No real PHI in local/demo environment
- [ ] No secrets committed
- [ ] Authentication and authorization design documented
- [ ] TLS, audit, rate limiting, retries and idempotency design documented
- [ ] Docker image versions pinned before final evaluation
