using System.Text.Json.Serialization;

namespace KutCode.Cve.Services.ApiRepositories.Microsoft.Models;

public sealed class MicrosoftKbArticle
{
	[JsonPropertyName("articleName")]
	public string ArticleName { get; set; }

	[JsonPropertyName("articleUrl")]
	public string ArticleUrl { get; set; }

	[JsonPropertyName("downloadName")]
	public string DownloadName { get; set; }

	[JsonPropertyName("downloadUrl")]
	public string DownloadUrl { get; set; }

	[JsonPropertyName("knownIssuesName")]
	public string KnownIssuesName { get; set; }

	[JsonPropertyName("knownIssuesUrl")]
	public string KnownIssuesUrl { get; set; }

	[JsonPropertyName("supercedence")]
	public string Supercedence { get; set; }

	[JsonPropertyName("rebootRequired")]
	public string RebootRequired { get; set; }

	[JsonPropertyName("ordinal")]
	public int Ordinal { get; set; }
    
	[JsonPropertyName("fixedBuildNumber")]
	public string FixedBuildNumber { get; set; }
}