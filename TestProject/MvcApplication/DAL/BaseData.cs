//using iThinking.Helper;
using VinCode.DateBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using MvcApplication.Common;
using MvcApplication.Models;
using MvcApplication.Models.Base;
using System.ComponentModel;
using System.Data.SqlClient;

namespace MvcApplication.DAL
{
    /// <summary>
    /// 服务端基类
    /// </summary>
    /// <typeparam name="TInfo"></typeparam>
    public class BaseData<TInfo> where TInfo : ActiveRecord, new()
    {
        public SqlHelper SqlFactory = new SqlHelper();

        #region 属性
        private List<string> columnList = new List<string>();
        /// <summary>
        /// 当前表的字段集合
        /// </summary>
        protected List<string> ColumnList
        {
            get
            {
                return this.columnList;
            }
        }
        private string tableName = string.Empty;
        /// <summary>
        /// 当前表的名称
        /// </summary>
        protected virtual string TableName
        {
            get
            {
                return this.tableName;
            }
        }
        private string validField = string.Empty;
        /// <summary>
        /// 当前表的有效字段
        /// </summary>
        protected virtual string ValidField
        {
            get
            {
                return this.validField;
            }
        }
        private string keyField = string.Empty;
        /// <summary>
        /// 当前表的主键字段
        /// </summary>
        protected virtual string KeyField
        {
            get
            {
                return this.keyField;
            }
        }
        /// <summary>
        /// 带前缀的表名
        /// </summary>
        protected virtual string FullTableName
        {
            get
            {
                return string.Format("[{0}].[dbo].[{1}]", this.DataBaseName, this.TableName);
            }
        }
        /// <summary>
        /// 表名前缀
        /// </summary>
        protected virtual string PrefixDB
        {
            get
            {
                return string.Format("[{0}].[dbo]", this.DataBaseName);
            }
        }

        /// <summary>
        /// 开始页码参数
        /// </summary>
        protected virtual string NumberStartParam
        {
            get
            {
                return "@numStart";
            }
        }
        /// <summary>
        /// 结束页码参数
        /// </summary>
        protected virtual string NumberEndParam
        {
            get
            {
                return "@numEnd";
            }
        }

        //protected virtual Database DataBase_Read
        //{
        //    get
        //    {
        //        return DatabaseFactory.GetReadDatabase(Globals.G_DatabaseName);
        //    }
        //}
        //protected virtual Database DataBase_Write
        //{
        //    get
        //    {
        //        return DatabaseFactory.GetWriteDatabase(Globals.G_DatabaseName);
        //    }
        //}
        #endregion

        public BaseData()
        {
            TInfo info = new TInfo();
            //emp = this.CurrEmployee;
            columnList = info.GetAllColumns();
            tableName = info.GetTableName();
            validField = info.GetValidField();
            keyField = info.GetKeyField();
        }
        /// <summary>
        /// 数据库名称
        /// </summary>
        protected virtual string DataBaseName
        {
            get
            {
                return Globals.G_DatabaseName;
            }
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        protected virtual string Order
        {
            get
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取全部有效数据
        /// </summary>
        /// <returns>获取全部有效数据</returns>
        public List<TInfo> FindAll(params string[] fields)
        {
            return this.FindAll(this.Order, fields);
        }
        /// <summary>
        /// 获取全部有效数据
        /// </summary>
        /// <param name="orderString">order字符串</param>
        /// <returns>获取全部有效数据</returns>
        public List<TInfo> FindAll(string orderString, params string[] fields)
        {
            return this.FindAll(orderString, string.Empty, new List<SqlParameter>(), fields);
        }

        public List<TInfo> SelectAll(string whereString)
        {
            return this.FindAll(string.Empty, whereString, new List<SqlParameter>());
        }
        /// <summary>
        /// 获取全部有效数据
        /// </summary>
        /// <param name="orderString">order字符串</param>
        /// <param name="whereString">where字符串</param>
        /// <param name="paramList">参数集合</param>
        /// <returns>获取全部有效数据</returns>
        public List<TInfo> FindAll(string orderString, string whereString, List<SqlParameter> paramList, params string[] fields)
        {
            TInfo info = new TInfo();
            StringBuilder builder = new StringBuilder("Select ");

            string tableName = info.GetTableName();
            if (fields.Length > 0)
            {
                builder.AppendFormat("TB.{0} ", string.Join(",TB.", fields.Intersect(columnList).ToArray()));
            }
            else
            {
                builder.AppendFormat("TB.{0} ", string.Join(",TB.", columnList.ToArray()));
            }
            builder.AppendFormat("From [{0}].[dbo].[{1}] TB With(nolock) ", this.DataBaseName, tableName);
            if (!string.IsNullOrEmpty(validField))
            {
                builder.AppendFormat("Where TB.{0} = 1 ", validField);
            }
            else
            {
                builder.Append("Where 1 = 1 ");
            }
            if (!string.IsNullOrEmpty(whereString))
            {
                if (!whereString.TrimStart().StartsWith("And"))
                {
                    builder.AppendFormat("And ", whereString);
                }
                builder.AppendFormat("{0} ", whereString);
            }
            if (NeedPaging(paramList))
            {
                if (string.IsNullOrEmpty(orderString))
                {
                    orderString = "Order By " + this.keyField + " Asc ";
                }
                builder = new StringBuilder(string.Format(@"
                            Select TB.* From (
                                Select ROW_NUMBER() OVER ({0}) As RowNumber,T.* From (
                                    {1}
                                ) T 
                            ) TB Where TB.RowNumber Between {2} And {3} 
                        ", orderString, builder.ToString(), this.NumberStartParam, this.NumberEndParam));
            }
            else
            {
                builder.Append(orderString);
            }

            DataTable table = SqlFactory.ExecuteReader(builder.ToString(), paramList);
            return info.LoadData<TInfo>(table);
        }

        /// <summary>
        /// 获取全部有效数据(表格)
        /// </summary>
        /// <returns>获取全部有效数据</returns>
        public DataTable FindAll_Table()
        {
            return FindAll_Table(this.Order, string.Empty, new List<SqlParameter>());
        }

        /// <summary>
        /// 获取全部有效数据(表格)
        /// </summary>
        /// <param name="orderString">order字符串</param>
        /// <param name="whereString">where字符串</param>
        /// <param name="paramList">参数集合</param>
        /// <returns>获取全部有效数据</returns>
        public DataTable FindAll_Table(string orderString, string whereString, List<SqlParameter> paramList)
        {
            TInfo info = new TInfo();
            StringBuilder builder = new StringBuilder("Select ");

            string tableName = info.GetTableName();
            builder.AppendFormat("TB.{0} ", string.Join(",TB.", columnList.ToArray()));
            builder.AppendFormat("From {0}.[dbo].[{1}] TB With(nolock) ", this.DataBaseName, tableName);
            if (!string.IsNullOrEmpty(validField))
            {
                builder.AppendFormat("Where TB.{0} = 1 ", validField);
            }
            else
            {
                builder.Append("Where 1 = 1 ");
            }
            if (!string.IsNullOrEmpty(whereString))
            {
                if (!whereString.TrimStart().StartsWith("And"))
                {
                    builder.AppendFormat("And ", whereString);
                }
                builder.AppendFormat("{0} ", whereString);
            }
            if (NeedPaging(paramList))
            {
                if (string.IsNullOrEmpty(orderString))
                {
                    orderString = "Order By " + this.keyField + " Asc ";
                }
                builder = new StringBuilder(string.Format(@"
                            Select TB.* From (
                                Select ROW_NUMBER() OVER ({0}) As RowNumber,T.* From (
                                    {1}
                                ) T 
                            ) TB Where TB.RowNumber Between {2} And {3} 
                        ", orderString, builder.ToString(), this.NumberStartParam, this.NumberEndParam));
            }
            else
            {
                builder.Append(orderString);
            }

            return SqlFactory.ExecuteReader(builder.ToString(), paramList);
        }

        private bool NeedPaging(List<SqlParameter> collection)
        {
            if (collection != null)
            {
                foreach (SqlParameter param in collection)
                {
                    if (param.ParameterName.Equals(this.NumberStartParam, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取符合条件的全部数据
        /// </summary>
        /// <param name="key">键值对</param>
        /// <param name="orderString">排序字符串</param>
        /// <returns>获取符合条件的全部数据</returns>
        public List<TInfo> FindAll(CompositeKey key, string orderString = "", params string[] fields)
        {
            TInfo info = new TInfo();
            StringBuilder builder = new StringBuilder("Select ");
            List<SqlParameter> paramList = new List<SqlParameter>();

            if (fields.Length > 0)
            {
                builder.AppendFormat("TB.{0} ", string.Join(",TB.", fields.Intersect(columnList).ToArray()));
            }
            else
            {
                builder.AppendFormat("TB.{0} ", string.Join(",TB.", columnList.ToArray()));
            }
            builder.AppendFormat("From {0} TB With(nolock) ", this.FullTableName);
            if (!string.IsNullOrEmpty(validField))
            {
                builder.AppendFormat("Where TB.{0} = 1 ", validField);
            }
            else
            {
                builder.Append("Where 1 = 1 ");
            }
            foreach (KeyValuePair<string, object> item in key.KeyDic)
            {
                builder.AppendFormat("And TB.{0} = @{0} ", item.Key, item.Value);
                paramList.Add(new SqlParameter("@" + item.Key, item.Value));
            }
            if (NeedPaging(paramList))
            {
                if (string.IsNullOrEmpty(orderString))
                {
                    orderString = "Order By " + this.keyField + " Asc ";
                }
                builder = new StringBuilder(string.Format(@"
                            Select TB.* From (
                                Select ROW_NUMBER() OVER ({0}) As RowNumber,T.* From (
                                    {1}
                                ) T 
                            ) TB Where TB.RowNumber Between {2} And {3} 
                        ", orderString, builder.ToString(), this.NumberStartParam, this.NumberEndParam));
            }
            else
            {
                builder.Append(orderString);
            }

            DataTable table = SqlFactory.ExecuteReader(builder.ToString(), paramList);

            return info.LoadData<TInfo>(table);
        }

        public List<TInfo> FindAllWithJoin(CompositeKey key, string orderString = "", params string[] fields)
        {
            #region join
            List<string> onconditions = new List<string>();//left join xxx on xxx=xxx
            List<string> selects = new List<string>();//select xx.xxx

            Dictionary<string, JoinAttribute> dicJoins = new Dictionary<string, JoinAttribute>();//含有外连接的字段名
            foreach (PropertyInfo propInfo in typeof(TInfo).GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(JoinAttribute), true);
                if (objAttrs.Length > 0)
                {
                    JoinAttribute attr = objAttrs[0] as JoinAttribute;
                    Type tTable = propInfo.PropertyType;//表的类
                    // foreach (var Tableprop in tTable.getp)
                    var tableattributes = tTable.GetCustomAttributes(typeof(TableAttribute), false);
                    if (tableattributes.Length > 0)
                    {
                        TableAttribute tb = tableattributes[0] as TableAttribute;
                        onconditions.Add(System.Enum.GetName(typeof(JoinOperator), attr.JoinType) + " " + string.Format("[{0}].[dbo].[{1}]", this.DataBaseName, tb.TableName) + " as  " + propInfo.Name + " With(nolock) on " + attr.OnString);

                        if (attr.SelectString != null)
                        {
                            attr.SelectString.ToList().ForEach(s =>
                            {
                                selects.Add(propInfo.Name + "." + s + " as " + (propInfo.Name + s).ToLower());
                            });
                        }
                        dicJoins.Add(propInfo.Name, attr);
                    }
                }
            }
            #endregion

            TInfo info = new TInfo();
            StringBuilder builder = new StringBuilder("Select ");
            List<SqlParameter> paramList = new List<SqlParameter>();

            if (fields.Length > 0)
            {
                builder.AppendFormat("TB.{0} ", string.Join(",TB.", fields.Intersect(columnList).ToArray()));
            }
            else
            {
                builder.AppendFormat("TB.{0} ", string.Join(",TB.", columnList.ToArray()));
            }
            builder.Append("," + string.Join(",", selects.ToArray()));
            builder.AppendFormat(" From {0} TB With(nolock) ", this.FullTableName);
            onconditions.ForEach(cond =>
            {
                builder.Append(" " + cond + " ");

            });
            if (!string.IsNullOrEmpty(validField))
            {
                builder.AppendFormat("Where TB.{0} = 1 ", validField);
            }
            else
            {
                builder.Append("Where 1 = 1 ");
            }
            foreach (KeyValuePair<string, object> item in key.KeyDic)
            {
                builder.AppendFormat("And TB.{0} = @{0} ", item.Key, item.Value);
                paramList.Add(new SqlParameter("@" + item.Key, item.Value));
            }
            if (NeedPaging(paramList))
            {
                if (string.IsNullOrEmpty(orderString))
                {
                    orderString = "Order By " + this.keyField + " Asc ";
                }
                builder = new StringBuilder(string.Format(@"
                            Select TB.* From (
                                Select ROW_NUMBER() OVER ({0}) As RowNumber,T.* From (
                                    {1}
                                ) T 
                            ) TB Where TB.RowNumber Between {2} And {3} 
                        ", orderString, builder.ToString(), this.NumberStartParam, this.NumberEndParam));
            }
            else
            {
                builder.Append(orderString);
            }

            DataTable table = SqlFactory.ExecuteReader(builder.ToString(), paramList);

            List<TInfo> dataList = new List<TInfo>();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    TInfo data = new TInfo();

                    dicJoins.ToList().ForEach(pair =>
                    {
                        PropertyInfo p = data.GetType().GetProperty(pair.Key);
                        Type t = p.PropertyType;//含有join标记的属性
                        ConstructorInfo ct = t.GetConstructor(System.Type.EmptyTypes);
                        var ins = ct.Invoke(null);//实例化对象
                        var childprops = ins.GetType().GetProperties();
                        foreach (var prop in childprops)
                        {
                            var a = (from s in pair.Value.SelectString.ToList()
                                     where s.ToLower() == prop.Name.ToLower()
                                     select s);//查出是否是需要记录的属性
                            if (a.Count() > 0)
                            {
                                string childcol = (pair.Key + a.First()).ToLower();
                                if (prop != null && row[childcol] != DBNull.Value)
                                {
                                    if (row[childcol] is Guid && !prop.PropertyType.FullName.Equals(typeof(Guid).FullName))
                                    {
                                        prop.SetValue(ins, row[childcol].ToString(), null);
                                    }
                                    else if (table.Columns[childcol].DataType.FullName == "System.String" && prop.PropertyType.FullName == "System.Int32")
                                    {
                                        prop.SetValue(ins, int.Parse(row[childcol].ToString()), null);
                                    }
                                    else
                                    {
                                        prop.SetValue(ins, row[childcol], null);
                                    }
                                }
                            }
                        }
                        p.SetValue(data, ins, null);
                    });


                    foreach (DataColumn col in table.Columns)
                    {
                        PropertyInfo propertyInfo = data.GetType().GetProperty(col.ColumnName);
                        if (propertyInfo != null && propertyInfo.CanWrite && row[col.ColumnName] != DBNull.Value)
                        {
                            if (row[col.ColumnName] is Guid && !propertyInfo.PropertyType.FullName.Equals(typeof(Guid).FullName))
                            {
                                propertyInfo.SetValue(data, row[col.ColumnName].ToString(), null);
                            }
                            else if (col.DataType.FullName == "System.String" && propertyInfo.PropertyType.FullName == "System.Int32")
                            {
                                propertyInfo.SetValue(data, int.Parse(row[col.ColumnName].ToString()), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(data, row[col.ColumnName], null);
                            }
                        }
                    }
                    dataList.Add(data);
                }
            }
            return dataList;




            //return info.LoadData<TInfo>(table);
        }

        /// <summary>
        /// 获取数据行数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Count(string.Empty, new List<SqlParameter>());
        }

        /// <summary>
        /// 获取数据行数
        /// </summary>
        /// <param name="whereString">where条件字符串</param>
        /// <param name="paramList">参数集合</param>
        /// <returns>返回数据行数</returns>
        public int Count(string whereString, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder("Select ");

            builder.Append("Count(1) ");
            builder.AppendFormat("From {0}.[dbo].[{1}] TB With(nolock) ", this.DataBaseName, tableName);
            if (!string.IsNullOrEmpty(validField))
            {
                builder.AppendFormat("Where TB.{0} = 1 ", validField);
            }
            else
            {
                builder.Append("Where 1 = 1 ");
            }
            if (!string.IsNullOrEmpty(whereString))
            {
                if (!whereString.TrimStart().StartsWith("And"))
                {
                    builder.AppendFormat("And ", whereString);
                }
                builder.AppendFormat("{0} ", whereString);
            }

            return SqlFactory.ExecuteScalarByInt(builder.ToString(), paramList);
        }

        /// <summary>
        /// 通过key值获取单个数据
        /// </summary>
        /// <param name="value">单个主键的值，或键值对CompositeKey</param>
        /// <param name="fields">查询字段</param>
        /// <returns>获取数据实体类</returns>
        public TInfo FindData(object value, params string[] fields)
        {
            TInfo info = new TInfo();
            List<SqlParameter> paramList = new List<SqlParameter>();
            StringBuilder builder = new StringBuilder("Select Top 1 ");

            if (fields.Length > 0)
            {
                builder.AppendFormat("TB.{0} ", string.Join(",TB.", fields));
            }
            else
            {
                builder.AppendFormat("TB.{0} ", string.Join(",TB.", columnList.ToArray()));
            }
            builder.AppendFormat("From {0}.[dbo].[{1}] TB With(nolock) ", this.DataBaseName, tableName);
            if (!string.IsNullOrEmpty(validField))
            {
                builder.AppendFormat("Where TB.{0} = 1 ", validField);
            }
            else
            {
                builder.Append("Where 1 = 1 ");
            }
            if (value is CompositeKey)
            {
                Dictionary<string, object> keyDic = (value as CompositeKey).KeyDic;
                foreach (KeyValuePair<string, object> item in keyDic)
                {
                    builder.AppendFormat("And TB.{0} = @{0} ", item.Key, item.Value);
                    paramList.Add(new SqlParameter("@" + item.Key, item.Value));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyField))
                {
                    builder.AppendFormat("And TB.{0} = @{0} ", keyField, value);
                    paramList.Add(new SqlParameter("@" + keyField, value));
                }
            }

            DataTable table = SqlFactory.ExecuteReader(builder.ToString(), paramList);
            if (table.Rows.Count == 1)
            {
                TInfo returnInfo = info.LoadData<TInfo>(table)[0];
                returnInfo.RecordStatus = RecordStatus.Modify;

                return returnInfo;
            }
            return null;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="info">需添加的数据类</param>
        /// <param name="fields">添加的字段数组</param>
        /// <returns>返回执行状态</returns>
        public bool InsertData(TInfo info, params string[] fields)
        {
            return InsertData(info, null, fields);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="info">需添加的数据类</param>
        /// <param name="fields">添加的字段数组</param>
        /// <returns>返回执行状态</returns>
        public bool InsertDataWithOutLog(TInfo info, params string[] fields)
        {
            return InsertData(info, null, false, fields);
        }
        /// <summary>
        /// 添加数据（返回主键）
        /// </summary>
        /// <param name="info">需添加的数据类</param>
        /// <param name="fields">添加的字段数组</param>
        /// <returns>返回主键</returns>
        public int InsertIdentity(TInfo info, params string[] fields)
        {
            return InsertIdentity(info, null, fields);
        }
        /// <summary>
        /// 添加数据（返回主键）
        /// </summary>
        /// <param name="info">需添加的数据类</param>
        /// <param name="tran">事务基类</param>
        /// <param name="fields">添加的字段数组</param>
        /// <returns>返回主键</returns>
        public int InsertIdentity(TInfo info, SqlTransaction tran, params string[] fields)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            string sqlString = DoInsert(info, fields, paramList);

            int returnValue = 0;
            if (tran != null)
            {
                returnValue = SqlFactory.ExecuteIdentity(sqlString, paramList, tran);
            }
            else
            {
                returnValue = SqlFactory.ExecuteIdentity(sqlString, paramList);
            }
            //this.Log(OperateDataType.Delete, string.Empty, null, null, string.Format("{0}[{1}]对表{2}执行了添加操作，操作方法：{3}.", CurrEmployee.EmpName, CurrEmployee.EmpId, this.FullTableName, "BaseData.InsertData"));

            return returnValue;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="info">需添加的数据类</param>
        /// <param name="tran">事务基类</param>
        /// <param name="fields">添加的字段数组</param>
        /// <returns>返回执行状态</returns>
        public bool InsertData(TInfo info, SqlTransaction tran, params string[] fields)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            string sqlString = DoInsert(info, fields, paramList);

            bool returnValue = false;
            if (tran != null)
            {
                returnValue = SqlFactory.ExecuteNonQuery(sqlString, paramList, tran) > 0;
            }
            else
            {
                returnValue = SqlFactory.ExecuteNonQuery(sqlString, paramList) > 0;
            }
            //this.Log(OperateDataType.Create, string.Empty, "无", "无", string.Format("{0}[{1}]对表{2}执行了添加操作，操作方法：{3}.", CurrEmployee.EmpName, CurrEmployee.EmpId, this.FullTableName, "BaseData.InsertData"));

            return returnValue;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="info">需添加的数据类</param>
        /// <param name="tran">事务基类</param>
        /// <param name="fields">添加的字段数组</param>
        /// <returns>返回执行状态</returns>
        public bool InsertData(TInfo info, SqlTransaction tran, bool isRecLog, params string[] fields)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            string sqlString = DoInsert(info, fields, paramList);

            bool returnValue = false;
            if (tran != null)
            {
                returnValue = SqlFactory.ExecuteNonQuery(sqlString, paramList, tran) > 0;
            }
            else
            {
                returnValue = SqlFactory.ExecuteNonQuery(sqlString, paramList) > 0;
            }
            //this.Log(OperateDataType.Create, string.Empty, "无", "无", string.Format("{0}[{1}]对表{2}执行了添加操作，操作方法：{3}.", CurrEmployee.EmpName, CurrEmployee.EmpId, this.FullTableName, "BaseData.InsertData"));

            return returnValue;
        }

        private string DoInsert(TInfo info, string[] fields, List<SqlParameter> paramList)
        {
            string[] fieldArray = this.ColumnList.ToArray();
            List<string> fieldList = fieldArray.ToList();
            if (fields.Length > 0)
            {
                List<string> fList = fieldList.Intersect(fields.ToList()).ToList();
                fieldList.Clear();
                fieldList.AddRange(fList);
            }
            string increaseField = info.GetIncreaseField();
            CreateModel createModel = info.GetCreateField();
            ModifyModel modifyModel = info.GetModifyField();
            if (createModel != null && modifyModel != null)
            {
                info[createModel.CreateIdField] = 18018;//UNDONE:根据底层获取修改人ID
                info[createModel.CreateTimeField] = DateTime.Now;
                info[modifyModel.ModifyIdField] = 18018;//UNDONE:根据底层获取修改人ID
                info[modifyModel.ModifyTimeField] = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(this.ValidField))
            {
                if (!fieldList.Contains(this.ValidField))
                {
                    fieldList.Add(this.ValidField);
                }
                info[this.ValidField] = 1;
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Insert into {0} ", this.FullTableName);
            //排除自增长键的影响
            fieldList.Remove(increaseField);
            builder.AppendFormat("({0}) ", string.Join(",", fieldList.ToArray()));
            builder.Append("Values (");
            foreach (string name in fieldList)
            {
                if (!Helper.IsNotNull(info[name]))
                {
                    builder.AppendFormat("@{0},", name);
                    paramList.Add(new SqlParameter("@" + name, info[name]));
                }
                else
                {
                    builder.AppendFormat("'',");
                }
            }
            builder = builder.Remove(builder.Length - 1, 1);
            builder.Append(") ");

            return builder.ToString();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="info">需更新的数据类</param>
        /// <param name="fields">需更新的字段，默认全部字段</param>
        /// <returns>返回执行状态</returns>
        public bool UpdateData(TInfo info, params string[] fields)
        {
            return UpdateData(info, null, null, fields);
        }

        public bool UpdateData(TInfo info, CompositeKey wherePair, params string[] fields)
        {
            return UpdateData(info, wherePair, null, fields);
        }

        public bool UpdateData(TInfo info, SqlTransaction tran, params string[] fields)
        {
            return this.UpdateData(info, null, tran, fields);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="info">需更新的数据类</param>
        /// <param name="wherePair">条件键值对</param>
        /// <param name="tran">事务基类</param>
        /// <param name="fields">需更新的字段，默认全部字段</param>
        /// <returns>返回执行状态</returns>
        public bool UpdateData(TInfo info, CompositeKey wherePair, SqlTransaction tran, params string[] fields)
        {
            string increaseField = info.GetIncreaseField();
            List<string> keyArray = info.GetKeyArray();
            string[] fieldArray = this.ColumnList.ToArray();
            List<string> fieldList = fieldArray.ToList();
            if (fields.Length > 0)
            {
                List<string> fList = fieldList.Intersect(fields.ToList()).ToList();

                fieldList.Clear();
                fieldList.AddRange(fList);
            }
            ModifyModel modifyModel = info.GetModifyField();
            if (modifyModel != null)
            {
                info[modifyModel.ModifyIdField] = 18018;//UNDONE:根据底层获取修改人ID
                info[modifyModel.ModifyTimeField] = DateTime.Now;
            }
            List<SqlParameter> paramList = new List<SqlParameter>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Update {0} Set ", this.FullTableName);
            //排除自增长键的影响
            fieldList.Remove(increaseField);
            //如果没有指定需要修改是否有效的字段，则去除
            if (!fields.Contains(this.ValidField))
            {
                fieldList.Remove(this.ValidField);
            }
            CreateModel createModel = info.GetCreateField();
            if (createModel != null && !createModel.CreateIdField.Equals(modifyModel.ModifyIdField, StringComparison.InvariantCultureIgnoreCase))
            {
                fieldList.Remove(createModel.CreateIdField);
                fieldList.Remove(createModel.CreateTimeField);
            }
            if (modifyModel != null && !fieldList.Contains(modifyModel.ModifyIdField))
            {
                fieldList.Add(modifyModel.ModifyIdField);
                fieldList.Add(modifyModel.ModifyTimeField);
            }
            foreach (string keyField in keyArray)
            {
                fieldList.Remove(keyField);
            }
            foreach (string name in fieldList)
            {
                if (Helper.IsNotNull(info[name]))
                {
                    builder.AppendFormat("{0} = @{0},", name);
                    paramList.Add(new SqlParameter("@" + name, info[name]));
                }
                else
                {
                    //builder.AppendFormat("{0} = null,");
                }
            }
            builder = builder.Remove(builder.Length - 1, 1);
            builder.Append(" Where 1=1 ");

            if (wherePair == null)
            {
                foreach (string keyField in keyArray)
                {
                    builder.AppendFormat("And {0} = @{0} ", keyField);
                    paramList.Add(new SqlParameter("@" + keyField, info[keyField]));
                }
            }
            else
            {
                foreach (KeyValuePair<string, object> item in wherePair.KeyDic)
                {
                    builder.AppendFormat("And {0} = @{0} ", item.Key);
                    object value = item.Value;
                    if (value == null)
                    {
                        paramList.Add(new SqlParameter("@" + item.Key, info[item.Key]));
                        value = info[item.Key];
                    }
                    else
                    {
                        paramList.Add(new SqlParameter("@" + item.Key, item.Value));
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.ValidField))
            {
                builder.AppendFormat("And {0} = 1 ", this.ValidField);
            }

            bool returnValue = false;
            if (tran != null)
            {
                var k = paramList.Select(x => x.Value);
                string pp = string.Join(";", k);

                returnValue = SqlFactory.ExecuteNonQuery(builder.ToString(), paramList, tran) > 0;
            }
            else
            {
                returnValue = SqlFactory.ExecuteNonQuery(builder.ToString(), paramList) > 0;
            }
            //this.Log(OperateDataType.Edit, string.Empty, null, null, string.Format("{0}[{1}]对表{2}执行了更新操作，操作方法：{3}.", CurrEmployee.EmpName, CurrEmployee.EmpId, this.FullTableName, "BaseData.UpdateData"));

            return returnValue;
        }
        /// <summary>
        /// 删除数据(更新数据状态)
        /// </summary>
        /// <param name="info">需删除的数据类</param>
        /// <returns>返回执行状态</returns>
        public bool DeleteData(TInfo info)
        {
            string primaryKey = string.Empty;
            if (string.IsNullOrEmpty(this.ValidField))
            {
                return false;
            }
            List<string> keyArray = info.GetKeyArray();
            List<SqlParameter> paramList = new List<SqlParameter>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Update {0} Set {1} = 0 ", this.FullTableName, this.ValidField);
            builder.AppendFormat("Where 1=1 ");
            foreach (string keyField in keyArray)
            {
                builder.AppendFormat("And {0} = @{0} ", keyField);
                paramList.Add(new SqlParameter("@" + keyField, info[keyField]));
                if (string.IsNullOrEmpty(primaryKey))
                {
                    primaryKey = string.Format("{0}", info[keyField]);
                }
            }

            bool returnValue = SqlFactory.ExecuteNonQuery(builder.ToString(), paramList) > 0;
            //this.Log(OperateDataType.Delete, string.Empty, null, null, string.Format("{0}[{1}]对表{2}执行了删除操作，操作方法：{3}.", CurrEmployee.EmpName, CurrEmployee.EmpId, this.FullTableName, "BaseData.DeleteData"));
            return returnValue;
        }
        /// <summary>
        /// 删除数据(更新数据状态)
        /// </summary>
        /// <param name="key">需删除的key值</param>
        /// <returns>返回执行状态</returns>
        public bool DeleteData(object key)
        {
            string primaryKey = string.Empty;
            TInfo info = new TInfo();
            if (string.IsNullOrEmpty(this.ValidField))
            {
                return false;
            }
            List<string> keyArray = info.GetKeyArray();
            List<SqlParameter> paramList = new List<SqlParameter>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Update {0} Set {1} = 0 ", this.FullTableName, this.ValidField);
            builder.AppendFormat("Where 1=1 ");
            if (key is CompositeKey)
            {
                CompositeKey comKey = key as CompositeKey;
                foreach (KeyValuePair<string, object> item in comKey.KeyDic)
                {
                    builder.AppendFormat("And {0} = @{0} ", item.Key);
                    paramList.Add(new SqlParameter("@" + item.Key, item.Value));
                    if (string.IsNullOrEmpty(primaryKey))
                    {
                        primaryKey = string.Format("{0}", item.Value);
                    }
                }
            }
            else
            {
                foreach (string keyField in keyArray)
                {
                    builder.AppendFormat("And {0} = @{0} ", keyField);
                    paramList.Add(new SqlParameter("@" + keyField, key));
                    if (string.IsNullOrEmpty(primaryKey))
                    {
                        primaryKey = string.Format("{0}", key);
                    }
                }
            }

            bool returnValue = SqlFactory.ExecuteNonQuery(builder.ToString(), paramList) > 0;
            //this.Log(OperateDataType.Delete, string.Empty, null, null, string.Format("{0}[{1}]对表{2}执行了删除操作，操作方法：{3}.", CurrEmployee.EmpName, CurrEmployee.EmpId, this.FullTableName, "BaseData.DeleteData"));
            return returnValue;
        }
        public bool DeleteAll(string whereString)
        {
            return DeleteAll(whereString, new List<SqlParameter>());
        }
        public bool DeleteAll(string whereString, SqlParameter param)
        {
            List<SqlParameter> collection = new List<SqlParameter>();
            collection.Add(param);
            return DeleteAll(whereString, collection);
        }
        /// <summary>
        /// 根据条件删除数据(更新数据状态)
        /// </summary>
        /// <param name="whereString">where字符串</param>
        /// <param name="paramList">参数集合</param>
        /// <returns>返回执行状态</returns>
        public bool DeleteAll(string whereString, List<SqlParameter> paramList)
        {
            if (string.IsNullOrEmpty(this.ValidField))
            {
                return false;
            }
            if (paramList == null)
            {
                paramList = new List<SqlParameter>();
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Update {0} Set {1} = 0 ", this.FullTableName, this.ValidField);
            builder.AppendFormat("Where 1=1 ");
            if (!string.IsNullOrEmpty(whereString))
            {
                if (!whereString.TrimStart().StartsWith("And"))
                {
                    builder.AppendFormat("And ", whereString);
                }
                builder.AppendFormat("{0} ", whereString);
            }
            bool returnValue = SqlFactory.ExecuteNonQuery(builder.ToString(), paramList) > 0;
            //this.Log(OperateDataType.Delete, string.Empty, null, null, string.Format("{0}[{1}]对表{2}执行了删除操作，操作方法：{3}.", CurrEmployee.EmpName, CurrEmployee.EmpId, this.FullTableName, "BaseData.DeleteAll"));
            return returnValue;
        }
        /// <summary>
        /// 判断当前表中是否存在指定条件数据
        /// </summary>
        /// <param name="whereString"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public bool Exist(string whereString, string paramName, object paramValue)
        {
            return Exist(whereString, new string[] { paramName }, new object[] { paramValue });
        }
        /// <summary>
        /// 判断当前表中是否存在指定条件数据
        /// </summary>
        /// <param name="whereString"></param>
        /// <param name="paramNameArray"></param>
        /// <param name="paramValueArray"></param>
        /// <returns></returns>
        public bool Exist(string whereString, string[] paramNameArray, object[] paramValueArray)
        {
            int index = 0;
            List<SqlParameter> collection = new List<SqlParameter>();
            if (paramNameArray.Length != paramValueArray.Length)
            {
                throw new WarningException("参数名称集合与参数值集合长度不匹配.");
            }
            foreach (string paramName in paramNameArray)
            {
                string pName = paramName;
                if (!pName.StartsWith("@"))
                {
                    pName = "@" + pName;
                }
                collection.Add(new SqlParameter(pName, paramValueArray[index++]));
            }

            return Exist(whereString, collection);
        }
        /// <summary>
        /// 判断当前表中是否存在指定条件数据
        /// </summary>
        /// <param name="whereString">条件字符串</param>
        /// <param name="param">参数</param>
        /// <returns>返回是否存在</returns>
        public bool Exist(string whereString, SqlParameter param)
        {
            List<SqlParameter> collection = new List<SqlParameter>();
            collection.Add(param);

            return Exist(whereString, collection);
        }
        /// <summary>
        /// 判断当前表中是否存在指定条件数据
        /// </summary>
        /// <param name="whereString">条件字符串</param>
        /// <param name="paramList">参数集合</param>
        /// <returns>返回是否存在true/false</returns>
        public bool Exist(string whereString, List<SqlParameter> paramList = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Select Top 1 1 From {0} With(nolock) ", this.FullTableName);
            builder.AppendFormat("Where 1=1 ");
            if (!string.IsNullOrEmpty(whereString))
            {
                if (!whereString.TrimStart().StartsWith("And"))
                {
                    builder.AppendFormat("And ", whereString);
                }
                builder.AppendFormat("{0} ", whereString);
            }
            if (!string.IsNullOrEmpty(this.ValidField))
            {
                builder.AppendFormat("And {0} = 1 ", this.ValidField);
            }
            object returnValue = SqlFactory.ExecuteScalar(builder.ToString(), paramList);

            return (returnValue != null && returnValue != DBNull.Value);
        }
        /// <summary>
        /// 检查实体类，存在null值则用新定义类的属性来代替
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual TInfo NewInfo(TInfo info)
        {
            TInfo newInfo = this.NewInfo();
            foreach (string field in ColumnList)
            {
                if (!Helper.IsNotNull(info[field]) && Helper.IsNotNull(newInfo[field]))
                {
                    info[field] = newInfo[field];
                }
            }

            return info;
        }
        /// <summary>
        /// 获取表的默认值
        /// </summary>
        /// <returns>返回实体类对象</returns>
        public virtual TInfo NewInfo()
        {
            TInfo info = new TInfo();
            FillInfo(info, this.TableName);
            info.RecordStatus = RecordStatus.Add;

            return info;
        }
        /// <summary>
        /// 获取指定实体的默认值
        /// </summary>
        /// <typeparam name="TDetail">实体类型</typeparam>
        /// <returns>返回实体类对象</returns>
        public TDetail NewInfo<TDetail>() where TDetail : ActiveRecord, new()
        {
            TDetail detail = new TDetail();
            FillInfo(detail, detail.GetTableName());
            detail.RecordStatus = RecordStatus.Add;

            return detail;
        }
        /// <summary>
        /// 根据表名填充相应实体
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="info">实体类</param>
        /// <param name="tableName">表名</param>
        private void FillInfo<T>(T info, string tableName) where T : ActiveRecord, new()
        {
            string sqlString = string.Format(@"
                SELECT a.name FieldName,b.name DataType ,isnull(e.text, '') DefValue
                FROM  [{0}].[dbo].[syscolumns] a With(nolock)
                left join [{0}].[dbo].[systypes] b With(nolock) on a.xtype=b.xusertype
                inner join [{0}].[dbo].[sysobjects] d With(nolock) on a.id=d.id and d.xtype= 'U' and d.name <>'dtproperties'
                left join [{0}].[dbo].[syscomments] e With(nolock) on a.cdefault=e.id
                where b.name is not null
                AND d.name= @TableName
                order by a.id,a.colorder
            ", this.DataBaseName);

            SqlParameter collection = new SqlParameter("@TableName", tableName);

            //sa_read帐号不具备syscomments表的权限，所以此处使用write帐号进行查询
            DataTable table = SqlFactory.ExecuteReader(sqlString, collection);
            Dictionary<string, object> dicValues = new Dictionary<string, object>();
            Dictionary<string, string> dicTypes = new Dictionary<string, string>();
            Func<object, object> getValue = new Func<object, object>(value =>
            {
                if (value != null && value != DBNull.Value)
                {
                    if (value.ToString().IndexOf("newid") >= 0)
                    {
                        return Guid.NewGuid().ToString();
                    }
                    return value.ToString().Replace("('", "").Replace("')", "").Replace("(", "").Replace(")", "");
                }
                return value;
            });
            foreach (DataRow row in table.Rows)
            {
                dicValues.Add(row["FieldName"].ToString(), getValue(row["DefValue"]));
                dicTypes.Add(row["FieldName"].ToString(), row["DataType"].ToString());
            }
            foreach (string fieldName in this.ColumnList)
            {
                if (dicValues.ContainsKey(fieldName) && dicValues[fieldName] != null)
                {
                    switch (dicTypes[fieldName])
                    {
                        case "int":
                            info[fieldName] = Helper.GetInt32(dicValues[fieldName].ToString());
                            break;
                        case "tinyint":
                            info[fieldName] = Helper.GetByte(dicValues[fieldName]);
                            break;
                        case "bigint":
                            info[fieldName] = Helper.GetInt64(dicValues[fieldName]);
                            break;
                        case "datetime":
                            info[fieldName] = Helper.GetDateTime(dicValues[fieldName]);
                            break;
                        case "time":
                            info[fieldName] = Helper.GetTimeSpan(dicValues[fieldName]);
                            break;
                        case "decimal":
                            info[fieldName] = Helper.GetDecimal(dicValues[fieldName]);
                            break;
                        default:
                            info[fieldName] = Helper.GetString(dicValues[fieldName]);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 添加分页所需的参数
        /// </summary>
        /// <param name="paramList">参数集合</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页行数</param>
        protected void ToPagingParams(List<SqlParameter> paramList, int pageIndex, int pageSize)
        {
            int numStart = (pageIndex - 1) * pageSize + 1;
            int numEnd = pageSize * pageIndex;
            paramList.Add(new SqlParameter(this.NumberStartParam, numStart));
            paramList.Add(new SqlParameter(this.NumberEndParam, numEnd));
        }

        #region Log日志重载方法
        ///// <summary>
        ///// 记录其他表的操作日志
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="type"></param>
        ///// <param name="remark"></param>
        //protected void Log(string tableName, OperateDataType type, string remark)
        //{
        //    LogHelper.InfoLog(tableName, string.Empty, this.CurrEmployee.EmpId, type, null, null, remark);
        //}
        ///// <summary>
        ///// 记录当前表的操作日志（必须是对当前TInfo表的操作）
        ///// </summary>
        ///// <param name="type">操作类型</param>
        //protected void Log(OperateDataType type)
        //{
        //    string remark = CreateLogRemark(type);
        //    Log(type, remark);
        //}
        ///// <summary>
        ///// 记录当前表的操作日志（必须是对当前TInfo表的操作）
        ///// </summary>
        ///// <param name="type">操作类型</param>
        ///// <param name="remark">备注信息</param>
        //protected void Log(OperateDataType type, string remark)
        //{
        //    LogHelper.InfoLog(this.TableName, string.Empty, this.CurrEmployee.EmpId, type, null, null, remark);
        //}
        ///// <summary>
        ///// 记录当前表的操作日志（必须是对当前TInfo表的操作）
        ///// </summary>
        ///// <param name="type">操作类型</param>
        ///// <param name="primaryKey">唯一标识键</param>
        ///// <param name="beforeModel">操作前的数据</param>
        ///// <param name="afterModel">操作后的数据</param>
        ///// <param name="remark">备注信息</param>
        //protected void Log(OperateDataType type, string primaryKey, object beforeModel, object afterModel, string remark)
        //{
        //    if (string.IsNullOrEmpty(remark))
        //    {
        //        remark = CreateLogRemark(type);
        //    }
        //    LogHelper.InfoLog(this.TableName, primaryKey, this.CurrEmployee.EmpId, type, beforeModel, afterModel, remark);
        //}
        ///// <summary>
        ///// 记录当前表的操作日志（必须是对当前TInfo表的操作）
        ///// </summary>
        ///// <param name="type">操作类型</param>
        ///// <param name="primaryKey">唯一标识键</param>
        ///// <param name="beforeValue">操作前的值</param>
        ///// <param name="afterValue">操作后的值</param>
        ///// <param name="remark">备注信息</param>
        //protected void Log(OperateDataType type, string primaryKey, string beforeValue, object afterValue, string remark)
        //{
        //    if (string.IsNullOrEmpty(remark))
        //    {
        //        remark = CreateLogRemark(type);
        //    }
        //    LogHelper.InfoLog(this.TableName, primaryKey, this.CurrEmployee.EmpId, type, beforeValue, afterValue, remark);
        //}

        //private string CreateLogRemark(OperateDataType type)
        //{
        //    StackTrace st = new StackTrace(true);
        //    MethodBase method = st.GetFrame(1).GetMethod();
        //    string fullName = method.DeclaringType.FullName;
        //    return string.Format(@"{0}[{1}]对表{2}执行了{3}操作，操作方法：{4}.", CurrEmployee.EmpName, CurrEmployee.EmpId, this.FullTableName, EnumHelper.GetEnumDescription(type), (fullName + method.Name));
        //}
        #endregion
    }
}
