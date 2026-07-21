using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record UpdateLicenseExpiryDto(
    [Required] DateOnly ExpirationDate
);