using System.ComponentModel.DataAnnotations;

namespace LicenseGenerator.Api.Dtos;

public record AuthDto(
    [Required] string Username,
    [Required] [EmailAddress] string Email,
    [Required] string Password
);