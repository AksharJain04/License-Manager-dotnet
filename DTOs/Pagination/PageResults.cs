namespace LicenseGenerator.Api.Dto;

public record PageResultsDto<T> (
    IEnumerable<T> items,
    int Page,
    int PageSize,
    int TotalRecords,
    int TotalPages
);