using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApi.Models
{
    public class CommonResponse<T> where T : class, new()
    {
        public string reason { get; set; }

        public T result { get; set; }

        public int error_code { get; set; }
    }
}