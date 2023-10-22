namespace KutCode.Cve.Domain.Enums;

[Flags]
public enum PlatformType
{
	Unknown = 1,
	Windows = 2,
	Linux = 4,
	Apple = 8,
}