using Microsoft.EntityFrameworkCore;
using LicenseGenerator.Api.Models;
using LicenseGenerator.Api.LoginModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LicenseGenerator.Api.Dtos;

namespace LicenseGenerator.Api.Data;

public class LicenseGeneratorContext(DbContextOptions<LicenseGeneratorContext> options) : DbContext(options) {   
    public DbSet<DeviceModels> DeviceModel => Set<DeviceModels>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<License> Licenses => Set<License>();
    public DbSet<InvoiceDeviceMapping> InvoiceDeviceMappings => Set<InvoiceDeviceMapping>();
    public DbSet<LicenseListDto> LicenseList { get; set;}
    public DbSet<DashboardSummary> DashboardSummary => Set<DashboardSummary>();
    public DbSet<LicenseCountDto> LicenseCounts => Set<LicenseCountDto>();
    public DbSet<DeviceCountDto> DeviceCounts => Set<DeviceCountDto>();
    public DbSet<CustomerCountDto> CustomerCounts => Set<CustomerCountDto>();
    public DbSet<InvoiceCountDto> InvoiceCounts => Set<InvoiceCountDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>()
            .Property(l => l.Amount)
            .HasPrecision(10, 2);

        modelBuilder.Entity<InvoiceDeviceMapping>()
                    .HasKey(m => new {
                        m.InvoiceID,
                        m.SerialNumber
                    });

        modelBuilder.Entity<LicenseListDto>().HasNoKey();

        modelBuilder.Entity<DashboardSummary>().HasNoKey();

        modelBuilder.Entity<LicenseCountDto>().HasNoKey();
        modelBuilder.Entity<CustomerCountDto>().HasNoKey();
        modelBuilder.Entity<InvoiceCountDto>().HasNoKey();
        modelBuilder.Entity<DeviceCountDto>().HasNoKey();
    }
}
