using LicenseGenerator.Api.Endpoints;
using LicenseGenerator.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LicenseGenerator.Api.LoginModels;

var builder = WebApplication.CreateBuilder(args);

// Register Services
builder.Services.AddValidation();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<LicenseGeneratorContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LicenseGenerator")));

builder.Services.AddDbContext<AuthDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LicenseGenerator"))); 

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDBContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/api/auth/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
});

builder.Services.AddCors(options => {
    options.AddPolicy("Angular", policy => {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("Angular");

// Endpoint registration
app.MapCustomerEndpoints();
app.MapDeviceEndpoints();
app.MapInvoiceEndpoints();
app.MapLicensesEndpoints();
app.MapMappingEndpoints();
app.MapAuthEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.Run();








