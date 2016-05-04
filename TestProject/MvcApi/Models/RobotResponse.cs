using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApi.Models
{
    public class RobotResponse
    {
        public string reason { get; set; }

        public Result result { get; set; }
        public class Result
        {
            public int code { get; set; }
            public string text { get; set; }
        }
        public int error_code { get; set; }
    }
}