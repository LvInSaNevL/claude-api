using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;

namespace Claude_API
{
    public class Steam
    {
        public static Game.Info Lookup(string Id)
        {
            try { return Game.SteamLibrary[Id]; }
            catch (System.Collections.Generic.KeyNotFoundException) { return EmergencyDownload(Id); }
        }

        public static Game.Info EmergencyDownload(string target)
        {
            string response = new WebClient().DownloadString($"https://store.steampowered.com/api/appdetails?appids={target}");
            dynamic gameData = JObject.Parse(response)[target]["data"];

            Game.Info nowGame = new Game.Info();
            try
            {
                nowGame.Id = gameData["steam_appid"];
                nowGame.Title = gameData["name"];
                nowGame.About = gameData["about_the_game"];
                nowGame.Release = gameData["release_date"]["date"];
                nowGame.Developer = gameData["developers"][0];
                nowGame.Publisher = gameData["publishers"][0];
                nowGame.Launcher = "Steam";
                nowGame.Thumbnail = $"https://cdn.akamai.steamstatic.com/steam/apps/{gameData["steam_appid"]}/header.jpg";

                List<string> screens = new List<string>();
                foreach (var info in gameData["screenshots"]) { screens.Add(info.Full); }
                nowGame.Screenshots = screens;

                lock (Game.SteamLibrary) { Game.SteamLibrary.Add(nowGame.Id, nowGame); }
            }
            catch (System.Exception e)
            {
                nowGame.Id = target;
                nowGame.Title = "null";
            }

            return nowGame;
        }
    }
}
