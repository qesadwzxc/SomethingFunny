using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MvcApplication.Common
{
    /// <summary>
    /// Linq Distinct 比较器
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class PropertyEqualityComparer<T> : IEqualityComparer<T>
    {
        //用委托代替反射调用取值，提高性能
        private Func<T, Object> getPropertyValueFunc = null;
        private string propertyName = string.Empty;
        private bool isDataRow = false;

        public PropertyEqualityComparer(string propertyName)
        {
            if (typeof(T) == typeof(DataRow))
            {
                this.isDataRow = true;
                this.propertyName = propertyName;
            }
            else
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

                if (propertyInfo == null)
                {
                    throw new WarningException(string.Format("{0}不是类型{1}的有效属性.",
                        propertyName, typeof(T)));
                }

                ParameterExpression expPara = Expression.Parameter(typeof(T), "obj");
                MemberExpression me = Expression.Property(expPara, propertyInfo);
                getPropertyValueFunc = Expression.Lambda<Func<T, object>>(me, expPara).Compile();
            }
        }

        public bool Equals(T x, T y)
        {
            object xValue;
            object yValue;

            if (isDataRow)
            {
                xValue = (x as DataRow)[propertyName];
                yValue = (y as DataRow)[propertyName];
            }
            else
            {
                xValue = getPropertyValueFunc(x);
                yValue = getPropertyValueFunc(y);
            }

            if (xValue == null || xValue == DBNull.Value)
            {
                return yValue == null || yValue == DBNull.Value;
            }
            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            object propertyValue;
            if (isDataRow)
            {
                propertyValue = (obj as DataRow)[propertyName];
            }
            else
            {
                propertyValue = getPropertyValueFunc(obj);
            }

            if (propertyValue == null || propertyValue == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return propertyValue.GetHashCode();
            }
        }
    }
}
