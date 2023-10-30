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

	[TestCase("CVE-2021-00001", "CVE-2021-0002", -1)]
	[TestCase("CVE-2021-00001", "CVE-2021-0001", 0)]
	[TestCase("CVE-2021-00120", "CVE-2021-0001", 119)]
	[TestCase("CVE-2022-00001", "CVE-2021-02288", 1)]
	[TestCase("CVE-2021-00001", "CVE-2023-02288", -2)]
	[TestCase("CVE-2021-001", "CVE-2021-020", -19)]
	public void IComparableParsing_Test(string cve1, string cve2, int value)
	{
		var c1 = CveId.Parse(cve1);
		var c2 = CveId.Parse(cve2);
		var compareResult = c1.CompareTo(c2);
		Assert.IsTrue(compareResult == value, compareResult.ToString());
	}
}