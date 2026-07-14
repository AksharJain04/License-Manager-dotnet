using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using Fare;
using LicenseGenerator.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LicenseGenerator.Api.Endpoints;

public static class LicensesEndpoints {
    const string GetLicensesEndpointName = "GetLicenses";
    const string GetLicenseEndpointName = "GetLicense";

    public static void MapLicensesEndpoints(this WebApplication app){

        var group = app.MapGroup("/api/licenses");

    // GET /licenses
        app.MapGet("/api/licenselist", (LicenseGeneratorContext context) => {
            var licenses = context.Licenses.FromSqlRaw("EXEC GetAllLicenses").ToList();
            return Results.Ok(licenses);
        })
        .WithName(GetLicensesEndpointName);
        

    // GET /licenses/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) =>
        {
            var licenses = context.Licenses
                                  .FirstOrDefault( i => i.LicenseID == id);
            return licenses is null? Results.NotFound() : Results.Ok(licenses);
        })
        .WithName(GetLicenseEndpointName);


    // POST /licenses
        group.MapPost("/", (CreateLicenseDto dto, LicenseGeneratorContext context) => {
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
            context.Licenses.Add(licenses);
            context.SaveChanges();

            Console.WriteLine("Saved successfully!");
            Console.WriteLine("About to return response...");

            Console.WriteLine("POST RECEIVED");
            Console.WriteLine($"Invoice : {dto.InvoiceID}");
            Console.WriteLine($"Serial  : {dto.SerialNumber}");
            Console.WriteLine($"Activation : {dto.Activation_Date}");
            Console.WriteLine($"Expiration : {dto.Expiration_Date}");
            Console.WriteLine($"Status : {dto.ActivationStatus}");
                
            return Results.CreatedAtRoute(
                GetLicenseEndpointName,
                new { id = licenses.LicenseID},
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
        });


    // License Re-activation/Updation PATCH /licenses/activate
        group.MapPatch("/activate", (UpdateLicenseStatusDto dto, LicenseGeneratorContext context) => {
            var licenses = context.Licenses.FirstOrDefault(l => l.LicenseKey == dto.LicenseKey);

            if (licenses is null) return Results.NotFound();
            if (licenses.ActivationStatus == "Activated")
                return Results.Conflict("License already activated.");

            if(dto.ActivationStatus == "Activated"){
                licenses.ActivationStatus = dto.ActivationStatus;
                licenses.ActivationDate = DateOnly.FromDateTime(DateTime.Today);
            }else{
                licenses.ActivationStatus = dto.ActivationStatus;
            }
            context.SaveChanges();
            return Results.Ok(licenses);
        });


    // PUT /licenses/id
        group.MapPut("/{id}", (string id, UpdateLicenseDto dto, LicenseGeneratorContext context) => {
            var licenses = context.Licenses.Find(id);
            if (licenses is null) return Results.NotFound();

            licenses.ExpirationDate = dto.ExpirationDate;
            licenses.ActivationStatus = dto.ActivationStatus;
            context.SaveChanges();
            return Results.NoContent();
        });
    

    // DELETE /licenses/id
        group.MapDelete("/{id}", (string id, LicenseGeneratorContext context) => {
            var licenses = context.Licenses.Find(id);
            if (licenses is null) return Results.NotFound();

            licenses.ActivationStatus = "Revoked";
            context.SaveChanges();
            return Results.NoContent();
        });

    }
}
