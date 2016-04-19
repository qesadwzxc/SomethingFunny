using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;

namespace TestConsoleApplication.Spider
{
    public static class FormatHtml
    {
        //static IConfiguration config = Configuration.Default.WithDefaultLoader();

        public static async Task<IEnumerable<Topic>> TopicFormat(string htmlText)
        {
            CQ dom = htmlText;
            
            //var document = await BrowsingContext.New(config).OpenAsync(act => act.Content(htmlText));

            return null; //TopicFormat(document);
        }

        public static void TopicFormat(this Topic topic, string htmlText)
        {
        }

        public static Task<bool> DownloadPic(string picUrl)
        {
            return Task.Run(() => { return false; });
        }

        public static string CommentFormat(this Topic topic, string htmlText)
        {
            return null;
        }
    }
}
