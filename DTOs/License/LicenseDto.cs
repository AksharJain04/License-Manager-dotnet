using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record LicenseDto(
    string LicenseID,
    string LicenseKey,
    string InvoiceID,
    string SerialNumber,
    DateOnly ActivationDate,
    DateOnly ExpirationDate,
    string ActivationStatus
);