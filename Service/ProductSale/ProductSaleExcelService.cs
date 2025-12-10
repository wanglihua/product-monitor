using Core;
using Core.Util;
using NLog;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Service.ProductSale;

[Service(ServiceType.Scoped)]
public class ProductSaleExcelService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public byte[] Export()
    {
        string sheetName = "销售目录";

        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet(sheetName);

        int rowHeight = 22;
        ICellStyle headerCellStyle = ExcelUtil.CreateHeaderCellStyle(workbook);
        ICellStyle contentCellStyle = ExcelUtil.CreateContentCellStyle(workbook);

        int rowIndex = 0;

        IRow row = sheet.CreateRow(rowIndex++);
        row.HeightInPoints = rowHeight;

        int cellIndex = 0;

        ICell cell;

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 16 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("学校");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 32 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("产品");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 32 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("版本号");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 20 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("服务到期时间");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 20 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("客户经理");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 20 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("联系老师");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 20 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("联系电话");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 96 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("备注");


        for (int index = 0; index < 100; index++)
        {
            row = sheet.CreateRow(rowIndex++);
            row.HeightInPoints = rowHeight;

            cellIndex = 0;

            // 学校
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue("");

            // 产品
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue("");

            // 版本号
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.Numeric);
            cell.SetCellValue("");

            // 服务到期时间
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue("");

            // 客户经理
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue("");
            
            // 联系老师
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue("");
            
            // 联系电话
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue("");
            
            // 备注
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue("");
        }


        using MemoryStream memoryStream = new MemoryStream();
        workbook.Write(memoryStream);
        memoryStream.Flush();

        return memoryStream.ToArray();
    }
}