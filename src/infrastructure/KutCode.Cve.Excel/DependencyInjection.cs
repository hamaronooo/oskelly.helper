
using KutCode.Cve.Application.Interfaces.Excel;
using KutCode.Cve.Excel.Parsers;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace KutCode.Cve.Excel;

public static class DependencyInjection
{
	public static IServiceCollection AddExcelServices(this IServiceCollection services)
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		services.AddScoped<IReportRequestParser, ReportRequestParser>();
		return services;
	}
}