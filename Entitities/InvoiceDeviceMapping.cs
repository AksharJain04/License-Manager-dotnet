using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace LicenseGenerator.Api.Models;

public class InvoiceDeviceMapping {
    [Required] [ForeignKey(nameof(Invoice))] public string? InvoiceID {get; set;}
        public Invoice? Invoice {get; set;}
    [Required] [ForeignKey(nameof(Device))] public string? SerialNumber {get; set;}
        public Device? Device {get; set;}
    
}

