using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;
using Microsoft.EntityFrameworkCore;
using LicenseGenerator.Api.Dto;

namespace LicenseGenerator.Api.Endpoints;

public static class InvoiceEndpoints {
    const string GetInvoicesEndpointName = "GetInvoices";
    const string GetInvoiceEndpointName = "GetInvoice";
    public static void MapInvoiceEndpoints(this WebApplication app){
        
        var group = app.MapGroup("/api/invoice");

    // GET /invoice
        app.MapGet("/api/invoicelist", async (LicenseGeneratorContext context, int page=1, int pageSize=10) => {
            var invoices = await context.Invoices
                                        .FromSqlInterpolated($"EXEC GetPagedInvoices @Page={page}, @PageSize={pageSize}")
                                        .ToListAsync();
            var countResult = await context.InvoiceCounts
                                           .FromSqlRaw("EXEC GetInvoiceCount")
                                           .ToListAsync();
            var totalRecords = countResult.First().TotalRecords;                  

            var result = new PageResultsDto<Invoice>(
                invoices,
                page,
                pageSize,
                totalRecords,
                (int)Math.Ceiling( totalRecords/(double)pageSize )
            );

            return Results.Ok(result);
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "InvoiceManager"))
        .WithName(GetInvoicesEndpointName);

    // GET /invoice/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) => {
            var invoice = context.Invoices.FirstOrDefault( i => i.InvoiceID == id);
            return invoice is null? Results.NotFound() : Results.Ok(invoice);
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "InvoiceManager", "Customer"))
        .WithName(GetInvoiceEndpointName);


    // POST /invoice
        group.MapPost("/", (CreateInvoiceDto dto, LicenseGeneratorContext context) => {
            var lastInvoice = context.Invoices
                                      .OrderByDescending(i => i.InvoiceID)
                                      .FirstOrDefault();
            int nextNumber = lastInvoice == null? 
                                10000001 : int.Parse(lastInvoice.InvoiceID!.Substring(4)) + 1;
            string invoiceid = $"INV-{nextNumber}";

            var invoice = new Invoice {
                InvoiceID = invoiceid,
                CustomerID = dto.CustomerID,
                DealerID = dto.DealerID,
                SaleDate = dto.SaleDate,
                Amount = dto.Amount
            };
            context.Invoices.Add(invoice);
            context.SaveChanges();

            context.InvoiceDeviceMappings.Add(new InvoiceDeviceMapping {
                InvoiceID = invoiceid,
                SerialNumber = dto.SerialNumber
            });
            context.SaveChanges();

            return Results.CreatedAtRoute(
                GetInvoiceEndpointName,
                new { id = invoice.InvoiceID },
                new InvoiceDto(
                    invoice.InvoiceID,
                    invoice.CustomerID,
                    invoice.DealerID,
                    invoice.SaleDate,
                    invoice.Amount
                ));
            })
            .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "InvoiceManager"));

    // PUT /invoice/{id}
        group.MapPut("/{id}", (string id, UpdateInvoiceDto dto, LicenseGeneratorContext context) => {
            var invoice = context.Invoices.Find(id);
            if (invoice is null) return Results.NotFound();

            invoice.DealerID = dto.DealerID;
            context.SaveChanges();
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "InvoiceManager"));
    
    // No DELETE Request defined for invoices.

    }
}