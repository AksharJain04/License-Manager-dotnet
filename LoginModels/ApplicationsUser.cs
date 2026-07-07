using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LicenseGenerator.Api.LoginModels;

public class ApplicationUser : IdentityUser {
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}




