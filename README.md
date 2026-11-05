# Healthcare Interoperability Technical Assessment

A Windows-friendly starter implementation for an interoperability evaluation using:
- HAPI FHIR JPA Server (FHIR R4)
- PostgreSQL
- ASP.NET Core Web API + Firely .NET SDK
- eCRNow (upstream project, cloned separately)
- Inferno US Core Test Kit (upstream project, cloned separately)
- HRSA UDS+ FHIR Implementation Guide (documentation/specification)

> This repository is an integration scaffold, not a claim of validated eCRNow-to-HAPI interoperability or UDS+ conformance. Those outcomes must be demonstrated with the actual upstream versions, configuration and test evidence.

## Architecture

```text
EHR / test client
     |
     v
eCRNow (Java / Spring Boot) ---- FHIR R4 ----+
                                             |
                                             v
                                  HAPI FHIR R4 Server
                                             |
                                      PostgreSQL
                                             ^
                                             |
                                ASP.NET Core Integration API
                                + Firely .NET SDK

Inferno test client ---> HAPI FHIR endpoint
UDS+ profiles/IG ------> validation and reporting analysis
```

## Prerequisites (Windows 10/11)

Install:
- Git for Windows
- Docker Desktop (Linux containers, WSL2 backend)
- .NET 8 SDK (or the SDK required by the selected Firely package version)
- Java 17 and Maven (for eCRNow; verify upstream README)
- PowerShell 7 recommended

Check:
```powershell
git --version
docker version
dotnet --info
java -version
mvn -version
```

## 1. Start HAPI FHIR R4 + PostgreSQL

From repository root:
```powershell
docker compose -f infrastructure/docker-compose.yml up -d
docker compose -f infrastructure/docker-compose.yml ps
```

FHIR base URL:
`http://localhost:8080/fhir`

Capability statement:
```powershell
Invoke-RestMethod http://localhost:8080/fhir/metadata
```

Stop:
```powershell
docker compose -f infrastructure/docker-compose.yml down
```

Remove local database volume (destructive):
```powershell
docker compose -f infrastructure/docker-compose.yml down -v
```

## 2. Run .NET integration API

```powershell
dotnet restore .\dotnet\Healthcare.Integration.sln
dotnet run --project .\dotnet\src\Healthcare.Api --urls http://localhost:5080
```

Swagger:
`http://localhost:5080/swagger`

Set HAPI URL through environment variable:
```powershell
$env:Fhir__BaseUrl = "http://localhost:8080/fhir"
```

## 3. Demonstrate FHIR operations

Create patient:
```powershell
$patient = @{
  resourceType = "Patient"
  identifier = @(@{ system = "http://demo.example.org/mrn"; value = "MRN-10001" })
  name = @(@{ family = "Sharma"; given = @("Rahul") })
  gender = "male"
  birthDate = "1985-05-15"
} | ConvertTo-Json -Depth 20

Invoke-RestMethod -Method Post -Uri "http://localhost:5080/api/fhir/patients" `
  -ContentType "application/fhir+json" -Body $patient
```

Search patients:
```powershell
Invoke-RestMethod "http://localhost:5080/api/fhir/patients?identifier=MRN-10001"
```

Retrieve patient:
```powershell
Invoke-RestMethod "http://localhost:5080/api/fhir/patients/1"
```

Create an encounter (replace patient ID with the ID returned above):
```powershell
$encounter = @{
  resourceType = "Encounter"
  status = "finished"
  class = @{ system = "http://terminology.hl7.org/CodeSystem/v3-ActCode"; code = "AMB" }
  subject = @{ reference = "Patient/1" }
  period = @{ start = "2026-11-02T09:00:00Z"; end = "2026-11-02T09:30:00Z" }
} | ConvertTo-Json -Depth 20

Invoke-RestMethod -Method Post -Uri "http://localhost:5080/api/fhir/encounters" `
  -ContentType "application/fhir+json" -Body $encounter
```

## 4. Upstream projects

Clone separately; don't vendor their code unless license and attribution requirements are met:
```powershell
New-Item -ItemType Directory -Force .\upstream | Out-Null
git clone https://github.com/drajer-health/eCRNow.git .\upstream\eCRNow
git clone https://github.com/drajer-health/eCRNow-UI.git .\upstream\eCRNow-UI
git clone https://github.com/hapifhir/hapi-fhir-jpaserver-starter.git .\upstream\hapi-fhir-jpaserver-starter
git clone https://github.com/inferno-framework/us-core-test-kit.git .\upstream\us-core-test-kit
```

Follow each upstream README and pin a tested tag/commit. Do not assume the default branch has compatible configuration or that eCRNow can use the local HAPI server without additional EHR/eRSD configuration.

## 5. eCRNow integration validation plan

1. Read the eCRNow upstream deployment and configuration documentation.
2. Identify its EHR/FHIR base URL and authentication configuration.
3. Point it at the HAPI endpoint only if the eCRNow workflow supports that interface.
4. Confirm the required FHIR search interactions and supported resource profiles.
5. Configure the required eRSD and reporting artifacts using authorized sources.
6. Trigger a test patient/encounter workflow with synthetic data.
7. Capture application logs, outbound requests and generated eICR artifacts.
8. Record gaps. Do not represent a simple FHIR CRUD test as a completed eCR workflow.

## 6. Inferno and UDS+

- Inferno is a test framework. Select the test suite matching the implementation's declared scope.
- US Core tests do not automatically prove UDS+ conformance.
- Review the current HRSA UDS+ Implementation Guide and its profiles, value sets, terminology and submission requirements.
- Record the exact IG version, test kit version, test configuration, results and unresolved requirements.
- Use synthetic data only.

## Security and privacy

This starter has no production authentication, SMART on FHIR authorization, TLS termination, audit policy, PHI controls, or production hardening. It is for local evaluation with synthetic data only. Never commit secrets or real patient information.


## 7. Publish your own public GitHub repository

Create an empty public repository named `healthcare-interoperability-technical-assessment` in your GitHub account (do not initialize it with a README if you already have this local README), then run:

```powershell
git init
git add .
git commit -m "Initial healthcare interoperability assessment scaffold"
git branch -M main
git remote add origin https://github.com/<YOUR_GITHUB_USERNAME>/healthcare-interoperability-technical-assessment.git
git push -u origin main
```

Do not commit `.env`, passwords, access tokens, real patient information, or generated eICR files containing PHI. Replace local-only database credentials before sharing any deployment publicly.
