namespace KutCode.Cve.Domain.Dto;

public sealed class CveFinderQueueState
{
	/// <summary>
	/// Cve year -- items count
	/// </summary>
	public Dictionary<int, int> Yearly { get; set; } = new();

	public int Total => Yearly.Sum(x => x.Value);
}