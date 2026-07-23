using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LicenseGenerator.Api.Endpoints;

public static class DashboardEndpoints {
    public static void MapDashboardEndpoints (this WebApplication app)
    {
        var group = app.MapGroup("/api/dashboard");

        group.MapGet("/", async(LicenseGeneratorContext context) =>
        {
            var summary = ( await context.DashboardSummary.FromSqlRaw("EXEC GetDashboardSummary")
                                                        .ToListAsync())
                                                        .First();
            var dto = new DashboardDto(
                summary.TotalCustomers,
                summary.PendingMappings,
                summary.RegisteredDevices,
                summary.ActiveLicenses,
                summary.InactiveLicenses,
                summary.SuspendedLicenses
            );
            return Results.Ok(dto);
        });
    }
}