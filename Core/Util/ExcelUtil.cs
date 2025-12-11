using System.Globalization;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

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
}