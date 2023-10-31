using KutCode.Cve.Application.Interfaces.Excel;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Models;
using Microsoft.VisualBasic.FileIO;
using OfficeOpenXml;

namespace KutCode.Cve.Excel.Parsers;

public sealed class ReportRequestParser : IReportRequestParser
{
	public List<ReportRequestVulnerabilityPointDto> ParseXlsxReportRequestCve(Stream fileStream)
	{
		using ExcelPackage package = new (fileStream);
		var result = new List<ReportRequestVulnerabilityPointDto>();
		var sheet = package.Workbook.Worksheets[0];
		if (sheet is null) return result;
		for (int i = 1; i <= sheet.Dimension.Rows; i++) {
			if (CveId.TryParse(sheet.Cells[i,1].Text.Replace(" ", ""), out var cveId) is false) continue;
			result.Add(new ReportRequestVulnerabilityPointDto {
				CveYear = cveId.Value.Year,
				CveCnaNumber = cveId.Value.CnaNumber,
				Software = sheet.Cells[i,2].Text.Trim(),
				Platform = sheet.Cells[i,3].Text.Trim(),
				CveDescription = sheet.Cells[i,4].Text.Trim()
			});
		}
		return result;
	}

	// public List<ReportRequestVulnerabilityPointDto> ParseCsvReportRequestCve(Stream fileStream)
	// {
	// 	using var textFieldParser = new TextFieldParser(fileStream);
	// 	textFieldParser.TextFieldType = FieldType.Delimited;
	// 	textFieldParser.SetDelimiters(";");
	// 	while (!textFieldParser.EndOfData)
	// 	{
	// 		string[]? rows = textFieldParser.ReadFields();
	// 	}
	// 	var result = new List<ReportRequestVulnerabilityPointDto>();
	// 	return result;
	// }
}
