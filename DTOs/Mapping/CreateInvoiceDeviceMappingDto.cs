using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record CreateInvoiceDeviceMappingDto(
    [Required] CreateInvoiceDto InvoiceID,
    [Required] string SerialNumber
);