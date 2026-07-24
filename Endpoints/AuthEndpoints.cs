using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;
using LicenseGenerator.Api.LoginModels;
using Microsoft.AspNetCore.Identity;
using LicenseGenerator.Api.Services;
using System.Security.Claims;

namespace LicenseGenerator.Api.Endpoints;

public static class AuthEndpoints {
    public static void MapAuthEndpoints(this WebApplication app) {

        var group = app.MapGroup("/api/auth");
        
        // USER REGISTRATION ENDPOINT
        group.MapPost("/register", async(AuthDto dto, UserManager<ApplicationUser> userManager) => {

            var user = new ApplicationUser {
                UserName = dto.Username,
                Email = dto.Email,
            };
            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return Results.BadRequest(result.Errors);

            await userManager.AddToRoleAsync(user, "Customer");
            return Results.Ok(new {
                message = "User registered Successfully!"
            });
        });


        // LOGIN CREATION ENDPOINT
        group.MapPost("/login", async(LoginDto dto, 
                                      UserManager<ApplicationUser> userManager, 
                                      JwtService jwtService) => {
            var user = await userManager.FindByNameAsync(dto.Username!);
            if (user == null) return Results.Unauthorized();
            var validPassword = await userManager.CheckPasswordAsync(user, dto.Password!);
            if (!validPassword) return Results.Unauthorized();

            var roles = await userManager.GetRolesAsync(user);
            var token = jwtService.GenerateToken(user, roles);

            return Results.Ok(new LoginResponseDto {
                Token = token,
                Username = user.UserName!,
                Email = user.Email!,
                Roles = roles
            });
        });


        // ME ENDPOINT TO BE USED TO CHECK CURRENT LOGIN AFTER EVERY PAGE
        group.MapGet("/me", async(ClaimsPrincipal user, UserManager<ApplicationUser> userManager) =>
        {
            var currentUser = await userManager.GetUserAsync(user);
            if(currentUser == null) return Results.Unauthorized();

            var roles = await userManager.GetRolesAsync(currentUser);

            return Results.Ok(new MeDto(
                currentUser.Id,
                currentUser.UserName!,
                currentUser.Email!,
                roles
            ));
        })
        .RequireAuthorization();
    }
}