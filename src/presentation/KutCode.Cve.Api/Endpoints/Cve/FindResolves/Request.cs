namespace KutCode.Cve.Api.Endpoints.Cve.FindResolves;

public sealed class Request
{
	public IEnumerable<RequestItem> Items { get; set; }
	public sealed class RequestItem
	{
		public string CveId { get; set; }
		public string SourceCode { get; set; }
	}
}