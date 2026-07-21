namespace LicenseGenerator.Api.Dtos;

public record MeDto (
    string Id,
    string Username,
    string Email,
    IList<string> Roles
);