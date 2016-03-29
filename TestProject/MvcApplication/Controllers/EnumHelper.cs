using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI.WebControls;

namespace MvcApplication.Controllers
{
    public static class EnumHelper
    {
        /// <summary>获取枚举的描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum value)
        {
            if (value == null)
            {
                return "";
            }
            FieldInfo field = value.GetType().GetField(value.ToString());
            return ((DescriptionAttribute)System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))).Description;
        }

        /// <summary>将枚举转化为集合（Text显示为枚举的Value）
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IList ConvertEnumToListDisplayValue(Type enumType)
        {
            ArrayList arrayList = new ArrayList();
            foreach (int num in Enum.GetValues(enumType))
            {
                ListItem value = new ListItem(Enum.GetName(enumType, num), num.ToString());
                arrayList.Add(value);
            }
            return arrayList;
        }

        /// <summary>将枚举转化为集合（Text显示为枚举的Description）
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IEnumerable ConvertEnumToListDisplayDescription(Type enumType)
        {
            ArrayList arrayList = new ArrayList();
            foreach (int num in Enum.GetValues(enumType))
            {
                string name = Enum.GetName(enumType, num);
                ListItem value = new ListItem((Enum.Parse(enumType, name) as Enum).GetEnumDescription(), num.ToString());
                arrayList.Add(value);
            }
            return arrayList;
        }
    }
}