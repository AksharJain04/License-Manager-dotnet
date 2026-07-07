using LicenseGenerator.Api.Endpoints;
using LicenseGenerator.Api.Data;
using Microsoft.EntityFrameworkCore;
using LicenseGenerator.Api.LoginModels;

var builder = WebApplication.CreateBuilder(args);

// Register Services
builder.Services.AddValidation();
builder.Services.AddDbContext<LicenseGeneratorContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LicenseGenerator")));

var app = builder.Build();

// Endpoint registration
app.MapCustomerEndpoints();
app.MapDeviceEndpoints();
app.MapInvoiceEndpoints();
app.MapLicensesEndpoints();
app.MapMappingEndpoints();

app.Run();








