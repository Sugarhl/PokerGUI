using ControlsAndStyles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace ControlsAndStyles
{
    public class Request
    {
        [JsonProperty("operationCode")]
        int operationCode { get; set; }

        [JsonProperty("board")]
        List<int> board { get; set; }


        [JsonProperty("playerHand")]
        UserHand playerHand
        {
            get; set;
        }

        [JsonProperty("enemyHand")]
        UserHand enemyHand
        {
            get; set;
        }
        public Request(List<int> table, HashSet<ChoiceButton> playeRange, HashSet<ChoiceButton> enemyRange)
        {
            operationCode = 1;
            board = table;
            playerHand = new UserHand(playeRange);
            enemyHand = new UserHand(enemyRange);
        }

        public override string ToString()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            StringBuilder json = new StringBuilder();
            using (StringWriter sw = new StringWriter(json))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, this);
                }
            }
            return json.ToString();
        }
    }
}
