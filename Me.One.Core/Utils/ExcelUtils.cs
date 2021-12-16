using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Aspose.Cells;

namespace Me.One.Core.Utils
{
    public class ExcelUtils
    {
        public static void InitLicense()
        {
            using var manifestResourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Me.One.Core.SDKs.Aspose.Total.lic");
            new License().SetLicense(manifestResourceStream);
        }

        public static void ExportList<T>(string path, List<T> data, int rowIdx, int colIdx)
        {
            var directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            var workbook = new Workbook();
            workbook.Worksheets[0].Cells.ImportCustomObjects(data, rowIdx, colIdx, new ImportTableOptions
            {
                InsertRows = true
            });
            workbook.Worksheets[0].AutoFitColumns();
            workbook.Save(path);
            workbook.Dispose();
        }

        public static List<T> ReadFile<T>(
            string path,
            string[] property,
            int sheetIdx = 0,
            int rowIdx = 0,
            int colIdx = 0)
            where T : new()
        {
            return ReadFile<T>(path, property, null, sheetIdx, rowIdx, colIdx);
        }

        public static List<T> ReadFile<T>(
            string path,
            string[] property,
            Dictionary<Type, string> formatStyles,
            int sheetIdx = 0,
            int rowIdx = 0,
            int colIdx = 0)
            where T : new()
        {
            var objList = new List<T>();
            var workbook = new Workbook(path);
            var cells = workbook.Worksheets[sheetIdx].Cells;
            var maxDataRow = cells.MaxDataRow;
            var num = property.Length;
            if (cells.Columns.Count < num)
            {
                num = cells.MaxDataColumn;
            }
            for (var row = rowIdx; row <= maxDataRow; ++row)
            {
                var obj1 = new T();
                var type1 = typeof(T);
                for (var column = colIdx; column <= num; ++column)
                {
                    try
                    {
                        var property1 = type1.GetProperty(property[column - colIdx],
                            BindingFlags.Instance | BindingFlags.Public);
                        var obj2 = cells[row, column].Value;
                        if (property1 == null)
                        {
                            continue;
                        }
                        var type2 = property1.PropertyType;
                        if (type2.IsGenericType)
                        {
                            type2 = type2.GenericTypeArguments[0];
                        }

                        try
                        {
                            if (formatStyles != null && formatStyles.ContainsKey(type2))
                            {
                                if ((type2 == typeof(DateTime)) && (obj2 is string strObj2))
                                {
                                    obj2 = DateTime.ParseExact(strObj2, formatStyles[type2], null);
                                }
                            }
                            else
                            {
                                obj2 = Convert.ChangeType(obj2, type2);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            obj2 = Activator.CreateInstance(property1.PropertyType);
                        }

                        property1.SetValue(obj1, obj2, null);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }

                objList.Add(obj1);
            }

            workbook.Dispose();
            return objList;
        }
    }
}