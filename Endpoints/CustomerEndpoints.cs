using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;
using Microsoft.EntityFrameworkCore;
using LicenseGenerator.Api.Dto;

namespace LicenseGenerator.Api.Endpoints;

public static class CustomerEndpoints{   
    const string GetCustomersEndpointName = "GetCustomers";
    const string GetCustomerEndpointName = "GetCustomer";
    public static void MapCustomerEndpoints(this WebApplication app) {

        var group = app.MapGroup("/api/customer");

        // GET /customerlist
        app.MapGet("/api/customerlist", async (LicenseGeneratorContext context, int page=1, int pageSize=10) => {
            var customers = await context.Customers
                                        .FromSqlInterpolated($"EXEC GetPagedCustomers @Page={page}, @PageSize={pageSize}")
                                        .ToListAsync();
            var countResult = await context.CustomerCounts
                                           .FromSqlRaw("EXEC GetCustomerCount")
                                           .ToListAsync();
            var totalRecords = countResult.First().TotalRecords;

            var result = new PageResultsDto<Customer>(
                customers,
                page,
                pageSize,
                totalRecords,
                (int)Math.Ceiling( totalRecords/(double)pageSize )
            );

            return Results.Ok(result);
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "CustomerManager"))
        .WithName(GetCustomersEndpointName);


        // GET /customers/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) => {
            var customer = context.Customers
                                  .FirstOrDefault( c => c.CustomerID == id && c.isActive);
            return customer is null? Results.NotFound(): Results.Ok(customer);
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "CustomerManager", "Customer"))
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
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "CustomerManager"));


        // PATCH /customers/{id}
        group.MapPatch("/{id}", (string id, UpdateCustomerDto dto, LicenseGeneratorContext context) => {
            var customer = context.Customers.Find(id);
            if (customer is null) return Results.NotFound();

            customer.CustomerName = dto.CustomerName;
            customer.CustomerEmail = dto.CustomerEmail;
            customer.CustomerPhone = dto.CustomerPhone;
            customer.Company = dto.Company;

            context.SaveChanges();
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "CustomerManager"));


        // PATCH /customers/{id}/status
        group.MapPatch("/{id}/status", (string id, UpdateCustomerStatusDto dto, LicenseGeneratorContext context) => {
            var customer = context.Customers.Find(id);
            if (customer is null) return Results.NotFound();

            customer.isActive = dto.isActive;

            context.SaveChanges();
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "CustomerManager"));


        // DELETE /customers/{id}
        group.MapDelete("/{id}", (string id, LicenseGeneratorContext context) => {
            var customer = context.Customers.Find(id);
            if (customer is null) return Results.NotFound();

            customer.isActive = false;
            context.SaveChanges();
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "CustomerManager"));
    }
}