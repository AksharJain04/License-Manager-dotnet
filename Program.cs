using LicenseGenerator.Api.Endpoints;
using LicenseGenerator.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LicenseGenerator.Api.LoginModels;
using LicenseGenerator.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Jwt");

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

builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthentication ( options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer ( options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("Angular");

app.UseAuthentication();
app.UseAuthorization();

// Endpoint registration
app.MapCustomerEndpoints();
app.MapDeviceEndpoints();
app.MapInvoiceEndpoints();
app.MapLicensesEndpoints();
app.MapMappingEndpoints();
app.MapDashboardEndpoints();
app.MapAuthEndpoints();
app.MapUserEndpoints();

app.Run();








