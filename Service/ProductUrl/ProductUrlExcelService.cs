using Core;
using Core.Database;
using Core.Util;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLog;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Service.ProductUrl;

[Service(ServiceType.Scoped)]
public class ProductUrlExcelService(Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public byte[] GetExcelContent(IList<ProductUrlExcelExportItem> productUrlList)
    {
        string sheetName = "网址";

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
        cell.SetCellValue("类型");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 32 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("产品");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 32 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("网址");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 20 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("账号");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 20 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("密码");

        cell = row.CreateCell(cellIndex);
        sheet.SetColumnWidth(cellIndex++, 96 * 256);
        cell.CellStyle = headerCellStyle;
        cell.SetCellType(CellType.String);
        cell.SetCellValue("备注");


        foreach (var item in productUrlList)
        {
            row = sheet.CreateRow(rowIndex++);
            row.HeightInPoints = rowHeight;

            cellIndex = 0;

            // 类型
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.Type);

            // 产品
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.ProductName);

            // 网址
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.Url);

            // 账号
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.LoginUser);

            // 密码
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.LoginPassword);

            // 备注
            cell = row.CreateCell(cellIndex++);
            cell.CellStyle = contentCellStyle;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(item.Description);
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

        var productUrlImportList = new List<ProductUrlEntity>();

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

            // 类型
            string productUrlTypeName = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(productUrlTypeName))
            {
                break;
            }

            var productUrlType = ProductUrlType.GetByName(productUrlTypeName);
            if (productUrlType == ProductUrlType.None)
            {
                throw new ValidationException($"Excel第 {rowIndex + 1} 行， 指定的网址类型 {productUrlTypeName} 不存在！");
            }

            // 产品名称
            string productName = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();
            var product = db.Product.AsNoTracking()
                .Where(e => e.Name == productName)
                .Take(1)
                .SingleOrDefault();
            if (product is null)
            {
                throw new ValidationException($"Excel第 {rowIndex + 1} 行， 指定的产品名称  {productName} 不存在！");
            }

            long productId = product.Id;

            // 网址
            string url = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(url) == true)
            {
                throw new ValidationException($"Excel第 {rowIndex + 1} 行， 网址不能为空！");
            }

            // 账号
            string loginUser = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(loginUser) == true)
            {
                throw new ValidationException($"Excel第 {rowIndex + 1} 行， 账号不能为空！");
            }

            // 密码
            string loginPassword = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(loginPassword) == true)
            {
                throw new ValidationException($"Excel第 {rowIndex + 1} 行， 密码不能为空！");
            }

            // 备注
            string description = (row.GetCell(cellIndex++)?.StringCellValue ?? string.Empty).Trim();

            productUrlImportList.Add(new ProductUrlEntity {
                Type = productUrlType,
                ProductId = productId,
                Url = url,
                LoginUser = loginUser,
                LoginPassword = loginPassword,
                Description = description,
            });
        }

        if (productUrlImportList.Count == 0)
        {
            throw new ValidationException("网址导入列表为空!");
        }

        // 把耗时的Excel操作排除在数据库事务之外
        Transactional.Wrap(db, () => {
            foreach (var item in productUrlImportList)
            {
                db.ProductUrl.Add(item);
            }

            db.SaveChanges();
        });
    }
}