namespace KutCode.Cve.Domain.Dto.Entities;

public sealed record CveDto
{
	public CveId CveId { get; init; }
	public string? ShortName { get; init; }
	public string? DescriptionEnglish { get; init; }
	public string? DescriptionRussian { get; init; }
	public double? CvssMaximumRate { get; init; }
	public IEnumerable<VulnerabilityPointDto>? Vulnerabilities { get; init; }
}