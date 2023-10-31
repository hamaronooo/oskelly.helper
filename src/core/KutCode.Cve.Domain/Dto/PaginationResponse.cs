namespace KutCode.Cve.Domain.Dto;

public record PaginationResponse<T> : PaginationRequest
{
	public PaginationResponse(int currentPage, int onPage, List<T> items, int total)
	{
		Page = currentPage;
		OnPage = onPage;
		Items = items;
		Total = total;
	}
	public PaginationResponse(PaginationRequest request, List<T> items, int total)
	{
		Page = request.Page;
		OnPage = request.OnPage;
		Items = items;
		Total = total;
	}

	public List<T> Items { get; init; } = new();
	public int Total { get; init; }
}