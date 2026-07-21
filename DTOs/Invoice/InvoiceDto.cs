using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record InvoiceDto(
    string InvoiceID,
    string CustomerID,
    string DealerID,
    DateOnly SaleDate,
    decimal Amount
);