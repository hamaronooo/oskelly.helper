using FakeItEasy;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Services.ApiRepositories.Microsoft;
using KutCode.Cve.Services.CveResolve;
using KutCode.Cve.Services.CveSolution;

namespace KutCode.Cve.Domain.Tests;

[TestFixture]
public class CveSolutionFinderTests
{
	[Test]
	public async Task SimpleFindTest()
	{
		var finder = new CveSolutionFinder();
		var vulnerabilityPoint = new ReportRequestVulnerabilityPointDto {
			CveYear = 2023, CveCnaNumber = "21799", Software = "Microsoft Windows - Windows Server 2012 R2 Standard (x64)"
		};
		var httpFact = A.Fake<IHttpClientFactory>();
		A.CallTo(() => httpFact.CreateClient(A<string>._)).Returns(new HttpClient());
		var resolver = new MicrosoftCveResolver(new MicrosoftSecurityApiRepository(httpFact));
		var resolves = await resolver.ResolveAsync(vulnerabilityPoint.CveId);
		var result = await finder.FindAsync(vulnerabilityPoint, resolves);
		Assert.IsTrue(result.Resolves.Count > 0);
	}
}