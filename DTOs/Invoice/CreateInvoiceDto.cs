using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record CreateInvoiceDto(
    [Required] string CustomerID,
    [Required] string DealerID,
    [Required] [Range(0.01, double.MaxValue)] decimal Amount,
    [Required] DateOnly SaleDate
);