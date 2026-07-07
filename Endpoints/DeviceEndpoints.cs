using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;

namespace LicenseGenerator.Api.Endpoints;

public static class DeviceEndpoints {
    const string GetDeviceEndpointName = "GetDevice";
    public static void MapDeviceEndpoints (this WebApplication app) {

        var group  = app.MapGroup("/api/device");
        
    // GET /device
        group.MapGet("/", (LicenseGeneratorContext context) =>
        {
            var device = context.Devices
                                .Where( d => d.DeviceStatus == "Available")
                                .ToList();
            return Results.Ok(device);       
        })
        .WithName(GetDeviceEndpointName);


    // GET /device/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) =>
        {
            var device = context.Devices
                                .FirstOrDefault(d => d.DeviceID == id );
            return device is null? Results.NotFound(device): Results.Ok(device) ;
        })
        .WithName(GetDeviceEndpointName);


    // POST /device
        group.MapPost("/", (string id, CreateDeviceDto dto, LicenseGeneratorContext context) =>
        {   
            var device = new Device {
                DeviceID = dto.DeviceID,
                SerialNumber = dto.SerialNumber,
                ModelID = dto.ModelID,
                SaleDate = dto.SaleDate,
                DeviceStatus = dto.DeviceStatus
            };
            context.Devices.Add(device);
            context.SaveChanges();

            return Results.CreatedAtRoute(
                GetDeviceEndpointName,
                new { id = device.DeviceID },
                new DeviceDto(
                    device.DeviceID,
                    device.SerialNumber,
                    device.ModelID,
                    device.SaleDate,
                    device.DeviceStatus
            ));
        });


    // PUT /device/{id}
        group.MapPut("/{id}", (string id, UpdateDeviceDto dto, LicenseGeneratorContext context) => {
            var device = context.Devices.Find(id);
            if (device is null) return Results.NotFound();

            device.DeviceStatus = dto.DeviceStatus;
            context.SaveChanges();
            return Results.NoContent();
        });


    // POST-SOLD /device/{id}/sold
        group.MapPost("/{id}/sold", (string id, LicenseGeneratorContext context) =>
        {
            var device = context.Devices.Find(id);
            if (device is null) return Results.NotFound();

            device.DeviceStatus = "Sold";
            context.SaveChanges();
            return Results.NoContent();
        });

    // POST-REPLACEMENT /device/{id}/replaced
        group.MapPost("/{id}/replace", (string id, LicenseGeneratorContext context) =>
        {
            var device = context.Devices.Find(id);
            if (device is null) return Results.NotFound();

            device.DeviceStatus = "Replaced";
            context.SaveChanges();
            return Results.NoContent();

    // No DELETE Request defined for devices. 

        });
    }
}