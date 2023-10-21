namespace KutCode.Cve.Domain.Enums;

[Flags]
public enum PlatformType
{
	Windows = 1,
	Linux = 2,
	MacOs = 4,
	Other = 8
}