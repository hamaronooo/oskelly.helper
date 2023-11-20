
using Newtonsoft.Json;

namespace KutCode.Cve.Services.ApiRepositories.RedHat.Models;

public sealed class RedhatCveJsonModel
{
	[JsonProperty("title")]
	public string Title { get; set; }

	[JsonProperty("language")]
	public string Language { get; set; }

	[JsonProperty("field_cve_details_text")]
	public RedhatCveJsonFieldCveDetailsText RedhatCveJsonFieldCveDetailsText { get; set; }

	[JsonProperty("field_cve_releases_txt")]
	public RedhatCveJsonFieldCveReleasesTxt RedhatCveJsonFieldCveReleasesTxt { get; set; }
}

public class RedhatCveJsonUnd
{
	[JsonProperty("value")]
	public string Value { get; set; }

	[JsonProperty("object")]
	public List<RedhatCveJsonObject> Object { get; set; }
}

public class RedhatCveJsonAdvisory
{
	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("url")]
	public string Url { get; set; }
}

public class RedhatCveJsonFieldCveDetailsText
{
	[JsonProperty("und")]
	public List<RedhatCveJsonUnd> Und { get; set; }
}

public class RedhatCveJsonFieldCveReleasesTxt
{
	[JsonProperty("und")]
	public List<RedhatCveJsonUnd> Und { get; set; }
}

public class RedhatCveJsonObject
{
	[JsonProperty("cpe")]
	public string Cpe { get; set; }

	[JsonProperty("product")]
	public string Product { get; set; }

	[JsonProperty("advisory")]
	public RedhatCveJsonAdvisory RedhatCveJsonAdvisory { get; set; }

	[JsonProperty("package")]
	public string Package { get; set; }

	[JsonProperty("state")]
	public string State { get; set; }
}