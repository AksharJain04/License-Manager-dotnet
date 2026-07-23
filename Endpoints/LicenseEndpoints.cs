using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using Fare;
using LicenseGenerator.Api.Models;
using Microsoft.EntityFrameworkCore;
using LicenseGenerator.Api.Dto;

namespace LicenseGenerator.Api.Endpoints;

public static class LicensesEndpoints {
    const string GetLicensesEndpointName = "GetLicenses";
    const string GetLicenseEndpointName = "GetLicense";

    public static void MapLicensesEndpoints(this WebApplication app){

        var group = app.MapGroup("/api/licenses");

    // GET /licenses
        app.MapGet("/api/licenselist", async (LicenseGeneratorContext context, int page=1, int pageSize=10 ) => {
            var licenses = await context.Licenses
                                        .FromSqlInterpolated($"EXEC GetPagedLicenses @Page={page}, @PageSize={pageSize}")
                                        .ToListAsync();
            var countResult = await context.LicenseCounts
                                           .FromSqlRaw("EXEC GetLicenseCount")
                                           .ToListAsync();
            var totalRecords = countResult.First().TotalRecords;                  

            var result = new PageResultsDto<License>(
                licenses,
                page,
                pageSize,
                totalRecords,
                (int)Math.Ceiling( totalRecords/(double)pageSize )
            );

            return Results.Ok(result);
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "LicenseManager"))
        .WithName(GetLicensesEndpointName);
        

    // GET /licenses/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) =>
        {
            var licenses = context.Licenses
                                  .FirstOrDefault( i => i.LicenseID == id);
            return licenses is null? Results.NotFound() : Results.Ok(licenses);
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "LicenseManager", "Customer"))
        .WithName(GetLicenseEndpointName);


    // POST /licenses
        group.MapPost("/", (CreateLicenseDto dto, LicenseGeneratorContext context) => {

            var mappingExists = context.InvoiceDeviceMappings.Any(m =>
                m.InvoiceID == dto.InvoiceID &&
                m.SerialNumber == dto.SerialNumber);
            if (!mappingExists) {
                return Results.BadRequest(
                    "This serial number was not sold on the specified invoice."
                );
            }

            var alreadyLicensed = context.Licenses.Any(l =>
                l.InvoiceID == dto.InvoiceID &&
                l.SerialNumber == dto.SerialNumber);
            if (alreadyLicensed) {
                return Results.BadRequest(
                    "A license already exists for this Invoice Number and Serial Number."
                );
            }

            string pattern = "^[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}$";
            Xeger xeger = new Xeger(pattern);
            string licensekey = xeger.Generate();
            string licenseid = $"LIC-{Random.Shared.Next(10000000, 99999999)}";

            var licenses = new License {
                LicenseID = licenseid,
                LicenseKey = licensekey,
                InvoiceID = dto.InvoiceID,
                SerialNumber = dto.SerialNumber,
                ActivationDate = dto.Activation_Date,
                ExpirationDate = dto.Expiration_Date,
                ActivationStatus = dto.ActivationStatus
            };
            try {
                context.Licenses.Add(licenses);
                context.SaveChanges();
            } 
            catch {
                return Results.Conflict("A license already exists for this Invoice Number and Serial Number.");
            }
                
            return Results.CreatedAtRoute(
                GetLicenseEndpointName,
                new { id = licenses.LicenseID },
                new LicenseDto(
                    licenses.LicenseID,
                    licenses.LicenseKey,
                    licenses.InvoiceID,
                    licenses.SerialNumber,
                    licenses.ActivationDate,
                    licenses.ExpirationDate,  
                    licenses.ActivationStatus
                )
            );
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "LicenseManager"));


    // License Re-activation/Updation PATCH /licenses/activate
        group.MapPatch("/activate/{id}", (string id, UpdateLicenseStatusDto dto, LicenseGeneratorContext context) => {
            var licenses = context.Licenses.Find(id);

            if (licenses is null) return Results.NotFound();
            if (licenses.ActivationStatus == dto.ActivationStatus)
                return Results.Conflict("License already has the current status.");

            if(dto.ActivationStatus == "Active"){
                licenses.ActivationStatus = dto.ActivationStatus;
                licenses.ActivationDate = DateOnly.FromDateTime(DateTime.Today);
            }else{
                licenses.ActivationStatus = dto.ActivationStatus;
            }
            context.SaveChanges();
            return Results.Ok(licenses);
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "LicenseManager"));


    // PATCH /licenses/id
        group.MapPatch("/{id}", (string id, UpdateLicenseExpiryDto dto, LicenseGeneratorContext context) => {
            var licenses = context.Licenses.Find(id);
            if (licenses is null) return Results.NotFound();

            licenses.ExpirationDate = dto.ExpirationDate;
            context.SaveChanges();
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "LicenseManager"));
    

    // DELETE /licenses/id
        group.MapDelete("/{id}", (string id, LicenseGeneratorContext context) => {
            var licenses = context.Licenses.Find(id);
            if (licenses is null) return Results.NotFound();

            licenses.ActivationStatus = "Expired";
            context.SaveChanges();
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "LicenseManager"));
        
    }
}
