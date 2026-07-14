using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;

namespace LicenseGenerator.Api.Endpoints;

public static class InvoiceEndpoints {
    const string GetInvoicesEndpointName = "GetInvoices";
    const string GetInvoiceEndpointName = "GetInvoice";
    public static void MapInvoiceEndpoints(this WebApplication app){
        
        var group = app.MapGroup("/api/invoice");

    // GET /invoice
        group.MapGet("/", (LicenseGeneratorContext context) => {
            var invoice = context.Invoices.Find();
        })
        .WithName(GetInvoicesEndpointName);

    // GET /invoice/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) => {
            var invoice = context.Invoices.FirstOrDefault( i => i.InvoiceID == id);
            return invoice is null? Results.NotFound() : Results.Ok(invoice);
        })
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
                SaleDate = dto.SaleDate,
                DealerID = dto.DealerID,
                Amount = dto.Amount
            };
            context.Invoices.Add(invoice);
            context.SaveChanges();

            return Results.CreatedAtRoute(
                GetInvoiceEndpointName,
                new { id = invoice.InvoiceID },
                new InvoiceDto(
                    invoice.InvoiceID,
                    invoice.CustomerID,
                    invoice.SaleDate,
                    invoice.DealerID,
                    invoice.Amount
                ));
            });

    // PUT /invoice/{id}
        group.MapPut("/{id}", (string id, UpdateInvoiceDto dto, LicenseGeneratorContext context) => {
            var invoice = context.Invoices.Find(id);
            if (invoice is null) return Results.NotFound();

            invoice.DealerID = dto.DealerID;
            context.SaveChanges();
            return Results.NoContent();
        });
    
    // No DELETE Request defined for invoices.

    }
}