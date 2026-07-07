using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record UpdateLicenseDto(
    DateOnly ExpirationDate,
    string ActivationStatus
);