namespace KutCode.Cve.Domain.Dto.Entities;

public sealed record CveSolutionDto(
	Guid Id,
	string Name,
	string? Description,
	string? SolutionLink,
	string? DownloadLink,
	string? AdditionalLink,
	Guid VulnerabilityPointId
)
{
	
}