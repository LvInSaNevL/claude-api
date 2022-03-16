using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Claude_API
{
    public class Watchdog
    {
        private static readonly Dictionary<string, string> baseURLs = new Dictionary<string, string>()
        {
            { "Steam", "https://store.steampowered.com/api/appdetails?appids=" },
            { "BattleNet", "https://us.shop.battle.net/en-us/product/" }
        };

        public static void Monitor()
        {

        }
             
        public static void ReadDisk()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "data\\games");
            string[] files = Directory.GetFiles(path);

            Game.SteamLibrary.EnsureCapacity(files.Length);
            Game.BattleNetLibrary.EnsureCapacity(files.Length);

            foreach(string file in files)
            { 
                string fileName = Path.GetFileName(file);

                using StreamReader reader = new StreamReader(file);
                string result = reader.ReadToEnd().ToString();
                if (result == null) { }
                else
                {
                    Game.Info parsedData = JsonConvert.DeserializeObject<Game.Info>(result);

                    switch (parsedData.Launcher)
                    {
                        case "Steam":
                            Game.SteamLibrary.Add(parsedData.Id, parsedData);
                            break;
                        case "BattleNet":
                            Game.BattleNetLibrary.Add(parsedData.Id, parsedData);
                            break;
                        default:
                            break;
                    }
                }
            };
        }
    }
}
