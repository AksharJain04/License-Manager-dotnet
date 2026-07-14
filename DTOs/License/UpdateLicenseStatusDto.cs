using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record UpdateLicenseStatusDto(
    [Required] [RegularExpression("^[A-Z0-9]{5}(-[A-Z0-9]{5}){4}$")] string LicenseKey,
    [Required] string ActivationStatus
);