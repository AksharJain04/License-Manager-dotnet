using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record InvoiceDto(
    string InvoiceID,
    string CustomerID,
    DateOnly SaleDate,
    string DealerID,
    decimal Amount
);