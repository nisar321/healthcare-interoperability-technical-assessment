using Hl7.Fhir.Model;

namespace Healthcare.Api.Services;

public interface IFhirService
{
    Task<Patient> CreatePatientAsync(Patient patient);
    Task<Patient?> GetPatientAsync(string id);
    Task<Bundle> SearchPatientsByIdentifierAsync(string identifier);
    Task<Encounter> CreateEncounterAsync(Encounter encounter);
    Task<Observation> CreateObservationAsync(Observation observation);
}
