using System;
using System.Collections.Generic;
using System.Text;

namespace ControlsAndStyles
{
    public static class Utilits
    {
        public const int CardsInSuit = 8;

        public static readonly Dictionary<Suits, int> SuitToInt = new Dictionary<Suits, int>()
        {
            [Suits.Club] = 0,
            [Suits.Diamond] = 1,
            [Suits.Heart] = 2,
            [Suits.Spate] = 3
        };
        public static readonly Dictionary<int, Suits> IntToSuit = new Dictionary<int, Suits>()
        {
            [0] = Suits.Club,
            [1] = Suits.Diamond,
            [2] = Suits.Heart,
            [3] = Suits.Spate
        };


        public static readonly Dictionary<Suits, string> SuitsToString = new Dictionary<Suits, string>
        {
            [Suits.Club] = "♣",
            [Suits.Diamond] = "♦",
            [Suits.Heart] = "♥",
            [Suits.Spate] = "♠"
        }; public static readonly Dictionary<string, Suits> StringToSuits = new Dictionary<string, Suits>
        {
            ["♣"] = Suits.Club,
            ["♦"] = Suits.Diamond,
            ["♥"] = Suits.Heart,
            ["♠"] = Suits.Spate
        };

        public static readonly Dictionary<string, int> NominalToInt = new Dictionary<string, int>
        {
            ["6"] = 0,
            ["7"] = 1,
            ["8"] = 2,
            ["9"] = 3,
            ["T"] = 4,
            ["J"] = 5,
            ["Q"] = 6,
            ["K"] = 7,
            ["A"] = 8
        };
        public static int getCardCode(string card, Suits suit)
        {
            return SuitToInt[suit] * CardsInSuit + NominalToInt[card];
        }
    }
}
