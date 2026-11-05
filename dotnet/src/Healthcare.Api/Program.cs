using Healthcare.Api.Services;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var fhirBaseUrl = builder.Configuration["Fhir:BaseUrl"]
    ?? "http://localhost:8080/fhir";

builder.Services.AddSingleton(_ =>
{
    var settings = new FhirClientSettings
    {
        PreferredFormat = ResourceFormat.Json 
        // ,
        // PreferredReturn = Prefer.ReturnRepresentation
    };
    return new FhirClient(fhirBaseUrl, settings);
});
builder.Services.AddScoped<IFhirService, FhirService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapControllers();
app.Run();
