using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ControlsAndStyles
{
    public class Response
    {
        public static bool ParseResponse(string json, out Response response)
        {
            try
            {
                response = JsonConvert.DeserializeObject<Response>(json);
                return true;
            }
            catch
            {
                response = null;
                return false;
            }
        }
        public Response(int playerWinrate, int enemyWinrate)
        {
            PlayerWinrate = playerWinrate;
            EnemyWinrate = enemyWinrate;
        }

        [JsonProperty("PlayerOdds")]
        public int PlayerWinrate { get; }
        [JsonProperty("EnemyOdds")]
        public int EnemyWinrate { get; }
    }
}
