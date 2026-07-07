using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

// Defines what Angular receives back from the database
public record CustomerDto(
    string CustomerID,
    string CustomerEmail,
    string CustomerName,
    string CustomerPhone,
    string Company,
    bool isActive
);