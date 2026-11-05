# Architecture and design decisions

## Components
- eCRNow: upstream electronic case reporting application, Java/Spring Boot.
- HAPI FHIR: local FHIR R4 server used as a test EHR endpoint.
- PostgreSQL: HAPI persistence.
- Healthcare.Api: ASP.NET Core façade/integration service using Firely R4 models/client.
- Inferno: independent conformance test harness.
- UDS+ IG: reporting specification; profiles and requirements are evaluated separately.

## Important boundary
FHIR CRUD interoperability does not prove that eCRNow can execute its complete eCR workflow against a bare HAPI deployment. eCRNow may require EHR query support, authentication, trigger code configuration, eRSD artifacts, CDA generation/validation and downstream routing. Verify these against the exact upstream release.

## Production hardening not included
SMART on FHIR OAuth2, TLS, secrets management, audit event persistence, PHI-safe logging, tenant isolation, rate limiting, retry/backoff, idempotency, monitoring and disaster recovery require explicit implementation and validation.
