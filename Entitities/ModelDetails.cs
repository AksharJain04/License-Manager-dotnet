using System.ComponentModel.DataAnnotations;
namespace LicenseGenerator.Api.Models;

public class DeviceModels
    {
        [Key] [Required] [StringLength(8)] [RegularExpression("MOD\\d{5}", ErrorMessage = "Model ID must be in the format MOD12345.")] 
            public string? ModelId { get; set; }
        [Required] public string? ModelName { get; set; }
        [Required] public string? Price { get; set; }
        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }