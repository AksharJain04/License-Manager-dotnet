using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public record UpdateDeviceDto(
    string DeviceStatus
);