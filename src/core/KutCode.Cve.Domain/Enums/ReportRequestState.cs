
namespace KutCode.Cve.Domain.Enums;

[Flags]
public enum ReportRequestState
{
	Unknown = 1,
	Created = 2,
	Success = 4,
	Error = 8
}