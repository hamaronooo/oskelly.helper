namespace KutCode.Cve.Domain.Dto;
// под вопросом - стоит ли использовать такую проладку 
public class FinderResultDto
{
	public List<FinderVulnerability> Vulnerabilities { get; set; } = new();
}

public class FinderVulnerability
{
	public double? CvssRate { get; set; }
	public string? DataSource { get; set; }
	
	public string ProductName { get; set; }
	public string PlatformName { get; set; }
	
	public string? ShortName { get; set; }
	public string? Description { get; set; }
	public string? Impact { get; set; }
	public List<FinderResolve> Resolves { get; set; }
}

public sealed class FinderResolve
{
	
}