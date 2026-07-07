using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record CreateDeviceDto(
    [Required] string SerialNumber,
    [Required] string DeviceID,
    [Required] string ModelID,
    [Required] DateOnly SaleDate,
    [Required] string DeviceStatus
);