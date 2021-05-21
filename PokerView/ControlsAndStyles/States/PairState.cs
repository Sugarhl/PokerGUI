using System;
using System.Collections.Generic;
using System.Text;

namespace ControlsAndStyles
{
    public static class PairState
    {
        public static List<SuitPairState> Defult(PairType type)
        {
            List<SuitPairState> pairState = new List<SuitPairState>();
            switch (type)
            {
                case PairType.Different:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (i != j)
                            {
                                pairState.Add(new SuitPairState(Utilits.IntToSuit[i], Utilits.IntToSuit[j], Probabilities.Max));
                            }
                        }
                    }
                    break;
                case PairType.Equal:
                    for (int i = 0; i < 4; i++)
                    {
                        pairState.Add(new SuitPairState(Utilits.IntToSuit[i], Utilits.IntToSuit[i], Probabilities.Max));
                    }
                    break;
                case PairType.Double:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (i != j)
                            {
                                pairState.Add(new SuitPairState(Utilits.IntToSuit[i], Utilits.IntToSuit[j], Probabilities.Max));
                            }
                        }
                    }
                    break;
            }
            return pairState;
        }
    }
}
