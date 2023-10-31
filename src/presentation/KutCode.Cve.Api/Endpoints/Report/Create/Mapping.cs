
using AutoMapper;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Enums;

namespace KutCode.Cve.Api.Endpoints.Report.Create;

public sealed class Mapping : Profile
{
	public Mapping()
	{
		//todo: create funnny CustomName generator;
		CreateMap<Request, ReportRequestDto>()
			.ForMember(x => x.State,
				opts => opts.MapFrom(x => ReportRequestState.Created));
	}
}