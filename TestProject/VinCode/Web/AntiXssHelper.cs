using System;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;

namespace VinCode.Web
{
    public static class AntiXssHelper
    {
        #region 防止XSS输入

        public static string AntiXssInput(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            #region 组合不需要过滤的字符集

            MidCodeCharts midCodes = MidCodeCharts.None;
            foreach (MidCodeCharts charts in Enum.GetValues(typeof (MidCodeCharts)))
            {
                midCodes = midCodes | charts;
            }
            UpperMidCodeCharts upperMidCodes = UpperMidCodeCharts.None;
            foreach (UpperMidCodeCharts charts in Enum.GetValues(typeof (UpperMidCodeCharts)))
            {
                upperMidCodes = upperMidCodes | charts;
            }

            #endregion

            #region 标记安全不过滤字符集

            UnicodeCharacterEncoder.MarkAsSafe(
                //默认标记英文安全不过滤
                LowerCodeCharts.Default,
                //该字符集中无需要标记字符
                LowerMidCodeCharts.None,
                //标记斜杠半角引号等字符安全不过滤
                midCodes,
                //标记中日韩字符安全，标点字符安全
                upperMidCodes,
                //标记半角和全角标点符号安全不过滤
                UpperCodeCharts.HalfWidthAndFullWidthForms);

            #endregion

            string filted = Sanitizer.GetSafeHtmlFragment(str);
            return filted;
        }

        #endregion

        #region 防xss输出

        /// <summary>
        /// 检测过滤纯文本内容
        /// </summary>
        /// <param name="txtStr">需要过滤的纯文本</param>
        /// <returns>过滤后的纯文本</returns>
        /// <remarks>适用于纯文本的过滤</remarks>
        public static string AntiXsssOutput(this string txtStr)
        {
            if (string.IsNullOrEmpty(txtStr))
            {
                return "";
            }
            string result = txtStr;

            #region 组合不需要过滤的字符集

            MidCodeCharts midCodes = MidCodeCharts.None;
            foreach (MidCodeCharts charts in Enum.GetValues(typeof (MidCodeCharts)))
            {
                midCodes = midCodes | charts;
            }
            UpperMidCodeCharts upperMidCodes = UpperMidCodeCharts.None;
            foreach (UpperMidCodeCharts charts in Enum.GetValues(typeof (UpperMidCodeCharts)))
            {
                upperMidCodes = upperMidCodes | charts;
            }

            #endregion

            #region 标记安全不过滤字符集

            UnicodeCharacterEncoder.MarkAsSafe(
                //默认标记英文安全不过滤
                LowerCodeCharts.Default,
                //该字符集中无需要标记字符
                LowerMidCodeCharts.None,
                //标记斜杠半角引号等字符安全不过滤
                midCodes,
                //标记中日韩字符安全，标点字符安全
                upperMidCodes,
                //标记半角和全角标点符号安全不过滤
                UpperCodeCharts.HalfWidthAndFullWidthForms);

            #endregion

            string filted = Encoder.HtmlEncode(result);
            return filted;
        }

        #endregion

        public static string MakePlainText(this string x)
        {
            string filterString = x == null ? "" : x.Trim();

            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.Replace(Environment.NewLine, "₪");
                filterString = new Regex(@"<[^>]*>").Replace(filterString, "");
                filterString = new Regex(@"\s{2,}").Replace(filterString, "");
                return new Regex(@"[₪]+").Replace(filterString, "<br />");
            }
            return filterString;
        }

        /// <summary>
        /// 此处过滤危险HTML方法
        /// </summary>
        /// <param name="html">html</param>
        /// <returns></returns>
        public static string FilterHTML(string html)
        {
            if (html == null)
                return "";

            //过滤 script
            Regex regex_script1 = new Regex("(<script[//s//S]*?///script//s*>)", RegexOptions.IgnoreCase);
            Regex regex_script2 = new Regex("(<(script[//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_script1.Replace(html, "");
            html = regex_script1.Replace(html, "");

            //过滤 <iframe> 标签
            Regex regex_iframe1 = new Regex("(<iframe [//s//S]+<iframe//s*>)", RegexOptions.IgnoreCase);
            Regex regex_iframe2 = new Regex("(<(iframe [//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_iframe1.Replace(html, "");
            html = regex_iframe2.Replace(html, "");

            //过滤 <frameset> 标签
            Regex regex_frameset1 = new Regex("(<frameset [//s//S]+<frameset //s*>)", RegexOptions.IgnoreCase);
            Regex regex_frameset2 = new Regex("(<(frameset [//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_frameset1.Replace(html, "");
            html = regex_frameset2.Replace(html, "");

            //过滤 <frame> 标签
            Regex regex_frame1 = new Regex("(<frame[//s//S]+<frame //s*>)", RegexOptions.IgnoreCase);
            Regex regex_frame2 = new Regex("(<(frame[//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_frame1.Replace(html, "");
            html = regex_frame2.Replace(html, "");

            //过滤 <form> 标签
            Regex regex_form1 = new Regex("(<(form [//s//S]*?)>)", RegexOptions.IgnoreCase);
            Regex regex_form2 = new Regex("(<(/form[//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_form1.Replace(html, "");
            html = regex_form2.Replace(html, "");

            //过滤 on: 的事件
            //过滤on 带单引号的 过滤on  带双引号的 过滤on 不带有引号的
            string regOn = @"<[//s//S]+ (on)[a-zA-Z]{4,20} *= *[//S ]{3,}>";
            string regOn2 = @"((on)[a-zA-Z]{4,20} *= *'[^']{3,}')|((on)[a-zA-Z]{4,20} *= */""[^/""]{3,}/"")|((on)[a-zA-Z]{4,20} *= *[^>/ ]{3,})";
            html = GetReplace(html, regOn, regOn2, "");


            //过滤 javascript: 的事件
            regOn = @"<[//s//S]+ (href|src|background|url|dynsrc|expression|codebase) *= *[ /""/']? *(javascript:)[//S]{1,}>";
            regOn2 = @"(' *(javascript|vbscript):([//S^'])*')|(/"" *(javascript|vbscript):[//S^/""]*/"")|([^=]*(javascript|vbscript):[^/> ]*)";
            html = GetReplace(html, regOn, regOn2, "");

            Regex regwinfile = new Regex("[~!$\\^\\*&\\\\/\\?\\|:{}()';=\"]", RegexOptions.IgnoreCase);
            html = regwinfile.Replace(html, "");
            return html;
        }

        /// <summary>
        /// 此处过滤危险HTML方法(区别上面的是最后两句话里面有当文件用的)
        /// </summary>
        /// <param name="html">html</param>
        /// <returns></returns>
        public static string FilterHTMLCommon(string html)
        {
            if (html == null)
                return "";




            //过滤 script
            Regex regex_script1 = new Regex("(<script[//s//S]*?///script//s*>)", RegexOptions.IgnoreCase);
            Regex regex_script2 = new Regex("(<(script[//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_script1.Replace(html, "");
            html = regex_script1.Replace(html, "");

            //过滤 <iframe> 标签
            Regex regex_iframe1 = new Regex("(<iframe [//s//S]+<iframe//s*>)", RegexOptions.IgnoreCase);
            Regex regex_iframe2 = new Regex("(<(iframe [//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_iframe1.Replace(html, "");
            html = regex_iframe2.Replace(html, "");

            //过滤 <frameset> 标签
            Regex regex_frameset1 = new Regex("(<frameset [//s//S]+<frameset //s*>)", RegexOptions.IgnoreCase);
            Regex regex_frameset2 = new Regex("(<(frameset [//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_frameset1.Replace(html, "");
            html = regex_frameset2.Replace(html, "");

            //过滤 <frame> 标签
            Regex regex_frame1 = new Regex("(<frame[//s//S]+<frame //s*>)", RegexOptions.IgnoreCase);
            Regex regex_frame2 = new Regex("(<(frame[//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_frame1.Replace(html, "");
            html = regex_frame2.Replace(html, "");

            //过滤 <form> 标签
            Regex regex_form1 = new Regex("(<(form [//s//S]*?)>)", RegexOptions.IgnoreCase);
            Regex regex_form2 = new Regex("(<(/form[//s//S]*?)>)", RegexOptions.IgnoreCase);
            html = regex_form1.Replace(html, "");
            html = regex_form2.Replace(html, "");

            //过滤 on: 的事件
            //过滤on 带单引号的 过滤on  带双引号的 过滤on 不带有引号的
            string regOn = @"<[//s//S]+ (on)[a-zA-Z]{4,20} *= *[//S ]{3,}>";
            string regOn2 = @"((on)[a-zA-Z]{4,20} *= *'[^']{3,}')|((on)[a-zA-Z]{4,20} *= */""[^/""]{3,}/"")|((on)[a-zA-Z]{4,20} *= *[^>/ ]{3,})";
            html = GetReplace(html, regOn, regOn2, "");


            //过滤 javascript: 的事件
            regOn = @"<[//s//S]+ (href|src|background|url|dynsrc|expression|codebase) *= *[ /""/']? *(javascript:)[//S]{1,}>";
            regOn2 = @"(' *(javascript|vbscript):([//S^'])*')|(/"" *(javascript|vbscript):[//S^/""]*/"")|([^=]*(javascript|vbscript):[^/> ]*)";
            html = GetReplace(html, regOn, regOn2, "");
           
            return html;
        }

        /// <summary>
        /// 正则双重过滤
        ///       //splitKey1 第一个正则式匹配//splitKey2 匹配结果中再次匹配进行替换
        /// </summary>
        /// <param name="content"></param>
        /// <param name="splitKey1"></param>
        /// <param name="splitKey2"></param>
        /// <param name="newChars"></param>
        /// <returns></returns>
        public static string GetReplace(string content, string splitKey1, string splitKey2, string newChars)
        {
      

            if (splitKey1 != null && splitKey1 != "" && splitKey2 != null && splitKey2 != "")
            {
                Regex rg = new Regex(splitKey1);
                System.Text.RegularExpressions.MatchCollection mc = rg.Matches(content);

                foreach (System.Text.RegularExpressions.Match mc1 in mc)
                {
                    string oldChar = mc1.ToString();
                    string newChar = new Regex(splitKey2, RegexOptions.IgnoreCase).Replace(oldChar, newChars);
                    content = content.Replace(oldChar, newChar);
                }
                return content;
            }
            else
            {
                if (splitKey2 != null && splitKey2 != "")
                {
                    Regex rg = new Regex(splitKey2, RegexOptions.IgnoreCase);
                    return rg.Replace(content, newChars);
                }
            }
            return content;
        }

        /// <summary>
        /// 过滤HTML标签
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripHTML(string source)
        {
            try
            {
                string result;
                result = source.Replace("\r", " ");
                result = result.Replace("\n", " ");
                result = result.Replace("'", " ");
                result = result.Replace("\t", string.Empty);
                result = Regex.Replace(result, @"( )+", " ");
                result = Regex.Replace(result, @"<( )*head([^>])*>", "<head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*td([^>])*>", "\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*br( )*>", "\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*li( )*>", "\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&nbsp;", " ", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&bull;", " * ", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&lsaquo;", "<", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&rsaquo;", ">", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&trade;", "(tm)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&frasl;", "/", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<", "<", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @">", ">", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&copy;", "(c)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&reg;", "(r)", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);
                result = result.Replace("\n", "\r");
                result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\t)", "\t\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\r)", "\t\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)( )+(\t)", "\r\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)(\t)+", "\r\t", RegexOptions.IgnoreCase);
                result = result.Replace("\r", " ").Replace("\t", string.Empty);

                return result;
            }
            catch
            {
                return source;
            }
                 
        }
        /// <summary>
        /// 标题用 过滤 不去除 Windows 文件名非法字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string TitleNoHtmlCommon(string source)
        {
            var result=StripHTML(source);
            return FilterHTMLCommon(result);
        }
        /// <summary>
        /// 标题用 过滤去除 Windows 文件名非法字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string TitleNoHtml(string source)
        {
            var result = StripHTML(source);
            return FilterHTML(result);
        }
    }





   
}

