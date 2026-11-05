using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace Healthcare.Api.Services;

public sealed class FhirService(FhirClient client) : IFhirService
{
    public async Task<Patient> CreatePatientAsync(Patient patient)
    {
        ArgumentNullException.ThrowIfNull(patient);
        // if (patient.ResourceBase.GetType != ResourceType.Patient)
        //     throw new ArgumentException("Resource must be Patient.", nameof(patient));

        var created = await client.CreateAsync(patient);
        return created as Patient
            ?? throw new InvalidOperationException("FHIR server did not return a Patient representation.");
    }

    public async Task<Patient?> GetPatientAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Patient id is required.", nameof(id));
        return await client.ReadAsync<Patient>($"Patient/{Uri.EscapeDataString(id)}");
    }

    public Task<Bundle> SearchPatientsByIdentifierAsync(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier)) throw new ArgumentException("Identifier is required.", nameof(identifier));
        return client.SearchAsync<Patient>(new[] { $"identifier={Uri.EscapeDataString(identifier)}" });
    }

    public async Task<Encounter> CreateEncounterAsync(Encounter encounter)
    {
        ArgumentNullException.ThrowIfNull(encounter);
        var created = await client.CreateAsync(encounter);
        return created as Encounter
            ?? throw new InvalidOperationException("FHIR server did not return an Encounter representation.");
    }

    public async Task<Observation> CreateObservationAsync(Observation observation)
    {
        ArgumentNullException.ThrowIfNull(observation);
        var created = await client.CreateAsync(observation);
        return created as Observation
            ?? throw new InvalidOperationException("FHIR server did not return an Observation representation.");
    }
}
