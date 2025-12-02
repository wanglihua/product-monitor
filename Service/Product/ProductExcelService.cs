using Core;
using Core.Util;
using Microsoft.AspNetCore.Http;
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
        sheet.SetColumnWidth(cellIndex++, 48 * 256);
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
        cell.SetCellValue("版本数量");

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

            // 版本数量
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.Numeric);
            cell.SetCellValue(item.VersionCount);

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

    public void Import(IFormFile excelFile)
    {
        if (excelFile == null)
        {
            throw new ValidationException("导入失败，文件未上传！");
        }

        IWorkbook workbook = new XSSFWorkbook(excelFile.OpenReadStream());
        ISheet sheet = workbook.GetSheetAt(0);
        if (sheet == null)
        {
            throw new ValidationException("导入的Excel文件应该至少有一个工作表！");
        }

        var productExcelImportList = new List<ProductExcelImportListItem>();

        // 略过标题行，所以 rowIndex = 1 开始
        int lastRowNum = sheet.LastRowNum;
        for (int rowIndex = 1; rowIndex <= lastRowNum; rowIndex++)
        {
            IRow row = sheet.GetRow(rowIndex);

            if (row == null)
            {
                break;
            }

            // 设置单元格的格式为 String，这样后面用 StringCellValue 取值的时候，就不会出错了
            int cellIndex = 0;
            int lastCellNum = row.LastCellNum;
            for (cellIndex = 0; cellIndex < lastCellNum; cellIndex++)
            {
                ICell cell = row.GetCell(cellIndex);
                if (cell == null)
                {
                    cell = row.CreateCell(cellIndex);
                }

                cell.SetCellType(CellType.String);
            }

            cellIndex = 0;

            // 产品名称
            string name = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                break;
            }

            //  产品英文名称
            string englishName = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();

            productExcelImportList.Add(new ProductExcelImportListItem {
                Name = name,
                EnglishName = englishName,
            });
        }

        if (productExcelImportList.Count == 0)
        {
            throw new ValidationException("产品导入列表为空!");
        }
    }
}