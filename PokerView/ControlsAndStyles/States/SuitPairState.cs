using System;
using System.Collections.Generic;
using System.Text;

namespace ControlsAndStyles
{
    public class SuitPairState
    {
        public Suits LeftSuit;
        public Suits RightSuit;

        private int prob;
        public int Probability
        {
            get => prob;
            set
            {
                if (value >= Probabilities.Min && value <= Probabilities.Max) { prob = value; }
                else { throw new Exception("Prob Error"); }
            }
        }

        public SuitPairState()
        {
            Probability = 0;
        }

        public SuitPairState(Suits first, Suits second)
        {
            LeftSuit = first;
            RightSuit = second;
            prob = 0;
        }

        public SuitPairState(Suits first, Suits second, int prob)
        {
            LeftSuit = first;
            RightSuit = second;
            this.prob = prob;
        }
    }
}
