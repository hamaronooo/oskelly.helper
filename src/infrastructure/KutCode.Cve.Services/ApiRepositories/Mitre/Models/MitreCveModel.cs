
using Newtonsoft.Json;

namespace KutCode.Cve.Services.ApiRepositories.Mitre.Models;

public sealed class MitreCveModel
{
	[JsonProperty("containers")]
	public ContainersMitreCveModel Containers { get; set; }
}
public class ContainersMitreCveModel
{
	[JsonProperty("cna")]
	public CnaMitreCveModel Cna { get; set; }
}
public class CnaMitreCveModel
{
	[JsonProperty("references")]
	public List<ReferenceMitreCveModel> References { get; set; }
}
public sealed class ReferenceMitreCveModel
{
	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("tags")]
	public List<string> Tags { get; set; }

	[JsonProperty("url")]
	public string Url { get; set; }
}