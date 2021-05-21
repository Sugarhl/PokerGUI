using ControlsAndStyles;
using Newtonsoft.Json;
using System.Collections.Generic;
using static ControlsAndStyles.Utilits;
namespace ControlsAndStyles
{
    public class UserHand
    {
        [JsonProperty("firstCards")]
        List<int> FirstCards { get; set; }

        [JsonProperty("secondCards")]
        List<int> SecondCards { get; set; }

        [JsonProperty("probabilities")]
        List<int> Probabilities { get; set; }

        public UserHand(HashSet<ChoiceButton> playeRange)
        {
            FirstCards = new List<int>(playeRange.Count);
            SecondCards = new List<int>(playeRange.Count);
            Probabilities = new List<int>(playeRange.Count);

            foreach (var pair in playeRange)
            {
                foreach (var suitPair in pair.HandState)
                {
                    FirstCards.Add(getCardCode(pair.LeftCard, suitPair.LeftSuit));
                    SecondCards.Add(getCardCode(pair.RightCard, suitPair.RightSuit));
                    Probabilities.Add(suitPair.Probability);
                }
            }
        }
    }
}
