using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Models;

public class Invoice {
     [Key] [Required] [StringLength(8)] [RegularExpression("INV-\\d{8}", ErrorMessage = "Invoice ID must be in the format INV-12345678.")]
        public string? InvoiceID {get; set;}
    [Required] public string? CustomerID {get; set;}
    public Customer? Customer {get; set;}
    [Required] public DateOnly SaleDate {get; set;}
    [Required] [RegularExpression(@"DLR-\d{5}", ErrorMessage = "Dealer ID must be in the format DLR-12345.")]
         public string? DealerID {get; set;}
    [Required] public decimal Amount {get; set;}

}