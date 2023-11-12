
using Newtonsoft.Json;

namespace KutCode.Cve.Services.ApiRepositories.Mitre.Models;

public sealed class MitreCveModel
{
	[JsonProperty("containers")]
	public MitreCveContainer Containers { get; set; }
}
public class MitreCveContainer
{
	[JsonProperty("cna")]
	public MitreCveCna Cna { get; set; }
}

public class MitreCveCna
{
	[JsonProperty("descriptions")]
	public List<MitreCveDescription> Descriptions { get; set; }

	[JsonProperty("references")]
	public List<MitreCveReference> References { get; set; }
}

public class MitreCveDescription
{
	[JsonProperty("lang")]
	public string Lang { get; set; }

	[JsonProperty("value")]
	public string Value { get; set; }
}

public class MitreCveReference
{
	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("tags")]
	public List<string> Tags { get; set; }

	[JsonProperty("url")]
	public string Url { get; set; }
}