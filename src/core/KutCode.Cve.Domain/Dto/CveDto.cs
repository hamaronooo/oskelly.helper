namespace KutCode.Cve.Domain.Dto;

public sealed record CveDto(
	CveId CveId,
	string? ShortName,
	string? Description,
	double? CVSS = null
);