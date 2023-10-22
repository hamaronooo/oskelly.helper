namespace KutCode.Cve.Api.Endpoints.Cve.FindYearlyResolves;

public sealed class Request
{
	public int Year { get; set; }
	public string SourceCode { get; set; }
}