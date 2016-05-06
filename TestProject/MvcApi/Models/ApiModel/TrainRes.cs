using System.Collections.Generic;
using Newtonsoft.Json;

namespace MvcApi.Models
{
    public class TrainRes
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("list")]
        public TrainDetail List { get; set; }
    }

    public class TrainDetail
    {
        [JsonProperty("train_no")]
        public string TrainNo { get; set; }
        [JsonProperty("train_type")]
        public string TrainType { get; set; }
        [JsonProperty("start_station")]
        public string StartStation { get; set; }
        [JsonProperty("start_station_type")]
        public string StartStationType { get; set; }
        [JsonProperty("end_station")]
        public string EndStation { get; set; }   
        [JsonProperty("end_station_type")]
        public string EndStationType { get; set; }
        [JsonProperty("start_time")]
        public string StartTime { get; set; }
        [JsonProperty("end_time")]
        public string EndTime { get; set; }
        [JsonProperty("run_time")]
        public string RunTime { get; set; }
        [JsonProperty("run_distance")]
        public string RunDistance { get; set; }
        [JsonProperty("price_list")]
        public PriceList PriceList { get; set; }
    }

    public class PriceList
    {
        [JsonProperty("item")]
        List<PriceDetail> Item { get; set; }
    }

    public class PriceDetail
    {
        [JsonProperty("price_type")]
        public string PriceType { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}