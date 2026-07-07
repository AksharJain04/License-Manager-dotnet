using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record ActivateLicenseDto(
    [Required] string LicenseKey,
    [Required] [RegularExpression("^[A-Z0-9]{8,15}$")] string SerialNumber
);