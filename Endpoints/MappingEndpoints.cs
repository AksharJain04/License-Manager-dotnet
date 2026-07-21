using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;

namespace LicenseGenerator.Api.Endpoints;

public static class MappingEndpoints {
    public static void MapMappingEndpoints(this WebApplication app){

        var group = app.MapGroup("/api/mapping");

    // GET /mapping
        group.MapGet("/", (LicenseGeneratorContext context) => {
            return Results.Ok(context.InvoiceDeviceMappings.ToList());
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "InvoiceManager"));

    // GET /mapping/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) => {
            var mappings = context.InvoiceDeviceMappings
                                  .Where(m => m.InvoiceID == id)
                                  .ToList();
            return Results.Ok(mappings);
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "InvoiceManager"));

    // No POST (here), PUT or DELETE Requests for MappingEndpoints.

    }
}