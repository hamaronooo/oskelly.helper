namespace KutCode.Cve.Domain.Dto;

public record PaginationResponse<T> : PaginationRequest
{
	public PaginationResponse(int currentPage, int onPage, List<T> items)
	{
		Page = currentPage;
		OnPage = onPage;
		Items = items;
	}
	public PaginationResponse(PaginationRequest request, List<T> items)
	{
		Page = request.Page;
		OnPage = request.OnPage;
		Items = items;
	}

	public List<T> Items { get; init; } = new();
	public int Total => Items.Count;
}