using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record UpdateInvoiceDto(
    string DealerID
);