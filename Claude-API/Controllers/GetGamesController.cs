using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Claude_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetGamesController : ControllerBase
    {
        private readonly ILogger<GetGamesController> _logger;

        public GetGamesController(ILogger<GetGamesController> logger)
        {
            _logger = logger;
        }

        [HttpPost, ActionName("GetInfo")]
        public async Task<string> GetGamesAsync([FromBody] object input)
        {
            List<Game.LilGame> inputGame = JsonConvert.DeserializeObject<List<Game.LilGame>>(input.ToString());
            string returnVal = LookupController(inputGame).Result;
            return returnVal;
        }

        private static async Task<string> LookupController(List<Game.LilGame> inputGames)
        {
            List<Game.Info> games = new List<Game.Info>();

            Parallel.ForEach(inputGames, game =>
            {
                switch (game.Launcher)
                {
                    case "Steam":
                        lock (games) { games.Add(Steam.Lookup(game.Id)); }
                        break;
                    case "BattleNet":
                        lock (games) { games.Add(BattleNet.Lookup(game.Id)); }
                        break;
                    default:
                        break;
                }
            });

            return JsonConvert.SerializeObject(games);
        }
    }
}
