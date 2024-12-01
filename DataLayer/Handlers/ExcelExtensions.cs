using OfficeOpenXml;

namespace DataLayer.Handlers;

public static class ExcelExtensions
{
    private static ExcelWorksheet AddWorkSheet<T>(this ExcelPackage? package, T[] data)
    {
        var type = typeof(T);
        var typeName = type.Name;
        var worksheet = package.Workbook.Worksheets[typeName];
        var typeProperties = type.GetProperties();

        foreach (var property in typeProperties)
        {
            if (property.PropertyType.IsPublic)
                worksheet.Cells[0, 0].Value = property.Name;
        }
        int rowCount, colIndex;
        T item;
        for (var arrayIndex = 0; arrayIndex < data.Count(); arrayIndex++)
        {
            item = data[arrayIndex];
            rowCount = arrayIndex + 1;
            colIndex = 0;
            foreach (var property in typeProperties)
            {
                if (property.PropertyType.IsPublic && property.PropertyType.IsPrimitive)
                {
                    worksheet.Cells[rowCount, colIndex].Value = property.GetValue(item);
                    colIndex++;
                }
            }
        }
        return worksheet;
    }
}
