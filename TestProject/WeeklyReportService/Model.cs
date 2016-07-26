using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WeeklyReportService
{
    public class DateModel
    {
        [JsonProperty("error_code")]
        public int Error_Code { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }
    }

    public class Result
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("holiday")]
        public string Holiday { get; set; }
        [JsonProperty("avoid")]
        public string Avoid { get; set; }
        [JsonProperty("animalsYear")]
        public string AnimalsYear { get; set; }
        [JsonProperty("desc")]
        public string Desc { get; set; }
        [JsonProperty("weekday")]
        public string Weekday { get; set; }
        [JsonProperty("suit")]
        public string Suit { get; set; }
        [JsonProperty("lunarYear")]
        public string LunarYear { get; set; }
        [JsonProperty("year-month")]
        public string Year_Month { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
    }
}
