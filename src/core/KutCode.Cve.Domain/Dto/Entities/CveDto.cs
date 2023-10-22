namespace KutCode.Cve.Domain.Dto.Entities;

public sealed record CveDto(
	CveId CveId,
	string? ShortName,
	string? DescriptionEnglish,
	string? DescriptionRussian = null,
	double? CvssMaximumRate = null,
	IEnumerable<VulnerabilityPointDto>? Vulnerabilities = null
);