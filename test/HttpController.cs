using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Configuration;

namespace test
{
    internal class HttpController
    {
        
        internal CatImage CatImageContents { get; set; } = new CatImage();

        internal async Task GetCatImage()
        {
            using var client = new HttpClient();
            const string caturl = "https://api.thecatapi.com/v1/images/search";
            var response = await client.GetAsync(caturl); // GET
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                JsonDocument document = JsonDocument.Parse(jsonString);
                var rootToList = document.RootElement.EnumerateArray().ToList();

                CatImageContents.Id = rootToList[0].GetProperty("id").GetString();
                CatImageContents.Url = rootToList[0].GetProperty("url").GetString();
                CatImageContents.Width = rootToList[0].GetProperty("width").GetInt32();
                CatImageContents.Height=rootToList[0].GetProperty("height").GetInt32();

                Console.WriteLine(CatImageContents.Url);
                //CatImageContents = JsonConvert.DeserializeObject<CatImage>(jsonString);

            }
        }

        internal Temperatures WeatherInfo { get; set; } = new Temperatures();
        internal async Task GetWeatherInfo(string region) 
        {
            using (var client = new HttpClient())
            {
                var url = $"http://api.openweathermap.org/data/2.5/weather?q={region}&units=metric&lang=ja&APPID={ConfigurationManager.AppSettings["apikey"]}";
                var result = await client.GetAsync(url);

                // レスポンスのステータスが成功時
                if (result.IsSuccessStatusCode)
                {
                    // JSONを文字列に変換
                    var jsonString = await result.Content.ReadAsStringAsync();
                    // 文字列のJSONをデシリアライズ
                    var res = JsonConvert.DeserializeObject<Temperatures>(jsonString);
                    WeatherInfo = res;
                    // 取得結果を表示する
                    Console.WriteLine($"Weather:{res.Weather[0].Description}");
                    Console.WriteLine($"Name:{res.Name}");
                    Console.WriteLine($"TempMax:{res.JsonMain.TempMax}");
                    Console.WriteLine($"TempMin:{res.JsonMain.TempMin}");
                }
            }
        
        
        }



        string FormatResponse(string response) => response.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "");
    }

    public  class CatImage
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("url")]
        public string? Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
    }

    #region weather
    public partial class Temperatures
    {
        [JsonProperty("coord")]
        public Coord Coord { get; set; }

        [JsonProperty("weather")]
        public List<Weather> Weather { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("main")]
        public JsonMain JsonMain { get; set; }

        [JsonProperty("visibility")]
        public long Visibility { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }

        [JsonProperty("dt")]
        public long Dt { get; set; }

        [JsonProperty("sys")]
        public Sys Sys { get; set; }

        [JsonProperty("timezone")]
        public long Timezone { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cod")]
        public long Cod { get; set; }
    }

    public partial class Clouds
    {
        [JsonProperty("all")]
        public long All { get; set; }
    }

    public partial class Coord
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }

    public partial class JsonMain
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public long TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }

        [JsonProperty("pressure")]
        public long Pressure { get; set; }

        [JsonProperty("humidity")]
        public long Humidity { get; set; }
    }

    public partial class Sys
    {
        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("sunrise")]
        public long Sunrise { get; set; }

        [JsonProperty("sunset")]
        public long Sunset { get; set; }
    }

    public partial class Weather
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("main")]
        public string JsonMain { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    public partial class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public long Deg { get; set; }
    }
#endregion weatehr
}
