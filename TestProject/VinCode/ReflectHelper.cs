using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace VinCode
{
    public class ReflectHelper<T> where T : new()
    {
        /// <summary>
        /// 表格填充数据模型
        /// </summary>
        /// <param name="dt">用于数据填充的表格</param>
        /// <returns>T类型实体集合</returns>
        public static List<T> FillModel(DataTable dt)
        {
            Type tType = typeof(T);
            List<T> tModelList = new List<T>();
            PropertyInfo[] modelPropertyInfo = tType.GetProperties();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    T tModel = new T();
                    //循环对每个属性进行赋值
                    foreach (PropertyInfo info in modelPropertyInfo)
                    {
                        //判断表格行中对应该属性的字段是否存在，存在则填充，不存在则忽略
                        if (info.CanWrite && dt.Columns.Contains(info.Name))
                        {
                            //若类型转换失败则跳过此字段。可根据需要自行修改此处
                            try
                            {
                                tType.GetProperty(info.Name).SetValue(tModel, Convert.ChangeType(dt.Rows[i][info.Name], info.PropertyType), null);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    tModelList.Add(tModel);
                }
            }
            return tModelList;
        }
    }
}
