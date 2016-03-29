using System.ComponentModel;

namespace VinCode.ProjectEnum
{
    public enum RegexEnum
    {
        /// <summary>电子邮箱
        /// </summary>
        [Description("电子邮箱")]
        Email = 0,
        /// <summary>手机
        /// </summary>
        [Description("手机")]
        MobilePhone = 1,
        /// <summary>电话
        /// </summary>
        [Description("电话")]
        TelPhone = 2,
        /// <summary>QQ
        /// </summary>
        [Description("QQ")]
        QQNumber = 3,
        /// <summary>2~50位英文
        /// </summary>
        [Description("2~50位英文")]
        English = 4,
        /// <summary>RTX正则
        /// </summary>
        [Description("RTX正则")]
        RTX = 5,
        /// <summary>电话区号正则
        /// </summary>
        [Description("电话区号正则")]
        StateCode = 6,
        /// <summary>中文正则
        /// </summary>
        [Description("中文正则")]
        Chinese = 7
    }
}
