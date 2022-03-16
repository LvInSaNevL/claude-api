using System;
using System.Collections.Generic;

namespace Claude_API
{
    public class Game
    {
        public static Dictionary<string, Info> SteamLibrary = new Dictionary<string, Info>();
        public static Dictionary<string, Info> BattleNetLibrary = new Dictionary<string, Info>();

        public struct LilGame
        {
            public string Id { get; set; }
            public string Launcher { get; set; }
        }

        public struct Info
        {
            /// <summary>
            /// The launcher code for the game
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// The "normal" human readable name
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// A short little description of the game
            /// </summary>
            public string About { get; set; }
            /// <summary>
            /// The date the game released, formatted in RFC1123 without timestamp
            /// </summary>
            public string Release { get; set; }
            /// <summary>
            /// The developer of the game
            /// </summary>
            public string Developer { get; set; }
            /// <summary>
            /// The publisher of the game
            /// </summary>
            public string Publisher { get; set; }
            /// <summary>
            /// The launcher claude code for the launcher
            /// Currently supported: "Steam", "BattleNet", "Other"
            /// </summary>
            public string Launcher { get; set; }
            /// <summary>
            /// The URL to the main thumbnail
            /// </summary>
            public string Thumbnail { get; set; }
            /// <summary>
            /// An array of URLs to any promotional images
            /// </summary>
            public List<string> Screenshots { get; set; }
        }
    }
}
