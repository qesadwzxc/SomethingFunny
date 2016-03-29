using System.Collections.Generic;
using System.Text.RegularExpressions;
using VinCode.ProjectEnum;

namespace VinCode
{
    public class RegexHelper
    {
        #region 正则判断条件
        //正则判断条件字典。根据需要在此处添加条件、在正则枚举中添加枚举类型。
        //键值（Key）建议用枚举写，避免遗忘和冲突。
        private static Dictionary<int, string> strPatern = new Dictionary<int, string>()
        { 
            //邮箱正则
            { (int)RegexEnum.Email, @"(^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$)" }, 
            //手机正则
            { (int)RegexEnum.MobilePhone,@"(^1[3-8]\d{9}$)" },
            //电话正则
            { (int)RegexEnum.TelPhone,@"(^\d{6,20}$)" },
            //QQ正则
            { (int)RegexEnum.QQNumber,@"(^[1-9]\d{4,10}$)" },
            //2~50位英文正则
            { (int)RegexEnum.English,@"(^[A-Za-z\s\.]{2,50}$)" },
            //RTX正则(暂时不用，待修改)
            { (int)RegexEnum.RTX,@"([\u4e00-\u9fa5]{2,4})(\d{0,5})" },
            //电话区号正则
            { (int)RegexEnum.StateCode,@"(^[\(86\)]?\d{3,4}$)" }
        };
        #endregion

        /// <summary>
        /// 根据选择的判断语句进行正则判断(若选择的正则条件不存在同样返回false)
        /// </summary>
        /// <param name="strTel">需要判断的字符串</param>
        /// <param name="select">正则枚举</param>
        /// <returns>匹配-true，不匹配-false</returns>
        public static bool RegexValid(string strTel, RegexEnum select)
        {
            bool isMatch = false;
            if (strPatern.ContainsKey((int)select))
            {
                Regex reg = new Regex(strPatern[(int)select]);
                isMatch = reg.IsMatch(strTel);
            }
            return isMatch;
        }
    }
}
