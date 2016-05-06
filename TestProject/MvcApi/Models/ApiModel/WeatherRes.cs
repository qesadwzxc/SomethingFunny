using Newtonsoft.Json;
using System.Collections.Generic;

namespace MvcApi.Models
{
    public class WeatherRes
    {
            [JsonProperty("data")]
            public WeatherContent Data { get; set; }
    }

    public class WeatherContent
    {
        [JsonProperty("realtime")]
        public WeatherRealTime Realtime { get; set; }

        [JsonProperty("life")]
        public LifeAbout Life { get; set; }

        [JsonProperty("weather")]
        public List<Weather> WeatherSevenDays { get; set; }

        [JsonProperty("pm25")]
        public PM25 PM25 { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("isForeign")]
        public int IsForeign { get; set; }
    }

    public class WeatherRealTime
    {
        [JsonProperty("wind")]
        public WindDetail Wind { get; set; }
        [JsonProperty("time")]
        public string Time { get; set; }
        [JsonProperty("weather")]
        public WeatherDetail Weather { get; set; }
        [JsonProperty("dataUptime")]
        public string DataUpTime { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("city_code")]
        public string CityCode { get; set; }
        [JsonProperty("city_name")]
        public string CityCame { get; set; }
        [JsonProperty("week")]
        public int Week { get; set; }
        [JsonProperty("moon")]
        public string Moon { get; set; }
    }

    public class WindDetail
    {
        [JsonProperty("windspeed")]
        public string Windspeed { get; set; }
        [JsonProperty("direct")]
        public string Direct { get; set; }
        [JsonProperty("power")]
        public string Power { get; set; }
        [JsonProperty("offset")]
        public string Offset { get; set; }
    }

    public class WeatherDetail
    {
        [JsonProperty("humidity")]
        public string Humidity { get; set; }
        [JsonProperty("img")]
        public string Img { get; set; }
        [JsonProperty("info")]
        public string Info { get; set; }
        [JsonProperty("temperature")]
        public string Temperature { get; set; }
    }

    public class LifeAbout
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("info")]
        public LifeType Info { get; set; }
    }

    public class LifeType
    {
        [JsonProperty("kongtiao")]
        public string[] KongTiao { get; set; }
        [JsonProperty("yundong")]
        public string[] YunDong { get; set; }
        [JsonProperty("ziwaixian")]
        public string[] ZiWaiXian { get; set; }
        [JsonProperty("ganmao")]
        public string[] GanMao { get; set; }
        [JsonProperty("xiche")]
        public string[] XiChe { get; set; }
        [JsonProperty("wuran")]
        public string[] WuRan { get; set; }
        [JsonProperty("chuanyi")]
        public string[] ChuanYi { get; set; }
    }

    public class Weather
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("info")]
        public WeatherInfo Info { get; set; }

        [JsonProperty("week")]
        public string Week { get; set; }

        [JsonProperty("nongli")]
        public string NongLi { get; set; }
    }

    public class WeatherInfo
    {
        [JsonProperty("night")]
        public string[] Night { get; set; }

        [JsonProperty("day")]
        public string[] Day { get; set; }
    }

    public class PM25
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("show_desc")]
        public int ShowDesc { get; set; }

        [JsonProperty("pm25")]
        public PMDetail PMDetail { get; set; }

        [JsonProperty("dateTime")]
        public string Date { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }
    }

    public class PMDetail
    {
        [JsonProperty("curPm")]
        public string CurrentPM { get; set; }

        [JsonProperty("pm25")]
        public string PM25 { get; set; }

        [JsonProperty("pm10")]
        public string PM10 { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("quality")]
        public string Quality { get; set; }

        [JsonProperty("des")]
        public string Description { get; set; }
    }
}