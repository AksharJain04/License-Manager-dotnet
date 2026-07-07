using Microsoft.EntityFrameworkCore;
using LicenseGenerator.Api.LoginModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LicenseGenerator.Api.Data;

public class AuthDBContext : IdentityDbContext<ApplicationUser> {
    public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options) {}
}