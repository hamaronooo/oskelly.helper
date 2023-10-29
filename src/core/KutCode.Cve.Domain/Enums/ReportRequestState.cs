
using System.ComponentModel;

namespace KutCode.Cve.Domain.Enums;

[Flags]
public enum ReportRequestState
{
	[Description("Неизвестно")]
	Unknown = 1,
	[Description("Создан")]
	Created = 2,
	[Description("В обработке")]
	Handling = 4,
	[Description("Успешно обработан")]
	Success = 8,
	[Description("Ошибка обработки")]
	Error = 16
}