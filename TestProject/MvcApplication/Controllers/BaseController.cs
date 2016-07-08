using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class BaseController : IResultFilter
    {
        //
        // 摘要:
        //     在操作结果执行后调用。
        //
        // 参数:
        //   filterContext:
        //     筛选器上下文。
        public void OnResultExecuted(ResultExecutedContext filterContext)
        { }
        //
        // 摘要:
        //     在操作结果执行之前调用。
        //
        // 参数:
        //   filterContext:
        //     筛选器上下文。
        public void OnResultExecuting(ResultExecutingContext filterContext)
        { }
    }
}
