using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication.Common
{
    public class Globals : GlobalVariables
    { }

    public class GlobalVariables
    {
        public static readonly DateTime MinValue = DateTime.Parse("1900-1-1");
        public static readonly string G_DatabaseName = "IDONTKNOW";
    }
}