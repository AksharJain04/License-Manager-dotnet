using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace LicenseGenerator.Api.Models;

public class InvoiceDeviceMapping {
    [Required] public string? InvoiceID {get; set;}
        public Invoice? Invoice {get; set;}
    [Required] public string? SerialNumber {get; set;}
        public Device? Device {get; set;}
    
}

