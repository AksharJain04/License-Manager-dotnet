using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;

namespace LicenseGenerator.Api.Endpoints;

public static class MappingEndpoints {
    const string GetMappingEndpoints = "GetMappings";
    public static void MapMappingEndpoints(this WebApplication app){

        var group = app.MapGroup("/api/mapping");

    // GET /mapping
        group.MapGet("/", (LicenseGeneratorContext context) => {
            return Results.Ok(context.InvoiceDeviceMappings.ToList());
        });

    // GET /mapping/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) => {
            var mappings = context.InvoiceDeviceMappings
                                  .Where(m => m.InvoiceID == id)
                                  .ToList();
            return Results.Ok(mappings);
        });

    // POST /mapping
        group.MapPost("/", (CreateInvoiceDeviceMappingDto dto, LicenseGeneratorContext context) => {
            var mappings = new InvoiceDeviceMapping{
                InvoiceID = dto.InvoiceID,
                SerialNumber = dto.SerialNumber
            };
            context.InvoiceDeviceMappings.Add(mappings);
            context.SaveChanges();

            return Results.Created($"/mapping/{mappings.InvoiceID}", mappings);
        });

    // No PUT or DELETE Requests for MappingEndpoints.

    }
}