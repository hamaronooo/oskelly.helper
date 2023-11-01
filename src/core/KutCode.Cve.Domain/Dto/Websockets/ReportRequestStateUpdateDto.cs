
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Domain.Dto.Websockets;

public sealed record ReportRequestStateUpdateDto(
	Guid ReportId,
	ReportRequestState State,
	int LoadPercent = 0
)
{
	public string StateName => EnumHelper.GetDescriptionValue(State);
}