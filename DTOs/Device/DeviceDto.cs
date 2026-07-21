using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record DeviceDto(
    string DeviceId,
    string SerialNumber,
    string ModelId,
    DateOnly SaleDate,
    string DeviceStatus
);