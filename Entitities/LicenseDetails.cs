using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Models;

public class License {
    [Required] [RegularExpression(@"^[A-Z0-9]{5}(-[A-Z0-9]{5}){4}$", ErrorMessage = "License Key is in Invalid Format.")]
        public string? LicenseKey {get; set;}
    [Key] [Required] [RegularExpression(@"LIC-\d{8}", ErrorMessage = "License ID is in Invalid Format.")]
        public string? LicenseID {get; set;}
    public Invoice? Invoice {get; set;}
    [Required] public string? InvoiceID {get; set;}
    public Device? Device {get; set;}
    [Required] public string? SerialNumber {get; set;}
    [Required] public DateOnly ActivationDate {get; set;}
    [Required] public DateOnly ExpirationDate {get; set;}
    private string? status {get; set;}
    [Required] public string? ActivationStatus {
        get => status;
        set
        {
            var allowed = new[] { "Active", "Expired", "Revoked", "Suspended", "Inactive" };
            if (!allowed.Contains(value))
                throw new ArgumentException("Invalid Activation Status Entry!");
            status = value;
        }
    }
}