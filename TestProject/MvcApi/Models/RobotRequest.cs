using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApi.Models
{
    public class RobotRequest
    {
        public string key { get; set; }
        public string info { get; set; }
        public string dtype { get; set; } = "json";
    }
}