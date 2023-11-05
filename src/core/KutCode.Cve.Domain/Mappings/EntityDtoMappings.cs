using AutoMapper;
using KutCode.Cve.Domain.Dto.Entities;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Entities;
using KutCode.Cve.Domain.Entities.Report;

namespace KutCode.Cve.Domain.Mappings;

public sealed class EntityDtoMappings : Profile
{
	public EntityDtoMappings()
	{
		CreateMap<CveEntity, CveDto>()
			.ForMember(x =>x.CveId, opts => 
				opts.MapFrom(x => x.CveId));
		CreateMap<SoftwareEntity, SoftwareDto>();
		CreateMap<PlatformEntity, PlatformDto>();
		CreateMap<VulnerabilityPointEntity, VulnerabilityPointDto>();
		CreateMap<CveSolutionEntity, CveSolutionDto>();

		CreateMap<ReportRequestEntity, ReportRequestDto>().ReverseMap();
		CreateMap<ReportRequestEntity, ReportRequestExtendedDto>().ReverseMap();
		CreateMap<ReportRequestVulnerabilityPointEntity, ReportRequestVulnerabilityPointDto>().ReverseMap();
		CreateMap<VulnerabilityPointEntity, VulnerabilityPointDto>();
	}
}