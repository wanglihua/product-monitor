using Core;
using Core.Util;
using NLog;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Service.Product;

[Service(ServiceType.Scoped)]
public class ProductExcelService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public byte[] GetExcelContent(IList<ProductExcelExportListItem> productList)
    {
        string sheetName = "产品";

        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet(sheetName);

        int rowHeight = 22;
        ICellStyle headerCellStyle = ExcelUtil.CreateHeaderCellStyle(workbook);
        ICellStyle contentCellStyle = ExcelUtil.CreateContentCellStyle(workbook);

        int rowIndex = 0;

        IRow row = sheet.CreateRow(rowIndex++);
        row.HeightInPoints = rowHeight;

        int cellIndex = 0;

        ICell cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 32 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("产品名称");
        
        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 32 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("产品英文名");
        
        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 32 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("最新版本号");
        
        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 32 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("版本更新时间");

        foreach (var item in productList)
        {
            row = sheet.CreateRow(rowIndex++);
            row.HeightInPoints = rowHeight;

            cellIndex = 0;

            // 产品名称
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.Name);
            
            // 产品英文名
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.EnglishName);
            
            // 最新版本号
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.LatestVersion);
            
            // 版本更新时间
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.VersionUpdateTime);
        }

        using MemoryStream memoryStream = new MemoryStream();
        workbook.Write(memoryStream);
        memoryStream.Flush();

        return memoryStream.ToArray();
    }
}