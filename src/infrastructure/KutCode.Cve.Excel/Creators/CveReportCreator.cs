using System.Drawing;
using KutCode.Cve.Application.Interfaces.Excel;
using KutCode.Cve.Domain.Dto.Entities.Report;
using KutCode.Cve.Domain.Entities;
using KutCode.Cve.Domain.Models.Solution;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace KutCode.Cve.Excel.Creators;

public sealed class CveReportCreator : ICveReportCreator
{
    public async Task<byte[]> CreateExcelReportAsync(
        ReportRequestExtendedDto reportRequest,
        IEnumerable<SolutionFinderResult<VulnerabilityPointEntity>> vulnerabilities,
        CancellationToken ct = default)
    {
        ExcelPackage package = new ();
        var successSheet = package.Workbook.Worksheets.Add("Успешно найдено");
        successSheet.TabColor = Color.LimeGreen;
        var errorSheet = package.Workbook.Worksheets.Add("Не найдено");
        errorSheet.TabColor = Color.Red;

        var vuls = vulnerabilities.ToList();
        List<(ReportRequestVulnerabilityPointDto Requested, SolutionFinderResult<VulnerabilityPointEntity> Resolves)> founds = new();
        List<ReportRequestVulnerabilityPointDto> notFounds = new();
        
        foreach (var reqVul in reportRequest.Vulnerabilities.OrderByDescending(x => x.CveId))
        {
            SolutionFinderResult<VulnerabilityPointEntity>? reqVulResolves = vuls.Where(x =>
                    x.Best.HasValue && x.Best.Value!.CveId == reqVul.CveId)
                .FirstOrDefault();
            if (reqVulResolves is null)
                notFounds.Add(reqVul);
            else founds.Add((reqVul, reqVulResolves));
        }
        FillSuccess(successSheet, founds);
        FillError(errorSheet, notFounds);
        return await package.GetAsByteArrayAsync(ct);
    }

    private void FillSuccess(
        ExcelWorksheet sheet,
        List<(ReportRequestVulnerabilityPointDto Requested, SolutionFinderResult<VulnerabilityPointEntity> Resolves)> founds)
    {
        int row = 1;
        sheet.Cells[row, 1].Value = "CVE";
        sheet.Cells[row, 2].Value = "Requested Software";
        sheet.Cells[row, 3].Value = "Requested Platform";
        sheet.Cells[row, 4].Value = "Software";
        sheet.Cells[row, 5].Value = "Platform";
        sheet.Cells[row, 6].Value = "Solution";
        sheet.Cells[row, 7].Value = "Description";
        sheet.Cells[row, 8].Value = "Solution Link";
        sheet.Cells[row, 9].Value = "Download Link";
        sheet.Cells[row, 10].Value = "Additional Link";
        foreach (var found in founds)
        {
            row++;
            var best = found.Resolves.Best.Value!;
            foreach (var solution in best.CveSolutions)
            {
                sheet.Cells[row, 1].Value = best.CveId.AsString;
                sheet.Cells[row, 2].Value = found.Requested.Software;
                sheet.Cells[row, 3].Value = found.Requested.Platform;
                sheet.Cells[row, 4].Value = best.Software?.Name;
                sheet.Cells[row, 5].Value = best.Platform?.Name;
                sheet.Cells[row, 6].Value = solution.Info;
                sheet.Cells[row, 7].Value = solution.Description;
                sheet.Cells[row, 8].Value = solution.SolutionLink;
                sheet.Cells[row, 9].Value = solution.DownloadLink;
                sheet.Cells[row, 10].Value = solution.AdditionalLink;
            }
        }
        var table = sheet.Tables.Add(new(1, 1, row, 10), "SuccessTable");
        table.TableStyle = TableStyles.Medium6;
        for (int i = 1; i <= 10; i++)
            sheet.Column(i).AutoFit();
        sheet.Column(1).Width = 23;
        sheet.Columns[2,10].Width = 30;
    }

    private void FillError(
        ExcelWorksheet sheet,
        List<ReportRequestVulnerabilityPointDto> notFounds)
    {
        int row = 1;
        sheet.Cells[row, 1].Value = "CVE";
        sheet.Cells[row, 2].Value = "Software";
        sheet.Cells[row, 3].Value = "Platform";
        sheet.Cells[row, 4].Value = "Description";
        foreach (var notFound in notFounds)
        {
            row++;
            sheet.Cells[row, 1].Value = notFound.CveId.AsString;
            sheet.Cells[row, 2].Value = notFound.Software;
            sheet.Cells[row, 3].Value = notFound.Platform;
            sheet.Cells[row, 4].Value = notFound.CveDescription;
        }
        var table = sheet.Tables.Add(new(1, 1, row, 4), "ErrorTable");
        table.TableStyle = TableStyles.Medium6;
        for (int i = 1; i <= 4; i++)
            sheet.Column(i).AutoFit();
        sheet.Column(1).Width = 23;
    }
}