using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace test
{
    public class Messages : ModuleBase
    {

        [Command($"plz cat")]
        public async Task GetCatImg()
        {
            var manager = new HttpController();
            await manager.GetCatImage();
            var mes = $"猫ちゃんです\n"+
                manager.CatImageContents.Url;
            await ReplyAsync(mes);
        }

        [Command($"weather tokyo")]
        public async Task GetWeather()
        {
            var manager = new HttpController();
            await manager.GetWeatherInfo("Tokyo,jp");
            var mes = "現在の東京のお天気をお伝えしますわ\n" +
                $"Weather:{manager.WeatherInfo.Weather[0].Description}\n" +
                $"Region:{manager.WeatherInfo.Name}\n" +
                $"TempMax:{manager.WeatherInfo.JsonMain.TempMax}\n" +
                $"TempMin:{manager.WeatherInfo.JsonMain.TempMin}";
            await ReplyAsync(mes);
        }
    }
}
