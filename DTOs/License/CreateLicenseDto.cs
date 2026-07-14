using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record CreateLicenseDto (
    [Required] [StringLength(12)] string InvoiceID,
    [Required] [StringLength(15)] string SerialNumber,
    DateOnly Activation_Date,
    DateOnly Expiration_Date,
    [Required] string ActivationStatus
);