using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Dtos;

public class LicenseListDto{
    public string? LicenseID {get; set;}
    public string? LicenseKey {get; set;}
    public string? InvoiceID {get; set;}
    public string? SerialNumber {get; set;}
    public DateOnly ActivationDate {get; set;}
    public DateOnly ExpirationDate {get; set;}
    public string? ActivationStatus {get; set;}
};