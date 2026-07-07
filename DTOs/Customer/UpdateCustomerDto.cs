using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record UpdateCustomerDto(
    string CustomerEmail,
    string CustomerName,
    string CustomerPhone,
    string Company
);