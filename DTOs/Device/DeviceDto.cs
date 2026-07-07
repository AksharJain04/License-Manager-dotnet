using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record DeviceDto(
    string DeviceID,
    string SerialNumber,
    string ModelID,
    DateOnly SaleDate,
    string DeviceStatus
);