using System.Text.Json.Serialization;

namespace KutCode.Cve.Persistence.ApiRepositories.Microsoft.Models;

public sealed class MicrosoftKbResponse
{
    [JsonPropertyName("value")]
    public List<MicrosoftKbValueItem> Value { get; set; }
}

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

    [JsonPropertyName("affectedBinaries")]
    public List<object> AffectedBinaries { get; set; }
}

public sealed class MicrosoftKbValueItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("releaseDate")]
    public DateTime ReleaseDate { get; set; }

    [JsonPropertyName("releaseNumber")]
    public string ReleaseNumber { get; set; }

    [JsonPropertyName("product")]
    public string Product { get; set; }

    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("productFamily")]
    public string ProductFamily { get; set; }

    [JsonPropertyName("productFamilyId")]
    public int ProductFamilyId { get; set; }

    [JsonPropertyName("platformId")]
    public int PlatformId { get; set; }
    
    [JsonPropertyName("platform")]
    public string Platform { get; set; }

    [JsonPropertyName("cveNumber")]
    public string CveNumber { get; set; }

    [JsonPropertyName("severityId")]
    public int SeverityId { get; set; }

    [JsonPropertyName("severity")]
    public string Severity { get; set; }

    [JsonPropertyName("impactId")]
    public int ImpactId { get; set; }

    [JsonPropertyName("impact")]
    public string Impact { get; set; }

    [JsonPropertyName("issuingCna")]
    public string IssuingCna { get; set; }

    [JsonPropertyName("architectureId")]
    public int ArchitectureId { get; set; }

    [JsonPropertyName("initialReleaseDate")]
    public DateTime InitialReleaseDate { get; set; }

    [JsonPropertyName("baseScore")]
    public string BaseScore { get; set; }

    [JsonPropertyName("temporalScore")]
    public string TemporalScore { get; set; }

    [JsonPropertyName("vectorString")]
    public string VectorString { get; set; }

    [JsonPropertyName("isMariner")]
    public bool IsMariner { get; set; }

    [JsonPropertyName("kbArticles")] public List<MicrosoftKbArticle> KbArticles { get; set; } = new();
}

