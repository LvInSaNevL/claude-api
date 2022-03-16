using Newtonsoft.Json;

namespace Claude_API
{
    public class BattleNet
    {
        public static Game.Info Lookup(string Id)
        {
            return Game.BattleNetLibrary[Id];
        }
    }
}
