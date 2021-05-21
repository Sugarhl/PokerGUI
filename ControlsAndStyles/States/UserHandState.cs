using System;
using System.Collections.Generic;
using System.Text;

namespace ControlsAndStyles
{
    public class UserHandState
    {
        int leftCard;

        int rightCard;

        List<SuitPairState> pairState = new List<SuitPairState>();

        public int LeftCard { get => leftCard; }
        public int RightCard { get => rightCard; }
        public List<SuitPairState> PairState { get => pairState; set => pairState = value; }

        UserHandState()
        { }

        public UserHandState(string left, string right)
        {
            leftCard = Utilits.NominalToInt[left];
            rightCard = Utilits.NominalToInt[right];
        }

    }
}
