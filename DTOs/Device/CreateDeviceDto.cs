using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record CreateDeviceDto(
    [Required] string SerialNumber,
    [Required] string DeviceId,
    [Required] string ModelId,
    [Required] DateOnly SaleDate,
    [Required] string DeviceStatus
);