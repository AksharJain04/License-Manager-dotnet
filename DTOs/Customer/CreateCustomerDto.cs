using System.ComponentModel.DataAnnotations;

namespace LicenseGenerator.Api.Dtos;

public record CreateCustomerDto(
    [Required] string CustomerEmail,
    [Required] string CustomerName,
    [Required] string CustomerPhone,
    string? Company
);