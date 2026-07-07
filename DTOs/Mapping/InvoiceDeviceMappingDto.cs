using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record InvoiceDeviceMappingDto(
    string InvoiceID,
    string SerialNumber
);