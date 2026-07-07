using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Models;

public class Device {

    [Key] [Required] [RegularExpression(@"^[A-Z0-9]{8,15}$", ErrorMessage = "Serial Number is in Invalid Format.")]
        public string? SerialNumber {get; set;}
    [Required] public string? DeviceID {get; set;}
    [Required] public string? ModelID {get; set;}
    public DeviceModels? Model {get; set;}
    [Required] public DateOnly SaleDate {get; set;}
    private string? status;
    [Required] public string? DeviceStatus {
        get => status;
        set {
            var allowed = new[] {"Available", "Sold", "Replaced"};
            if (!allowed.Contains(value))
                throw new ArgumentException("Invalid Status Entry!");
                status = value;
        }
    }
}