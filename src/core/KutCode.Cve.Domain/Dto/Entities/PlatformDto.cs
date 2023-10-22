using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Domain.Dto.Entities;

public sealed record PlatformDto(
	Guid Id, string Name, PlatformType PlatformType);