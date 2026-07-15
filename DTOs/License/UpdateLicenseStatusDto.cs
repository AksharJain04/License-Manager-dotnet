using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record UpdateLicenseStatusDto(
    [Required] string ActivationStatus
);