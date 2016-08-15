using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VinCode
{
    public class ExcelHelper
    {
        /// <summary>
        /// 从Excel中导入数据到DataTable中
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="columnList">列设置</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ExcelToDataTable(Stream fileStream, List<ExcelColumnData> columnList, string sheetName = "Sheet1")
        {
            return ExcelToDataTable(fileStream, columnList, sheetName, 0);
        }
        /// <summary>
        /// 从Excel中导入数据到DataTable中
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="columnList">列设置</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="headerRowIndex">列头行号</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ExcelToDataTable(Stream fileStream, List<ExcelColumnData> columnList, string sheetName, int headerRowIndex)
        {
            if (columnList == null || columnList.Count == 0)
            {
                throw new Exception("列设置为空，无法继续转换.");
            }
            if (fileStream.Length == 0)
            {
                throw new Exception("文件流为空,无法继续转换.");
            }
            IWorkbook workbook = null;
            try
            {
                workbook = WorkbookFactory.Create(fileStream);
            }
            catch
            {
                throw new Exception("未能解析上传的文件，请检查文件是否正确!");
            }
            if (!string.IsNullOrEmpty(sheetName))
            {
                string firstSheetName = workbook.GetSheetName(0);
                if (!string.IsNullOrEmpty(firstSheetName) && !string.IsNullOrEmpty(sheetName))
                {
                    if (firstSheetName.Equals(sheetName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        sheetName = firstSheetName;
                    }
                }
            }
            ISheet sheet = workbook.GetSheet(sheetName);
            DataTable table = new DataTable();
            DataRow dataRow;
            IRow row;
            ICell cell;
            int cellCount;
            int rowCount = sheet.LastRowNum;
            //headerRowIndex = headerRowIndex < 0 ? 0 : headerRowIndex;

            foreach (ExcelColumnData col in columnList)
            {
                if (!table.Columns.Contains(col.ColumnName))
                {
                    table.Columns.Add(col.ColumnName, Type.GetType(col.DataType.GetEnumDescription(), true, true));
                    table.Columns[col.ColumnName].Caption = col.ColumnDisplay;
                }
            }

            int rowIndex = 0;
            string field = string.Empty;
            for (int i = (headerRowIndex + 1); i <= sheet.LastRowNum; i++)
            {
                rowIndex++;
                if (sheet.GetRow(i) != null)
                {
                    row = sheet.GetRow(i);
                    //判断空行的，实际会影响正常数据，弃用
                    //if (row.LastCellNum > row.PhysicalNumberOfCells)
                    //{
                    //    continue;
                    //}
                    cellCount = row.LastCellNum;
                    dataRow = table.NewRow();
                    try
                    {
                        for (int j = row.FirstCellNum; j <= cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                            {
                                if (j >= table.Columns.Count)
                                {
                                    table.Columns.Add("Column" + j.ToString());
                                }
                                field = table.Columns[j].Caption;
                                cell = row.GetCell(j);
                                switch (cell.CellType)
                                {
                                    case CellType.String:
                                        dataRow[j] = cell.StringCellValue;
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        {
                                            DateTime date = DateTime.FromOADate(cell.NumericCellValue);
                                            if (date < DateTime.Parse("1900-2-28"))
                                            {
                                                if (date < DateTime.MinValue)
                                                {
                                                    date = DateTime.MinValue;
                                                }
                                                else
                                                {
                                                    date = date.AddDays(1);
                                                }
                                            }
                                            dataRow[j] = date;
                                        }
                                        else
                                        {
                                            dataRow[j] = Convert.ToDouble(cell.NumericCellValue);
                                        }
                                        break;
                                    case CellType.Boolean:
                                        dataRow[j] = Convert.ToString(cell.BooleanCellValue);
                                        break;
                                    case CellType.Error:
                                        dataRow[j] = ErrorEval.GetText(cell.ErrorCellValue);
                                        break;
                                    case CellType.Formula:
                                        switch (cell.CachedFormulaResultType)
                                        {
                                            case CellType.String:
                                                dataRow[j] = cell.StringCellValue;
                                                break;
                                            case CellType.Numeric:
                                                dataRow[j] = Convert.ToString(cell.NumericCellValue);
                                                break;
                                            case CellType.Boolean:
                                                dataRow[j] = Convert.ToString(cell.BooleanCellValue);
                                                break;
                                            case CellType.Error:
                                                dataRow[j] = ErrorEval.GetText(cell.ErrorCellValue);
                                                break;
                                            default:
                                                dataRow[j] = DBNull.Value;
                                                break;
                                        }
                                        break;
                                    default:
                                        dataRow[j] = DBNull.Value;
                                        break;
                                }
                            }
                        }
                        if (dataRow.ItemArray.Any(col => !string.IsNullOrEmpty(col.ToString())))//如果存在全部为空的数据，则不导入
                        {
                            table.Rows.Add(dataRow);
                        }
                    }
                    catch
                    {
                        //该行转换失败，需要标记，考虑收集失败行，重新导出Excel
                        //throw;
                        //LogHelper.ErrorLog(0, "表格" + rowIndex.ToString() + "行导入异常：" + ex.Message);
                        if (!string.IsNullOrEmpty(field))
                        {
                            throw new Exception(string.Format("第{0}行数据[{1}]列在转换中出现异常，请检查数据是否异常!", rowIndex, field));
                        }
                        else
                        {
                            throw new Exception("转换Excel文件时出现异常,请检查模板是否匹配！");
                        }
                    }
                }
            }
            return table;
        }
    }

    /// <summary>
    /// 表格列设置
    /// </summary>
    [Serializable]
    public class ExcelColumnData
    {
        public ExcelColumnData() { }
        public ExcelColumnData(string columnDisplay, decimal columnWidth, DataTypeEnum dataType, bool allowEmpty) :
            this(string.Empty, columnDisplay, columnWidth, dataType, allowEmpty, string.Empty)
        {

        }
        public ExcelColumnData(string columnDisplay, decimal columnWidth, DataTypeEnum dataType, bool allowEmpty, string cellComment) :
            this(string.Empty, columnDisplay, columnWidth, dataType, allowEmpty, cellComment)
        {

        }
        public ExcelColumnData(string columnName, string columnDisplay, decimal columnWidth, DataTypeEnum dataType, bool allowEmpty) :
            this(columnName, columnDisplay, columnWidth, dataType, allowEmpty, string.Empty)
        {

        }
        public ExcelColumnData(string columnName, string columnDisplay, decimal columnWidth, DataTypeEnum dataType, bool allowEmpty, string cellComment)
        {
            this.ColumnName = columnName;
            this.ColumnDisplay = columnDisplay;
            this.ColumnWidth = columnWidth;
            this.DataType = dataType;
            this.AllowEmpty = allowEmpty;
            this.CellComment = cellComment;
        }
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 列显示
        /// </summary>
        public string ColumnDisplay { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        public Nullable<SqlDbType> ColumnDataType
        {
            get;
            set;
        }
        private DataTypeEnum dataType = DataTypeEnum.Empty;
        /// <summary>
        /// 列类型
        /// </summary>
        public DataTypeEnum DataType
        {
            get
            {
                if (ColumnDataType.HasValue)
                {
                    return ToDataType(ColumnDataType.Value);
                }
                return dataType;
            }
            set
            {
                this.dataType = value;
            }
        }
        /// <summary>
        /// 列宽
        /// </summary>
        public decimal ColumnWidth { get; set; }
        /// <summary>
        /// 计算列宽
        /// </summary>
        public decimal DefaultWidth { get; set; }

        private bool allowEmpty = true;
        /// <summary>
        /// 允许为空
        /// </summary>
        public bool AllowEmpty
        {
            get
            {
                return allowEmpty;
            }
            set
            {
                this.allowEmpty = value;
            }
        }
        /// <summary>
        /// 批注
        /// </summary>
        public string CellComment
        {
            get;
            set;
        }

        private bool isShowMinVal = true;
        /// <summary>
        /// 日期类型是否显示小于1900年的内容
        /// </summary>
        public bool ISShowMinVal
        {
            get
            {
                return isShowMinVal;
            }
            set
            {
                isShowMinVal = value;
            }
        }
        /// <summary>
        /// 数据格式
        /// </summary>
        public string Format
        {
            get;
            set;
        }

        public DataTypeEnum ToDataType(SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                case SqlDbType.NChar:
                case SqlDbType.Char:
                case SqlDbType.NText:
                case SqlDbType.Text:
                case SqlDbType.UniqueIdentifier:
                case SqlDbType.Xml:
                    return DataTypeEnum.String;
                case SqlDbType.Int:
                case SqlDbType.SmallInt:
                case SqlDbType.BigInt:
                case SqlDbType.TinyInt:
                    return DataTypeEnum.Int;
                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                case SqlDbType.Float:
                    return DataTypeEnum.Decimal;
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Time:
                    return DataTypeEnum.DateTime;
                default:
                    return DataTypeEnum.String;
            }
        }
    }

    /// <summary>
    /// DataColumn.DataType
    /// </summary>
    public enum DataTypeEnum
    {
        [Description("System.String")]
        String = 0,
        [Description("System.Int32")]
        Int = 1,
        [Description("System.Decimal")]
        Decimal = 2,
        [Description("System.DateTime")]
        DateTime = 3,
        [Description("System.Boolean")]
        Boolean = 4,
        [Description("System.TimeSpan")]
        TimeSpan = 5,
        [Description("")]
        Empty = 6
    }
}
