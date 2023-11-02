using System.ComponentModel;

namespace KutCode.Cve.Domain.Enums;

[Description("Стратегия поиска решений для CVE")]
public enum ReportSearchStrategy
{
	[Description("Только новый поиск")]
	OnlyNew = 10,
	[Description("Поиск только по сохраненным результатам")]
	OnlyStorage = 20,
	[Description("Искать везде")]
	Combine = 30
}