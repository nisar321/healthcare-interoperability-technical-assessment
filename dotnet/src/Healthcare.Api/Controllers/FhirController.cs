using Healthcare.Api.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.Api.Controllers;

[ApiController]
[Route("api/fhir")]
public sealed class FhirController(IFhirService fhir) : ControllerBase
{
    [HttpPost("patients")]
    [Consumes("application/fhir+json", "application/json")]
    [Produces("application/fhir+json")]
    public async Task<ActionResult<Patient>> CreatePatient([FromBody] Patient patient)
    {
        if (patient is null) return BadRequest("Patient body is required.");
        var created = await fhir.CreatePatientAsync(patient);
        return CreatedAtAction(nameof(GetPatient), new { id = created.Id }, created);
    }

    [HttpGet("patients/{id}")]
    [Produces("application/fhir+json")]
    public async Task<ActionResult<Patient>> GetPatient(string id)
    {
        var patient = await fhir.GetPatientAsync(id);
        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpGet("patients")]
    [Produces("application/fhir+json")]
    public async Task<ActionResult<Bundle>> SearchPatients([FromQuery] string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier)) return BadRequest("identifier query parameter is required.");
        return Ok(await fhir.SearchPatientsByIdentifierAsync(identifier));
    }

    [HttpPost("encounters")]
    [Consumes("application/fhir+json", "application/json")]
    [Produces("application/fhir+json")]
    public async Task<ActionResult<Encounter>> CreateEncounter([FromBody] Encounter encounter)
    {
        if (encounter is null) return BadRequest("Encounter body is required.");
        var created = await fhir.CreateEncounterAsync(encounter);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpPost("observations")]
    [Consumes("application/fhir+json", "application/json")]
    [Produces("application/fhir+json")]
    public async Task<ActionResult<Observation>> CreateObservation([FromBody] Observation observation)
    {
        if (observation is null) return BadRequest("Observation body is required.");
        var created = await fhir.CreateObservationAsync(observation);
        return StatusCode(StatusCodes.Status201Created, created);
    }
}
