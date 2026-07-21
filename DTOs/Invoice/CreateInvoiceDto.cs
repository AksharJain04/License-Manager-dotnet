using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record CreateInvoiceDto(
    string CustomerID,
    string DealerID,
    [Range(0.01, double.MaxValue)] decimal Amount,
    DateOnly SaleDate,
    string SerialNumber
);