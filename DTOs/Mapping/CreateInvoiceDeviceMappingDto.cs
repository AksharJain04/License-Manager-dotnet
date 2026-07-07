using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record CreateInvoiceDeviceMappingDto(
    [Required] string InvoiceID,
    [Required] string SerialNumber
);