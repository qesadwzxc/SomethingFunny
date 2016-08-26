using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvcApplication.Common;

namespace MvcApplication.Models
{
    /// <summary>
    /// 实体类基类
    /// </summary>
    [Serializable]
    public class ActiveRecord
    {
        /// <summary>
        /// 获取全部字段
        /// </summary>
        /// <returns>获取全部字段</returns>
        public List<string> GetAllColumns()
        {
            List<string> fields = new List<string>();
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(PropertyAttribute), true);
                if (objAttrs.Length > 0)
                {
                    PropertyAttribute attr = objAttrs[0] as PropertyAttribute;
                    if (attr != null)
                    {
                        fields.Add(propInfo.Name);
                        continue;
                    }
                }
                objAttrs = propInfo.GetCustomAttributes(typeof(KeyPropertyAttribute), true);
                if (objAttrs.Length > 0)
                {
                    KeyPropertyAttribute attr = objAttrs[0] as KeyPropertyAttribute;
                    if (attr != null)
                    {
                        fields.Add(propInfo.Name);
                    }
                }
            }

            return fields;
        }
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns>获取表名</returns>
        public string GetTableName()
        {
            string tableName = string.Empty;
            object[] objAttrs = this.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            foreach (object obj in objAttrs)
            {
                TableAttribute attr = obj as TableAttribute;
                if (attr != null)
                {
                    tableName = attr.TableName;
                    break;
                }
            }

            return tableName;
        }
        /// <summary>
        /// 获取有效字段
        /// </summary>
        /// <returns>获取有效字段</returns>
        public string GetValidField()
        {
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(ValidAttribute), true);
                if (objAttrs.Length > 0)
                {
                    ValidAttribute attr = objAttrs[0] as ValidAttribute;
                    if (attr != null)
                    {
                        return propInfo.Name;
                    }
                }
            }

            return string.Empty;
        }
        /// <summary>
        /// 获取主键值
        /// </summary>
        /// <returns>获取主键值</returns>
        public string GetKeyField()
        {
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(KeyPropertyAttribute), true);
                if (objAttrs.Length > 0)
                {
                    KeyPropertyAttribute attr = objAttrs[0] as KeyPropertyAttribute;
                    if (attr != null)
                    {
                        return propInfo.Name;
                    }
                }
            }

            return string.Empty;
        }
        /// <summary>
        /// 获取自动增长值
        /// </summary>
        /// <returns>获取自动增长值</returns>
        public string GetIncreaseField()
        {
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(IncreaseAttribute), true);
                if (objAttrs.Length > 0)
                {
                    IncreaseAttribute attr = objAttrs[0] as IncreaseAttribute;
                    if (attr != null)
                    {
                        return propInfo.Name;
                    }
                }
            }

            return string.Empty;
        }
        /// <summary>
        /// 获取多主键
        /// </summary>
        /// <returns>获取多主键</returns>
        public List<string> GetKeyArray()
        {
            List<string> keyList = new List<string>();
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(KeyPropertyAttribute), true);
                if (objAttrs.Length > 0)
                {
                    KeyPropertyAttribute attr = objAttrs[0] as KeyPropertyAttribute;
                    if (attr != null)
                    {
                        keyList.Add(propInfo.Name);
                    }
                }
            }

            return keyList;
        }
        /// <summary>
        /// 获取创建字段
        /// </summary>
        /// <returns>获取创建字段</returns>
        public CreateModel GetCreateField()
        {
            CreateModel model = null;
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(CreateAttribute), true);
                if (objAttrs.Length > 0)
                {
                    CreateAttribute attr = objAttrs[0] as CreateAttribute;
                    if (attr != null)
                    {
                        if (model == null)
                        {
                            model = new CreateModel();
                        }
                        if (propInfo.PropertyType == typeof(int) || propInfo.PropertyType == typeof(Nullable<int>))
                        {
                            model.CreateIdField = propInfo.Name;
                            if (propInfo.CanRead)
                            {
                                model.CreateId = Convert.ToInt32(propInfo.GetValue(this, null));
                            }
                        }
                        else if (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(Nullable<DateTime>))
                        {
                            model.CreateTimeField = propInfo.Name;
                            if (propInfo.CanRead)
                            {
                                model.CreateTime = Convert.ToDateTime(propInfo.GetValue(this, null));
                            }
                        }
                    }
                }
            }

            return model;
        }
        /// <summary>
        /// 获取修改字段
        /// </summary>
        /// <returns>获取修改字段</returns>
        public ModifyModel GetModifyField()
        {
            ModifyModel model = null;
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(ModifyAttribute), true);
                if (objAttrs.Length > 0)
                {
                    ModifyAttribute attr = objAttrs[0] as ModifyAttribute;
                    if (attr != null)
                    {
                        if (model == null)
                        {
                            model = new ModifyModel();
                        }
                        if (propInfo.PropertyType == typeof(int) || propInfo.PropertyType == typeof(Nullable<int>))
                        {
                            model.ModifyIdField = propInfo.Name;
                            if (propInfo.CanRead && propInfo.GetValue(this, null) != null)
                            {
                                model.ModifyId = Convert.ToInt32(propInfo.GetValue(this, null));
                            }
                        }
                        else if (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(Nullable<DateTime>))
                        {
                            model.ModifyTimeField = propInfo.Name;
                            if (propInfo.CanRead && propInfo.GetValue(this, null) != null)
                            {
                                model.ModifyTime = Convert.ToDateTime(propInfo.GetValue(this, null));
                            }
                        }
                    }
                }
            }

            return model;
        }
        /// <summary>
        /// 复制当前对象属性，非深度复制
        /// </summary>
        /// <returns>返回新的对象</returns>
        public ActiveRecord Copy()
        {
            ActiveRecord data = new ActiveRecord();
            foreach (PropertyInfo pro in this.GetType().GetProperties())
            {
                if (pro.CanWrite && pro.CanRead)
                {
                    data[pro.Name] = this[pro.Name];
                }
            }

            return data;
        }
        /// <summary>
        /// 从DataTable组织泛型集合
        /// </summary>
        /// <param name="table">来源Table</param>
        /// <returns>返回泛型集合</returns>
        public List<ActiveRecord> LoadData(DataTable table)
        {
            return Helper.LoadData<ActiveRecord>(table);
        }
        public List<TInfo> LoadData<TInfo>(DataTable table) where TInfo : ActiveRecord, new()
        {
            return Helper.LoadData<TInfo>(table);
        }
        /// <summary>
        /// 复制Model
        /// </summary>
        /// <param name="model">来源Model</param>
        /// <param name="allowNull">允许赋null值</param>
        /// <returns>返回当前Model</returns>
        public ActiveRecord LoadData(ActiveRecord model, bool allowNull = false)
        {
            return LoadData(model, allowNull, new string[] { });
        }
        /// <summary>
        /// 复制Model
        /// </summary>
        /// <param name="model">来源Model</param>
        /// <param name="allowNull">允许赋null值</param>
        /// <param name="exFields">排除掉的字段</param>
        /// <returns>返回当前Model</returns>
        public ActiveRecord LoadData(ActiveRecord model, bool allowNull, params string[] exFields)
        {
            List<string> exList = exFields.ToList();
            Dictionary<string, object> fromDic = new Dictionary<string, object>();
            foreach (PropertyInfo pro in model.GetType().GetProperties())
            {
                if (pro.CanRead && !exList.Contains(pro.Name))
                {
                    fromDic.Add(pro.Name, pro.GetValue(this, null));
                }
            }

            PropertyInfo[] proArray = this.GetType().GetProperties();
            foreach (PropertyInfo pro in proArray)
            {
                if (pro.CanWrite && fromDic.ContainsKey(pro.Name))
                {
                    if (allowNull || fromDic[pro.Name] != null)
                    {
                        pro.SetValue(this, fromDic[pro.Name], null);
                    }
                }
            }

            return this;
        }
        /// <summary>
        /// 从参数Model中读取数据填充到当前Model
        /// </summary>
        /// <typeparam name="TInfo">Info类型</typeparam>
        /// <param name="model">来源Model</param>
        /// <param name="allowNull">允许赋null值</param>
        /// <param name="exFields">排除掉的字段</param>
        /// <returns>返回当前Model</returns>
        public TInfo LoadData<TInfo>(TInfo model, bool allowNull, params string[] exFields) where TInfo : ActiveRecord, new()
        {
            List<string> exList = exFields.ToList();
            Dictionary<string, object> fromDic = new Dictionary<string, object>();
            foreach (PropertyInfo pro in model.GetType().GetProperties())
            {
                if (pro.DeclaringType.Name.Equals("ActiveRecord"))
                {
                    continue;
                }
                if (pro.CanRead && !exList.Contains(pro.Name))
                {
                    fromDic.Add(pro.Name, model[pro.Name]);
                }
            }

            PropertyInfo[] proArray = this.GetType().GetProperties();
            foreach (PropertyInfo pro in proArray)
            {
                if (pro.DeclaringType.Name.Equals("ActiveRecord"))
                {
                    continue;
                }
                if (pro.CanWrite && fromDic.ContainsKey(pro.Name))
                {
                    if (allowNull || fromDic[pro.Name] != null)
                    {
                        if (fromDic[pro.Name] is DateTime && pro.CanRead)
                        {
                            DateTime oldDate = (DateTime)pro.GetValue(this, null);
                            DateTime currDate = (DateTime)fromDic[pro.Name];
                            if (currDate < Globals.MinValue)
                            {
                                currDate = Globals.MinValue;
                            }
                            if (!(currDate == Globals.MinValue && oldDate > Globals.MinValue))
                            {
                                pro.SetValue(this, fromDic[pro.Name], null);
                            }
                        }
                        else
                        {
                            pro.SetValue(this, fromDic[pro.Name], null);
                        }
                    }
                }
            }

            return (TInfo)this;
        }


        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="fieldName">属性名</param>
        /// <returns></returns>
        public object this[string fieldName]
        {
            get
            {
                PropertyInfo[] proArray = this.GetType().GetProperties();
                foreach (PropertyInfo pro in proArray)
                {
                    if (pro.Name.Equals(fieldName) && pro.CanRead)
                    {
                        object returnValue = pro.GetValue(this, null);
                        if (returnValue != null && returnValue is DateTime)
                        {
                            DateTime returnDate = Globals.MinValue;
                            if (DateTime.TryParse(returnValue.ToString(), out returnDate) && returnDate <= Globals.MinValue)
                            {
                                return Globals.MinValue;
                            }
                        }

                        return returnValue;
                    }
                }
                return null;
            }
            set
            {
                PropertyInfo[] proArray = this.GetType().GetProperties();
                foreach (PropertyInfo pro in proArray)
                {
                    if (pro.Name.Equals(fieldName) && pro.CanWrite)
                    {
                        if (value is Guid)
                        {
                            pro.SetValue(this, value.ToString(), null);
                        }
                        else
                        {
                            if (pro.PropertyType.FullName != value.GetType().FullName)
                            {
                                if (pro.PropertyType.FullName.Equals(typeof(byte).FullName) && value.GetType() == typeof(int))
                                {
                                    pro.SetValue(this, Convert.ToByte(value), null);
                                    return;
                                }
                            }
                            pro.SetValue(this, value, null);
                        }
                    }
                }
            }
        }

        private RecordStatus recordStatus = RecordStatus.View;
        /// <summary>
        /// 数据状态
        /// </summary>
        public RecordStatus RecordStatus
        {
            get { return recordStatus; }
            set { this.recordStatus = value; }
        }

        /// <summary>
        /// 验证属性值
        /// </summary>
        public void CheckData()
        {
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(StringLengthAttribute), true);
                if (objAttrs.Length > 0)
                {
                    StringLengthAttribute attr = objAttrs[0] as StringLengthAttribute;
                    if (attr != null)
                    {
                        object value = propInfo.GetValue(this, null);
                        if (value != null)
                        {
                            int length = value.ToString().Length;
                            if (length < attr.MinimumLength || length > attr.MaximumLength)
                            {
                                string fieldName = propInfo.Name;
                                object[] displayAttr = propInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                                if (displayAttr.Length > 0)
                                {
                                    fieldName = (displayAttr[0] as DisplayNameAttribute).DisplayName;
                                }
                                if (length < attr.MinimumLength)
                                {
                                    throw new WarningException(string.Format("[{0}]的值长度必须大于{1}.", fieldName, attr.MinimumLength.ToString()));
                                }
                                else
                                {
                                    throw new WarningException(string.Format("[{0}]的值长度必须小于{1}.", fieldName, attr.MaximumLength.ToString()));
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
