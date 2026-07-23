using LicenseGenerator.Api.Dtos;
using LicenseGenerator.Api.Data;
using LicenseGenerator.Api.Models;
using Microsoft.EntityFrameworkCore;
using LicenseGenerator.Api.Dto;

namespace LicenseGenerator.Api.Endpoints;

public static class DeviceEndpoints {
    const string GetDevicesEndpointName = "GetDevices";
    const string GetDeviceEndpointName = "GetDevice";
    public static void MapDeviceEndpoints (this WebApplication app) {

        var group  = app.MapGroup("/api/device");
        
    // GET /devicelist
        app.MapGet("/api/devicelist", async (LicenseGeneratorContext context, int page=1, int pageSize=10 ) => {
            var devices = await context.Devices
                                        .FromSqlInterpolated($"EXEC GetPagedDevices @Page={page}, @PageSize={pageSize}")
                                        .ToListAsync();
            var countResult = await context.DeviceCounts
                                           .FromSqlRaw("EXEC GetDeviceCount")
                                           .ToListAsync();
            var totalRecords = countResult.First().TotalRecords;                  

            var result = new PageResultsDto<Device>(
                devices,
                page,
                pageSize,
                totalRecords,
                (int)Math.Ceiling( totalRecords/(double)pageSize )
            );

            return Results.Ok(result);
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "DeviceManager"))
        .WithName(GetDevicesEndpointName);


    // GET /device/{id}
        group.MapGet("/{id}", (string id, LicenseGeneratorContext context) =>
        {
            var device = context.Devices
                                .FirstOrDefault(d => d.DeviceID == id );
            return device is null? Results.NotFound(device): Results.Ok(device) ;
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "DeviceManager", "Customer"))
        .WithName(GetDeviceEndpointName);


    // POST /device
        group.MapPost("/", (CreateDeviceDto dto, LicenseGeneratorContext context) =>
        {   
            var device = new Device {
                SerialNumber = dto.SerialNumber,
                DeviceID = dto.DeviceId,
                ModelID = dto.ModelId,
                SaleDate = dto.SaleDate,
                DeviceStatus = dto.DeviceStatus
            };
            context.Devices.Add(device);
            context.SaveChanges();

            return Results.CreatedAtRoute(
                GetDeviceEndpointName,
                new { id = device.DeviceID },
                new DeviceDto(
                    device.SerialNumber,
                    device.DeviceID,
                    device.ModelID,
                    device.SaleDate,
                    device.DeviceStatus
            ));
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "DeviceManager"));


    // PATCH /device/{id}
        group.MapPatch("/{id}", (string id, UpdateDeviceDto dto, LicenseGeneratorContext context) => {
            var device = context.Devices.FirstOrDefault(d => d.DeviceID == id );
            if (device is null) return Results.NotFound();

            device.DeviceStatus = dto.deviceStatus;
            context.SaveChanges();
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("SuperAdmin", "Admin", "DeviceManager"));
    }
}