using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;

namespace LicenseGenerator.Api.Endpoints;

public static class CustomerEndpoints{   
    const string GetCustomersEndpointName = "GetCustomers";
    const string GetCustomerEndpointName = "GetCustomer";
    public static void MapCustomerEndpoints(this WebApplication app) {

        var group = app.MapGroup("/api/customer");

        // GET /customers
        group.MapGet("/", (LicenseGeneratorContext context) => {
            var customer = context.Customers
                                  .Where( c => c.isActive )
                                  .ToList();
        })
        .WithName(GetCustomersEndpointName);


        // GET /customers/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) => {
            var customer = context.Customers
                                  .FirstOrDefault( c => c.CustomerID == id && c.isActive);
            return customer is null? Results.NotFound(): Results.Ok(customer);
        })
        .WithName(GetCustomerEndpointName);


        // POST /customers
        group.MapPost("/", (CreateCustomerDto dto, LicenseGeneratorContext context) => {   
            var lastCustomer = context.Customers
                                      .OrderByDescending(c => c.CustomerID)
                                      .FirstOrDefault();
            int nextNumber = lastCustomer == null? 
                                10000001 : int.Parse(lastCustomer.CustomerID!.Substring(4)) + 1;
            string customerID = $"CID-{nextNumber}";

            var customer = new Customer {
                CustomerID = customerID,
                CustomerEmail = dto.CustomerEmail,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                Company = dto.Company!
            };
            context.Customers.Add(customer);
            context.SaveChanges();

            return Results.CreatedAtRoute(
                GetCustomerEndpointName, 
                new { id = customer.CustomerID }, 
                new CustomerDto(
                    customer.CustomerID,
                    customer.CustomerEmail,
                    customer.CustomerName,
                    customer.CustomerPhone,
                    customer.Company,
                    customer.isActive
            ));
        });


        // PUT /customers/{id}
        group.MapPut("/{id}", (string id, UpdateCustomerDto dto, LicenseGeneratorContext context) => {
            var customer = context.Customers.Find(id);
            if (customer is null) return Results.NotFound();

            customer.CustomerName = dto.CustomerName;
            customer.CustomerEmail = dto.CustomerEmail;
            customer.CustomerPhone = dto.CustomerPhone;
            customer.Company = dto.Company;

            context.SaveChanges();
            return Results.NoContent();
        });


        // DELETE /customers/{id}
        group.MapDelete("/{id}", (string id, LicenseGeneratorContext context) => {
            var customer = context.Customers.Find(id);
            if (customer is null) return Results.NotFound();

            customer.isActive = false;
            context.SaveChanges();
            return Results.NoContent();

        });
    }
}