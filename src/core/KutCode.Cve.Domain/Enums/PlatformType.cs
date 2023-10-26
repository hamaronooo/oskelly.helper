using System.ComponentModel;

namespace KutCode.Cve.Domain.Enums;

[Flags]
public enum PlatformType
{
	[Description("Неизвестно")]
	Unknown = 1,
	[Description("Windows")]
	Windows = 2,
	[Description("Linux")]
	Linux = 4,
	[Description("Apple")]
	Apple = 8,
}