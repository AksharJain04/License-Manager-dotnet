using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Models;

public class Customer
{
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Format.")]
        public string? CustomerEmail {get; set;}
    [Key] [Required] [RegularExpression(@"CID-\\d{8}", ErrorMessage = "Enter a Valid Customer ID.")] 
        public string? CustomerID {get; set;}
    [Required] public string? CustomerName {get; set;}
    [Required] public string? CustomerPhone {get; set;}
    public string Company {get; set;}= "";
    public bool isActive {get; set;} = true;
}