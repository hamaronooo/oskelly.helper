using KutCode.Cve.Domain.Models;

namespace KutCode.Cve.Domain.Tests;

[TestFixture]
public class CveId_Tests
{
	[TestCase("cve-2022-0001", 2022, "0001")]
	[TestCase("2022-0001", 2022, "0001")]
	[TestCase("CVE-2024-22336", 2024, "22336")]
	[TestCase("    CVE-2024-22336      ", 2024, "22336")]
	public void CveCorrectParsing(string input, int year, string cna)
	{
		var parsed = CveId.Parse(input);
		Assert.IsTrue(parsed.Year == year, parsed.Year.ToString());
		Assert.IsTrue(parsed.CnaNumber == cna, cna);
	}
}