using Core;
using Core.Database;
using Core.Util;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLog;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Service.ProductSale;

[Service(ServiceType.Scoped)]
public class ProductSaleExcelService(
    Db db,
    ProductSaleService productSaleService)
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
        cell.SetCellValue("销售时间");

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

            // 销售时间
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
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

    public void Import(IFormFile excelFile)
    {
        if (excelFile is null)
        {
            throw new ValidationException("导入失败，文件未上传！");
        }

        IWorkbook workbook = new XSSFWorkbook(excelFile.OpenReadStream());
        ISheet sheet = workbook.GetSheetAt(0);
        if (sheet is null)
        {
            throw new ValidationException("导入的Excel文件应该至少有一个工作表！");
        }

        var productSaleExcelImportList = new List<ProductSaleExcelImportListItem>();

        // 略过标题行，所以 rowIndex = 1 开始
        int lastRowNum = sheet.LastRowNum;
        for (int rowIndex = 1; rowIndex <= lastRowNum; rowIndex++)
        {
            IRow row = sheet.GetRow(rowIndex);

            if (row is null)
            {
                break;
            }

            // 设置单元格的格式为 String，这样后面用 StringCellValue 取值的时候，就不会出错了
            int cellIndex = 0;
            int lastCellNum = row.LastCellNum;
            for (cellIndex = 0; cellIndex < lastCellNum; cellIndex++)
            {
                ICell cell = row.GetCell(cellIndex);
                if (cell is null)
                {
                    cell = row.CreateCell(cellIndex);
                }

                cell.SetCellType(CellType.String);
            }

            cellIndex = 0;

            // 学校名称
            string collegeName = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(collegeName))
            {
                break;
            }

            var college = db.College.AsNoTracking()
                .Where(e => e.Name == collegeName)
                .Take(1)
                .SingleOrDefault();
            if (college is null)
            {
                throw new ValidationException($"Excel第 {rowIndex + 1} 行，指定的 院校名称 不存在！");
            }

            long collegeId = college.Id;

            // 产品名称
            string productName = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();

            var product = db.Product.AsNoTracking()
                .Where(e => e.Name == productName)
                .Take(1)
                .SingleOrDefault();
            if (product is null)
            {
                throw new ValidationException($"Excel第 {rowIndex + 1} 行，指定的 产品名称 不存在！");
            }

            long productId = product.Id;

            //  版本号
            string version = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();

            // 销售时间
            string saleDateStr = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();
            if (DateTime.TryParse(saleDateStr, out DateTime saleDate) == false)
            {
                throw new ValidationException($"Excel第 {rowIndex + 1} 行，指定的 销售时间 无法识别！");
            }

            // 服务到期时间
            string serviceExpireDateStr = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();
            if (DateTime.TryParse(serviceExpireDateStr, out DateTime serviceExpireDate) == false)
            {
                throw new ValidationException($"Excel第 {rowIndex + 1} 行，指定的 服务到期时间 无法识别！");
            }

            // 客户经理
            string salesManager = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();

            // 联系老师
            string contactTeacher = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();

            // 联系电话
            string contactMobile = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();

            // 备注
            string description = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();

            productSaleExcelImportList.Add(new ProductSaleExcelImportListItem {
                CollegeId = collegeId,
                ProductId = productId,
                Version = version,
                SaleDate = saleDate,
                ServiceExpireDate = serviceExpireDate,
                SalesManager = salesManager,
                ContactTeacher = contactTeacher,
                ContactMobile = contactMobile,
                Description = description,
            });
        }

        if (productSaleExcelImportList.Count == 0)
        {
            throw new ValidationException("产品导入列表为空!");
        }

        // 把耗时的Excel操作排除在数据库事务之外
        Transactional.Wrap(db, () => {
            foreach (var item in productSaleExcelImportList)
            {
                
                
                db.ProductSaleEntry.Add(new ProductSaleEntryEntity {
                });
            }

            db.SaveChanges();
        });
    }
}