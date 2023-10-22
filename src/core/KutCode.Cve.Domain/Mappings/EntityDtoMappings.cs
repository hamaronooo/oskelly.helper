using AutoMapper;
using KutCode.Cve.Domain.Dto.Entities;
using KutCode.Cve.Domain.Entities;

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
	}
}