using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LicenseGenerator.Api.Models;

public class DashboardSummary {
    public int TotalCustomers {get; set;}
    public int PendingMappings {get; set;}
    public int RegisteredDevices {get; set;}
    public int ActiveLicenses {get; set;}
    public int InactiveLicenses {get; set;}
    public int SuspendedLicenses {get; set;}
}
