
namespace KutCode.Cve.Domain.Dto;

public record PaginationRequest
{
	public int Page { get; init; } = 1;
	public int OnPage { get; set; } = 10;
}