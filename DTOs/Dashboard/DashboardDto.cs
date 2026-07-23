namespace LicenseGenerator.Api.Dtos;

public record DashboardDto (
    int TotalCustomers,
    int PendingMappings,
    int RegisteredDevices,
    int ActiveLicenses,
    int InactiveLicenses,
    int SuspendedLicenses
);