using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace KutCode.Cve.Excel.Creators;

public class CveReportCreator
{
    // public static byte[] GetReport(IEnumerable<ReportDbModel> models)
    // {
    //     ExcelPackage package = new ();
    //
    //     var sheet = package.Workbook.Worksheets.Add("Report");
    //
    //     int colCounter = 0;
    //     sheet.Cells[1, ++colCounter].Value = "Название";
    //     sheet.Cells[1, ++colCounter].Value = "CVE";
    //     sheet.Cells[1, ++colCounter].Value = "Уровень угрозы";
    //     sheet.Cells[1, ++colCounter].Value = "Решение";
    //     sheet.Cells[1, ++colCounter].Value = "Версия сервера";
    //     sheet.Cells[1, ++colCounter].Value = "Ссылка для ознакомления";
    //
    //     var valArr = models.ToArray();
    //     for (int i = 0; i < valArr.Length; i++)
    //     {
    //         colCounter = 0;
    //         int row = i + 2;
    //
    //         bool isKb = long.TryParse(valArr[i].Resolve, out _);
    //         sheet.Cells[row, ++colCounter].Value = valArr[i].Name;
    //         sheet.Cells[row, ++colCounter].Value = $"CVE-{valArr[i].CveCode}";
    //         sheet.Cells[row, ++colCounter].Value = valArr[i].DangerLevel;
    //         sheet.Cells[row, ++colCounter].Value = isKb ? $"KB{valArr[i].Resolve}" : valArr[i].Resolve;
    //         sheet.Cells[row, ++colCounter].Value = valArr[i].Software;
    //         sheet.Cells[row, ++colCounter].Value = isKb ? valArr[i].ArticleUrl : string.Empty;
    //     }
    //
    //     var table = sheet.Tables.Add(new(1, 1, valArr.Length + 1, colCounter), "MainTable");
    //     table.TableStyle = TableStyles.Medium6;
    //
    //     for (int i = 1; i <= colCounter; i++)
    //         sheet.Column(i).AutoFit();
    //     
    //     return package.GetAsByteArray();
    // }
}