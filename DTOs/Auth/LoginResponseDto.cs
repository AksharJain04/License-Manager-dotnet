namespace LicenseGenerator.Api.Dtos;

public class LoginResponseDto {
    public string? Token { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}