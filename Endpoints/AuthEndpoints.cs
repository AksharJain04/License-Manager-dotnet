using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;
using LicenseGenerator.Api.LoginModels;
using Microsoft.AspNetCore.Identity;

namespace LicenseGenerator.Api.Endpoints;

public static class AuthEndpoints {
    public static void MapAuthEndpoints(this WebApplication app) {

        var group = app.MapGroup("/api/auth");
        
        group.MapPost("/register", async(AuthDto dto, UserManager<ApplicationUser> UserManager) => {
            var user = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
            };

            var result = await UserManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return Results.BadRequest(result.Errors);

            return Results.Ok("User registered successfully");
        });
    }
}