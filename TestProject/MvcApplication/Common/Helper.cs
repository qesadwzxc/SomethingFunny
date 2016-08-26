using Newtonsoft.Json;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcApplication.Common
{
    /// <summary>
    /// 通用帮助类
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            if (obj == null) return "{}";
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        /// <summary>
        /// 序列化对象，并排除Null值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeExceptNull(object obj)
        {
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
            //忽略值为null的
            jsonSetting.NullValueHandling = NullValueHandling.Ignore;
            //jsonSetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            if (obj == null) return "{}";
            return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSetting);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Deserialize(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeExceptNull<T>(string value)
        {
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
            //忽略值为null的
            jsonSetting.NullValueHandling = NullValueHandling.Ignore;

            return JsonConvert.DeserializeObject<T>(value, jsonSetting);
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <typeparam name="TInfo"></typeparam>
        /// <param name="info">info必须具有[Serializable]标签</param>
        /// <returns></returns>
        public static TInfo Clone<TInfo>(TInfo info)
        {
            using (MemoryStream ms = new MemoryStream(1000))
            {
                object CloneObject;
                BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, info);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化至另一个对象(即创建了一个原对象的深度副本)
                CloneObject = bf.Deserialize(ms);
                // 关闭流
                ms.Close();
                return (TInfo)CloneObject;
            }
        }
        /// <summary>
        /// 转换DataTable为实体类泛型
        /// </summary>
        /// <typeparam name="TInfo">实体类</typeparam>
        /// <param name="table">数据表</param>
        /// <returns>返回泛型</returns>
        public static List<TInfo> LoadData<TInfo>(DataTable table) where TInfo : new()
        {
            List<TInfo> dataList = new List<TInfo>();
            if (table != null && table.Rows.Count > 0)
            {
                Dictionary<string, PropertyInfo> propertyDic = new Dictionary<string, PropertyInfo>();
                Type type = typeof(TInfo);
                Array.ForEach(type.GetProperties(),
                    p =>
                    {
                        if (table.Columns.IndexOf(p.Name) != -1)
                        {
                            propertyDic.Add(p.Name, p);
                        }
                    });

                foreach (DataRow row in table.Rows)
                {
                    TInfo data = new TInfo();
                    foreach (DataColumn col in table.Columns)
                    {
                        if (propertyDic.ContainsKey(col.ColumnName))
                        {
                            PropertyInfo propertyInfo = propertyDic[col.ColumnName];
                            if (propertyInfo != null && propertyInfo.CanWrite && row[col.ColumnName] != DBNull.Value)
                            {
                                if (row[col.ColumnName] is Guid && !propertyInfo.PropertyType.FullName.Equals(typeof(Guid).FullName))
                                {
                                    propertyInfo.SetValue(data, row[col.ColumnName].ToString(), null);
                                }
                                else if (col.DataType == typeof(string) && propertyInfo.PropertyType == typeof(int))
                                {
                                    propertyInfo.SetValue(data, int.Parse(row[col.ColumnName].ToString()), null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(data, row[col.ColumnName], null);
                                }
                            }
                        }
                    }
                    dataList.Add(data);
                }
            }
            return dataList;
        }

        /// <summary>
        /// 转换DataRow为实体类
        /// </summary>
        /// <typeparam name="TInfo">实体类</typeparam>
        /// <param name="row">数据行</param>
        /// <param name="columnCollection">数据列集合</param>
        /// <returns>返回实体类</returns>
        public static TInfo LoadData<TInfo>(DataRow row, DataColumnCollection columnCollection, params string[] ignoreFields) where TInfo : new()
        {
            TInfo data = new TInfo();
            if (row != null)
            {
                foreach (PropertyInfo pro in data.GetType().GetProperties())
                {
                    if (pro.CanWrite && columnCollection.Contains(pro.Name) && !ignoreFields.Contains(pro.Name))
                    {
                        if (row[pro.Name] != DBNull.Value)
                        {
                            pro.SetValue(data, row[pro.Name], null);
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataTable FillDataTable<TInfo>(List<TInfo> modelList) where TInfo : new()
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable table = CreateTable(modelList[0]);
            foreach (TInfo model in modelList)
            {
                DataRow dataRow = table.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(TInfo).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private DataTable CreateTable<TInfo>(TInfo model) where TInfo : new()
        {
            DataTable dataTable = new DataTable(typeof(TInfo).Name);
            foreach (PropertyInfo propertyInfo in typeof(TInfo).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                DescriptionAttribute attr = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取枚举描述列表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<string> GetEnumDescriptionList(Type enumType)
        {
            List<string> returnList = new List<string>();
            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fi in fields)
            {
                DescriptionAttribute att = System.Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                returnList.Add(att.Description);
            }

            return returnList;
        }
        /// <summary>
        /// 从枚举中获得值+名称的表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static DataTable GetEnumManageTableByInt(Type enumType)
        {
            DataTable table = new DataTable();
            table.Columns.Add("value", typeof(int));
            table.Columns.Add("display", typeof(string));

            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fi in fields)
            {
                table.Rows.Add((int)(fi.GetValue(null)), fi.Name);
            }

            return table;
        }
        /// <summary>
        /// 从枚举中获得值+描述的表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static DataTable GetEnumManageTable(Type enumType)
        {
            DataTable table = new DataTable();
            table.Columns.Add("value", typeof(int));
            table.Columns.Add("display", typeof(string));

            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fi in fields)
            {
                DescriptionAttribute att = System.Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                table.Rows.Add((int)(fi.GetValue(null)), att.Description);
            }

            return table;
        }
        /// <summary>
        /// 从枚举中获得名称+描述的表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static DataTable GetEnumManageTableByDes(Type enumType)
        {
            DataTable table = new DataTable();
            table.Columns.Add("value", typeof(string));
            table.Columns.Add("display", typeof(string));

            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fi in fields)
            {
                DescriptionAttribute att = System.Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                table.Rows.Add(fi.Name, att.Description);
            }

            return table;
        }

        /// <summary>
        /// 根据枚举值描述和枚举，反串枚举值
        /// </summary>
        /// <returns></returns>
        public static int GetEnumValueByDescription(Type enumType, string descString)
        {
            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fi in fields)
            {
                DescriptionAttribute att = System.Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                if (att.Description == descString)
                {
                    return (int)fi.GetValue(null);
                }
            }
            return 0;
        }
        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool Equals(string value1, string value2)
        {
            if (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
            {
                return value1.Equals(value2, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        /// <summary>
        /// 判断是否为Guid
        /// </summary>
        /// <param name="guidString"></param>
        /// <returns></returns>
        public static bool IsGuid(string guidString)
        {
            Guid guid;
            return Guid.TryParse(guidString, out guid);
        }
        /// <summary>
        /// 判断类型是否为数字
        /// </summary>
        /// <param name="valueType">类型</param>
        /// <returns></returns>
        public static bool IsNumberType(Type valueType)
        {
            if (valueType == typeof(Int32) || valueType == typeof(Int64) || valueType == typeof(decimal) || valueType == typeof(double) || valueType == typeof(long) || valueType == typeof(float))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 从0/1转为bool型
        /// </summary>
        /// <param name="num">需转换的Int值</param>
        /// <returns>返回bool型</returns>
        public static bool ToBoolean(int num)
        {
            return num == 1;
        }
        /// <summary>
        /// 从0/1转为bool型
        /// </summary>
        /// <param name="num">需转换的Int值</param>
        /// <returns>返回bool型</returns>
        public static bool IsYes(int num)
        {
            return num == 1;
        }
        /// <summary>
        /// 从bool型转为0/1
        /// </summary>
        /// <param name="bol">需转换的boolean值</param>
        /// <returns>返回Int型</returns>
        public static int BooleanToNumber(Nullable<bool> bol)
        {
            return bol.GetValueOrDefault(false) ? 1 : 0;
        }
        /// <summary>
        /// 根据数据长度获取宽度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        public static int GetDataWidth(string value)
        {
            int width = ((Encoding.GetEncoding(936).GetBytes(value).Length + 3) * 300);
            if (width >= 65280)
            {
                width = 65279;
            }

            return width;
        }
        /// <summary>
        /// 根据数据长度获取宽度,如果大于传入宽度，则返回
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="width">比较宽度</param>
        /// <returns></returns>
        public static int GetDataWidth(string value, int width)
        {
            int calcWidth = (Encoding.GetEncoding(936).GetBytes(value).Length + 1) * 300;
            if (calcWidth > width)
            {
                return calcWidth;
            }
            return width;
        }
        /// <summary>
        /// SqlDbType转DataType
        /// </summary>
        /// <param name="type">SqlDbType</param>
        /// <returns>返回DataType</returns>
        public static DataTypeEnum ToDataType(SqlDbType type)
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
        /// <summary>
        /// SqlDbType转CellType
        /// </summary>
        /// <param name="type">SqlDbType</param>
        /// <returns>返回CellType</returns>
        public static CellType ToCellType(SqlDbType type)
        {
            DataTypeEnum dataType = ToDataType(type);

            return ToCellType(dataType);
        }
        /// <summary>
        /// DataTypeEnum转CellType
        /// </summary>
        /// <param name="type">DataTypeEnum</param>
        /// <returns>返回CellType</returns>
        public static CellType ToCellType(DataTypeEnum type)
        {
            switch (type)
            {
                case DataTypeEnum.String:
                    return CellType.String;
                case DataTypeEnum.Int:
                case DataTypeEnum.Decimal:
                    return CellType.Numeric;
                case DataTypeEnum.Boolean:
                    return CellType.Boolean;
                default:
                    return CellType.String;
            }
        }
        /// <summary>
        /// SqlDbType转CellType
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>返回CellType</returns>
        public static DataTypeEnum ToDataType(Type type)
        {
            var fields = typeof(DataTypeEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fi in fields)
            {
                DescriptionAttribute att = System.Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                if (type == Type.GetType(att.Description))
                {
                    return (DataTypeEnum)fi.GetValue(null);
                }
            }
            return DataTypeEnum.Empty;
        }
        /// <summary>
        /// 检查重复项，返回字符串
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="field">字段</param>
        /// <returns>返回提示信息</returns>
        public static string CheckRepeatData(DataTable table, string field)
        {
            StringBuilder builder = new StringBuilder();
            var dinstinctRowArray = table.AsEnumerable().Distinct(new PropertyEqualityComparer<DataRow>(field));
            int count = 0;
            foreach (DataRow row in dinstinctRowArray)
            {
                if (!string.IsNullOrEmpty(row[field].ToString()))
                {
                    count = table.AsEnumerable().Count(item => item[field].Equals(row[field]));
                    if (count > 1)
                    {
                        builder.AppendFormat("[{0}]存在{1}条重复项.", row[field], count - 1);
                    }
                }
            }
            return builder.ToString();
        }
        /// <summary>
        /// 根据身份证获取生日
        /// </summary>
        /// <param name="identityCard">身份证号</param>
        /// <returns>返回生日</returns>
        public static DateTime GetBirthday(string identityCard)
        {
            if (string.IsNullOrEmpty(identityCard))
            {
                return Globals.MinValue;
            }
            string birthString = string.Empty;
            if (identityCard.Length == 18)//处理18位的身份证号码从号码中得到生日和性别代码
            {
                birthString = identityCard.Substring(6, 8);
            }
            if (identityCard.Length == 15)
            {
                string shortYear = identityCard.Substring(6, 2);
                birthString = identityCard.Substring(6, 6);
                if (GetInt32(shortYear) < 16)
                {
                    //短年字段小于16，默认不会出现1916的身份号
                    birthString = "20" + birthString;
                }
                else
                {
                    birthString = "19" + birthString;
                }
            }
            if (string.IsNullOrEmpty(birthString))
            {
                return Globals.MinValue;
            }
            return GetDateTime(birthString, "yyyyMMdd");
        }
        /// <summary>
        /// 根据身份证获取性别
        /// </summary>
        /// <param name="identityCard">身份证号</param>
        /// <returns>返回性别0/1</returns>
        public static int GetSex(string identityCard)
        {
            if (string.IsNullOrEmpty(identityCard))
            {
                return -1;
            }
            string sexString = string.Empty;
            if (identityCard.Length == 18)//处理18位的身份证号码从号码中得到生日和性别代码
            {
                sexString = identityCard.Substring(14, 3);
            }
            if (identityCard.Length == 15)
            {
                sexString = identityCard.Substring(12, 3);
            }
            if (string.IsNullOrEmpty(sexString))
            {
                return -1;
            }

            return (int.Parse(sexString) % 2 == 0) ? 0 : 1;
        }
        /// <summary>
        /// 获取一段随机字符串
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetRandomString()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpperInvariant();
        }
        /// <summary>
        /// 获取星期字符串
        /// </summary>
        /// <param name="dayOfWeek">星期枚举</param>
        /// <param name="isFull">是否完整字符串</param>
        /// <returns></returns>
        public static string GetWeekString(DayOfWeek dayOfWeek, bool isFull = false)
        {
            string[] weekArray = new string[] { "日", "一", "二", "三", "四", "五", "六" };

            return (isFull ? "星期" : string.Empty) + weekArray[(int)dayOfWeek];
        }

        /// <summary>
        /// 验证特殊字符串,存在特殊字符返回false
        /// </summary>
        /// <param name="input">输入文本</param>
        /// <returns>存在特殊字符返回false</returns>
        public static bool CheckSpecialString(string input)
        {
            string[] patternArray ={
                        @"<script[^>]*?>.*?</script>",
                        @"<(///s*)?!?((/w+:)?/w+)(/w+(/s*=?/s*(([""'])(//[""'tbnr]|[^/7])*?/7|/w+)|.{0})|/s)*?(///s*)?>",
                        @"([/r/n])[/s]+",
                        @"&(quot|#34);",
                        @"&(amp|#38);",
                        @"&(lt|#60);",
                        @"&(gt|#62);",
                        @"&(nbsp|#160);",
                        @"&(iexcl|#161);",
                        @"&(cent|#162);",
                        @"&(pound|#163);",
                        @"&(copy|#169);",
                        @"&#(/d+);",
                        @"-->",
                        @"<!--.*/n",
                        "((?=[\x21-\x7e]+)[^A-Za-z0-9])"
                        };

            foreach (string pattern in patternArray)
            {
                if (Regex.IsMatch(input, pattern))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 根据路径获取文件类型
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetImageContentType(string filePath)
        {
            if (filePath.Contains('|'))
            {
                filePath = filePath.Split('|')[0];
            }
            if (filePath.EndsWith(".JPG", StringComparison.InvariantCultureIgnoreCase) || filePath.EndsWith(".JPE", StringComparison.InvariantCultureIgnoreCase) || filePath.EndsWith(".JPEG", StringComparison.InvariantCultureIgnoreCase))
            {
                return "image/jpeg";
            }
            else if (filePath.EndsWith(".bmp", StringComparison.InvariantCultureIgnoreCase))
            {
                return "image/bmp";
            }
            else if (filePath.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
            {
                return "image/gif";
            }
            else if (filePath.EndsWith(".ief", StringComparison.InvariantCultureIgnoreCase))
            {
                return "image/ief";
            }
            else if (filePath.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
            {
                return "image/png";
            }

            return string.Empty;
        }

        /// <summary>
        /// 过滤表格显示数据
        /// </summary>
        /// <param name="display"></param>
        /// <returns></returns>
        public static object FilterTableDisplay(object display)
        {
            if (IsNotNull(display) && display.Equals(Globals.MinValue.ToString("yyyy-MM-dd")))
            {
                return string.Empty;
            }
            return display;
        }

        public static object FilterTableDisplayForDate(object display)
        {

            if (IsNotNull(display))
            {
                DateTime time = Globals.MinValue;
                if (DateTime.TryParse(display.ToString(), out time))
                {
                    if (time <= Globals.MinValue)
                    {
                        return string.Empty;
                    }
                }
            }
            return display;
        }

        /// <summary>
        /// 获取指定日期的起止时间
        /// </summary>
        /// <param name="date">当前日期</param>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">截止时间</param>
        public static void GetThisWeekStartAndEnd(DateTime date, out DateTime startDate, out DateTime endDate)
        {
            int diff = (int)date.DayOfWeek == 0 ? (int)date.DayOfWeek : 7;
            startDate = DateTime.Parse(date.AddDays((-1) * (diff - 1)).ToString("yyyy-MM-dd 00:00:00"));
            endDate = DateTime.Parse(date.AddDays(7 - diff).ToString("yyyy-MM-dd 23:59:59"));
        }

        /// <summary>
        /// 获取第几周方法
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static int GetWeekOrderOfDate(DateTime Date)
        {
            //当天所在的年份  
            int year = Date.Year;
            //当年的第一天  
            DateTime firstDay = new DateTime(year, 1, 1);
            //当年的第一天是星期几  
            int firstOfWeek = Convert.ToInt32(firstDay.DayOfWeek);
            if (firstDay.DayOfWeek == DayOfWeek.Sunday) firstOfWeek = 7;
            //当年第一周的天数  
            int firstWeekDayNum = 8 - firstOfWeek;

            //传入日期在当年的天数与第一周天数的差  
            int otherDays = Date.DayOfYear - firstWeekDayNum;
            //传入日期不在第一周内  
            if (otherDays > 0)
            {
                int weekNumOfOtherDays;
                if (otherDays % 7 == 0)
                {
                    weekNumOfOtherDays = otherDays / 7;
                }
                else
                {
                    weekNumOfOtherDays = otherDays / 7 + 1;
                }

                return weekNumOfOtherDays + 1;
            }
            //传入日期在第一周内  
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 检查图片是否满足要求
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="size">大小（M）</param>
        public static void CheckUploadImage(System.Web.HttpPostedFileBase file, int size)
        {
            if (file == null || file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                throw new WarningException("上传失败，未取得文件大小");
            }
            if (file.ContentLength > size * 1024 * 1024)
            {
                throw new WarningException("上传失败，图片尺寸请限制在2M以内");
            }

            string exts = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);
            string[] extLimited = { "jpg", "jpeg", "JPEG", "JPG", "GIF", "gif", "PNG", "png", "BMP", "bmp" };
            if (!extLimited.Contains(exts))
            {
                throw new WarningException("上传失败，请选择图片进行上传");
            }
        }

        /// <summary>
        /// 流
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            //byte[] bytes = new byte[stream.Length];
            //stream.Read(bytes, 0, bytes.Length);
            //// 设置当前流的位置为流的开始
            //stream.Seek(0, SeekOrigin.Begin);
            //return bytes;

            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }

            return bytes.ToArray();
        }
        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 根据换行，空格，逗号，分号进行字符串分隔
        /// </summary>
        /// <param name="input">输入文本</param>
        /// <returns></returns>
        public static List<string> SplitString(string input)
        {
            return input.Split(new string[] { "\r\n", " ", ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static bool IsNotNull(object data)
        {
            return (data != null && data != DBNull.Value);
        }

        #region 类型转换
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <returns>返回整型</returns>
        public static int GetInt32(object value)
        {
            if (!IsNotNull(value))
            {
                return 0;
            }
            int returnValue;
            if (int.TryParse(value.ToString(), out returnValue))
            {
                return returnValue;
            }
            return 0;
        }
        public static int GetInt32(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            int returnValue;
            if (int.TryParse(value.ToString(), out returnValue))
            {
                return returnValue;
            }
            return 0;
        }
        /// <summary>
        /// 获取BigInt整数
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <returns>返回整型</returns>
        public static long GetInt64(object value)
        {
            if (!IsNotNull(value))
            {
                return 0;
            }
            long returnValue;
            if (long.TryParse(value.ToString(), out returnValue))
            {
                return returnValue;
            }
            return 0;
        }
        /// <summary>
        /// 获取byte整数
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <returns>返回整型</returns>
        public static byte GetByte(object value)
        {
            if (!IsNotNull(value))
            {
                return 0;
            }
            byte returnValue;
            if (byte.TryParse(value.ToString(), out returnValue))
            {
                return returnValue;
            }
            return 0;
        }

        /// <summary>
        /// 获取小数
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <returns>返回小数</returns>
        public static double GetDouble(object value)
        {
            if (!IsNotNull(value))
            {
                return 0;
            }
            double returnValue;
            if (double.TryParse(value.ToString(), out returnValue))
            {
                return returnValue;
            }
            return 0;
        }
        /// <summary>
        /// 获取小数
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <returns>返回小数</returns>
        public static decimal GetDecimal(object value)
        {
            if (!IsNotNull(value))
            {
                return decimal.Zero;
            }
            decimal returnValue;
            if (decimal.TryParse(value.ToString(), out returnValue))
            {
                return returnValue;
            }
            return decimal.Zero;
        }
        /// <summary>
        /// 获取小数(保留小数位，但无小数位不会添加)
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <param name="decimals">小数位</param>
        /// <returns>返回小数</returns>
        public static decimal GetDecimal(object value, int decimals)
        {
            if (!IsNotNull(value))
            {
                return Math.Round(decimal.Zero, decimals);
            }
            decimal returnValue;
            if (decimal.TryParse(value.ToString(), out returnValue))
            {
                return Math.Round(returnValue, decimals);
            }
            return Math.Round(decimal.Zero, decimals);
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <returns>返回小数</returns>
        public static DateTime GetDateTime(object value)
        {
            return GetDateTime(value, "yyyyMMdd");
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <param name="format">格式化</param>
        /// <returns>返回小数</returns>
        public static DateTime GetDateTime(object value, string format)
        {
            if (!IsNotNull(value))
            {
                return DateTime.Now;
            }
            DateTime returnValue;
            if (DateTime.TryParseExact(value.ToString(), format, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out returnValue))
            {
                return returnValue;
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 获取TimeSpan
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <returns>返回时间</returns>
        public static TimeSpan GetTimeSpan(object value)
        {
            if (!IsNotNull(value))
            {
                return TimeSpan.Zero;
            }
            if (value is TimeSpan)
            {
                return (TimeSpan)value;
            }
            return TimeSpan.Zero;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <returns>返回字符串</returns>
        public static string GetString(object value)
        {
            if (!IsNotNull(value))
            {
                return string.Empty;
            }
            return string.Format("{0}", value.ToString());
        }
        #endregion
    }

    public enum CustomContractResolver
    {
        /// <summary>
        /// 属性名首字母小写（长度大于1）
        /// </summary>
        LowercaseFirstContractResolver,
        /// <summary>
        /// 属性名首字母小写并且枚举取字符串
        /// </summary>
        LowercaseFirstAndGetEnumString
    }
}
