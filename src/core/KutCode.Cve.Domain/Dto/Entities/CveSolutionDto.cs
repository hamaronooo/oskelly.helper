namespace KutCode.Cve.Domain.Dto.Entities;

public sealed record CveSolutionDto
{
	public Guid Id { get; init; }
	public string Info { get; init; }
	public string? Description { get; init; }
	public string? SolutionLink { get; init; }
	public string? DownloadLink { get; init; }
	public string? AdditionalLink { get; init; }
	public Guid VulnerabilityPointId { get; init; }
}