using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Models;
using LicenseGenerator.Api.LoginModels;
using Microsoft.AspNetCore.Identity;
using LicenseGenerator.Api.Services;

namespace LicenseGenerator.Api.Endpoints;

public static class UserEndpoints {
    public static void MapUserEndpoints(this WebApplication app) {

        var group = app.MapGroup("/api/users")
                       .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin"));

        // 
        group.MapGet("/", async(UserManager<ApplicationUser> userManager) =>
        {
            var users = userManager.Users.ToList();
            var result = new List<UserDto>();

            foreach(var user in users) {
                var roles = await userManager.GetRolesAsync(user);
                result.Add(new UserDto
                {
                    ID = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = roles
                });
            }
            return Results.Ok(result);
        });

        // 
        group.MapGet("/{id}", async(string id, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user==null) return Results.NotFound("User not found.");
            var roles = await userManager.GetRolesAsync(user);
            var dto = new UserDto
            {
                ID = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles
            };
            return Results.Ok(dto);
        });

        // 
        group.MapPost("/", async(CreateUserDto dto, 
                                 UserManager<ApplicationUser> userManager, 
                                 RoleManager<IdentityRole> roleManager) =>
        {
            if(!await roleManager.RoleExistsAsync(dto.Role!)) return Results.BadRequest("Role does not exist.");
            var existing = await userManager.FindByNameAsync(dto.Username!);
            if(existing!=null) return Results.BadRequest("Username already exists.");

            var user = new ApplicationUser {
                UserName = dto.Username,
                Email = dto.Email
            };

            var createResult = await userManager.CreateAsync(user, dto.Password!);
            if(!createResult.Succeeded) return Results.BadRequest(createResult.Errors);
            await userManager.AddToRoleAsync(user, dto.Role!);

            return Results.Created($"/api/users/{user.Id}", new {
                message = "User created successfully."
            });
        });

        // 
        group.MapPut("/{id}/role", async(string id, UpdateRoleDto dto,
                                         UserManager<ApplicationUser> userManager,
                                         RoleManager<IdentityRole> roleManager) => {
            var user = await userManager.FindByIdAsync(id);
            if(user==null) return Results.NotFound("User not found");
            if(!await roleManager.RoleExistsAsync(dto.Role!)) return Results.BadRequest("Role does not exist.");
            var currentRoles = await userManager.GetRolesAsync(user);

            if (currentRoles.Any()) {
                await userManager.RemoveFromRolesAsync(user, currentRoles);
            }
            await userManager.AddToRoleAsync(user, dto.Role!);
            return Results.Ok(new {
                message = "Role updated successfully."
            });
        });

        // 
        group.MapDelete("/{id}", async(string id, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if(user==null) return Results.NotFound("User not found.");
            var result  = await userManager.DeleteAsync(user);

            if(!result.Succeeded) return Results.BadRequest(result.Errors);
            return Results.Ok(new
            {
                message = "User deleted successfully."
            });
        });
    }
}
