using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.Spider
{
    public class Topic
    {
        /// <summary>
        /// 文章名
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 地址
        /// </summary>
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CommitDate { get; set; } = DateTime.Parse("1900-1-1");

        /// <summary>
        /// 包含的图片地址
        /// </summary>
        public List<string> ContentImageUrl { get; set; } = new List<string>();
    }
}
