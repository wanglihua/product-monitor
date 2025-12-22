using System.Globalization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Core.Util;

public static class ExcelUtil
{
    public static void SetRowStyle(IRow row)
    {
    }

    public static ICellStyle CreateHeaderCellStyle(IWorkbook workbook, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();
        cellStyle.Alignment = horizontalAlignment;
        cellStyle.VerticalAlignment = VerticalAlignment.Center;
        cellStyle.BorderLeft = BorderStyle.Thin;
        cellStyle.BorderTop = BorderStyle.Thin;
        cellStyle.BorderRight = BorderStyle.Thin;
        cellStyle.BorderBottom = BorderStyle.Thin;
        cellStyle.WrapText = true;

        IFont font = workbook.CreateFont();
        font.FontName = "微软雅黑";
        // font.Boldweight = (short)FontBoldWeight.Bold;
        font.IsBold = true;
        font.FontHeightInPoints = 10;

        cellStyle.SetFont(font);

        return cellStyle;
    }

    public static ICellStyle CreateHeaderCellNoBorderStyle(IWorkbook workbook, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();
        cellStyle.Alignment = horizontalAlignment;
        cellStyle.VerticalAlignment = VerticalAlignment.Center;
        cellStyle.WrapText = true;

        IFont font = workbook.CreateFont();
        font.FontName = "微软雅黑";
        // sfont.Boldweight = (short)FontBoldWeight.Bold;
        font.IsBold = true;
        font.FontHeightInPoints = 10;

        cellStyle.SetFont(font);

        return cellStyle;
    }

    public static ICellStyle CreateContentCellStyle(IWorkbook workbook, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();
        cellStyle.Alignment = horizontalAlignment;
        cellStyle.VerticalAlignment = VerticalAlignment.Center;
        cellStyle.BorderLeft = BorderStyle.Thin;
        cellStyle.BorderTop = BorderStyle.Thin;
        cellStyle.BorderRight = BorderStyle.Thin;
        cellStyle.BorderBottom = BorderStyle.Thin;
        cellStyle.WrapText = true;

        IFont font = workbook.CreateFont();
        font.FontName = "微软雅黑";
        // font.Boldweight = (short)FontBoldWeight.Normal;
        font.IsBold = false;
        font.FontHeightInPoints = 10;

        cellStyle.SetFont(font);

        return cellStyle;
    }

    public static IRow CopyRow(ISheet sheet, int sourceRowIndex, int targetRowIndex)
    {
        if (sourceRowIndex == targetRowIndex)
        {
            throw new ArgumentException("sourceIndex and targetIndex cannot be same");
        }

        IRow targetRow = sheet.GetRow(targetRowIndex);
        IRow sourceRow = sheet.GetRow(sourceRowIndex);
        if (targetRow is not null)
        {
            sheet.ShiftRows(targetRowIndex, sheet.LastRowNum, 1, true, false);
        }

        IRow newRow = sheet.CreateRow(targetRowIndex);
        newRow.Height = sourceRow.Height;
        for (int cellNum = (int)sourceRow.FirstCellNum; cellNum < (int)sourceRow.LastCellNum; ++cellNum)
        {
            ICell sourceCell = sourceRow.GetCell(cellNum);
            if (sourceCell is not null)
            {
                ICell newCell = newRow.CreateCell(cellNum);

                if (sourceCell.CellStyle is not null)
                {
                    newCell.CellStyle = sourceCell.CellStyle;
                }

                if (sourceCell.CellComment is not null)
                {
                    newCell.CellComment = sourceCell.CellComment;
                }

                if (sourceCell.Hyperlink is not null)
                {
                    newCell.Hyperlink = sourceCell.Hyperlink;
                }

                newCell.SetCellType(sourceCell.CellType);

                switch (sourceCell.CellType)
                {
                    case CellType.Numeric:
                        newCell.SetCellValue(sourceCell.NumericCellValue);
                        continue;
                    case CellType.String:
                        newCell.SetCellValue(sourceCell.RichStringCellValue);
                        continue;
                    case CellType.Formula:
                        newCell.SetCellFormula(sourceCell.CellFormula);
                        continue;
                    case CellType.Blank:
                        newCell.SetCellValue(sourceCell.StringCellValue);
                        continue;
                    case CellType.Boolean:
                        newCell.SetCellValue(sourceCell.BooleanCellValue);
                        continue;
                    case CellType.Error:
                        newCell.SetCellErrorValue(sourceCell.ErrorCellValue);
                        continue;
                    default:
                        continue;
                }
            }
        }

        for (int index = 0; index < sheet.NumMergedRegions; ++index)
        {
            CellRangeAddress mergedRegion = sheet.GetMergedRegion(index);

            if (mergedRegion is null)
            {
                continue;
            }

            if (mergedRegion.FirstRow == sourceRow.RowNum)
            {
                CellRangeAddress region = new CellRangeAddress(newRow.RowNum, newRow.RowNum + (mergedRegion.LastRow - mergedRegion.FirstRow), mergedRegion.FirstColumn, mergedRegion.LastColumn);
                sheet.AddMergedRegion(region);
            }
        }

        return newRow;
    }

    public static string GetCellValue(ICell cell)
    {
        if (cell is null)
        {
            return "";
        }

        if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell) == true)
        {
            return cell.DateCellValue?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
        }

        cell.SetCellType(CellType.String);
        return (cell.StringCellValue ?? "").Trim();
    }

    public static void SetCellDropdownLongList(ISheet sheet, int colIndex, string[] valueList, string newSheetName)
    {
        IWorkbook workbook = sheet.Workbook;

        int inputRowIndexBegin = 1; // 跳过标题列
        // int inputRowIndexEnd = 65535;
        int inputRowIndexEnd = 10000; // 一万个足足够了

        // 下面几行就是为了创建一个新的工作表名称
        // int sheetCount = workbook.NumberOfSheets;
        // int newSheetIndex = sheetCount;
        // string newSheetName = $"sheet_hidden_for_select_{newSheetIndex}";

        //先创建一个Sheet专门用于存储下拉项的值
        ISheet newDataSheet = workbook.CreateSheet(newSheetName);
        newDataSheet.SetColumnWidth(0, 60 * 256);

        // 隐藏工作表，暂时不隐藏吧
        // workbook.SetSheetHidden(newSheetIndex, SheetVisibility.VeryHidden);

        int index = 0;
        foreach (var item in valueList)
        {
            newDataSheet.CreateRow(index).CreateCell(0).SetCellValue(item);
            index++;
        }

        //创建的下拉项的区域：
        var rangeName = newSheetName + "Range";
        IName range = workbook.CreateName();
        range.RefersToFormula = newSheetName + "!$A$1:$A$" + index;
        range.NameName = rangeName;
        CellRangeAddressList regions = new CellRangeAddressList(inputRowIndexBegin, inputRowIndexEnd, colIndex, colIndex);

        IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)sheet);
        IDataValidationConstraint constraint = validationHelper.CreateFormulaListConstraint(rangeName);
        IDataValidation dataValidation = validationHelper.CreateValidation(constraint, regions);
        dataValidation.CreateErrorBox("输入不合法", "请输入或选择下拉列表中的值");
        dataValidation.ShowPromptBox = true;
        sheet.AddValidationData(dataValidation);
    }

    public static void SetCellDropdownShortList(ISheet sheet, int colIndex, string[] valueList)
    {
        // 这种方式只能设置少量数据，数据一多就会出错

        int inputRowIndexBegin = 1; // 跳过标题列
        // int inputRowIndexEnd = 65535;
        int inputRowIndexEnd = 10000; // 一万个足足够了

        CellRangeAddressList regions = new CellRangeAddressList(inputRowIndexBegin, inputRowIndexEnd, colIndex, colIndex);

        IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)sheet);
        IDataValidationConstraint constraint = validationHelper.CreateExplicitListConstraint(valueList);
        IDataValidation dataValidation = validationHelper.CreateValidation(constraint, regions);
        dataValidation.ShowPromptBox = true;
        dataValidation.CreateErrorBox("输入不合法", "请输入或选择下拉列表中的值");
        sheet.AddValidationData(dataValidation);
    }
}