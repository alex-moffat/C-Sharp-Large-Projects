using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Blackjack
{
    public class BlackjackRules
    {
        //===== PROPERTIES
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>()
        {
            [Face.Two] = 2,
            [Face.Three] = 3,
            [Face.Four] = 4,
            [Face.Five] = 5,
            [Face.Six] = 6,
            [Face.Seven] = 7,
            [Face.Eight] = 8,
            [Face.Nine] = 9,
            [Face.Ten] = 10,
            [Face.Jack] = 10,
            [Face.Queen] = 10,
            [Face.King] = 10,
            [Face.Ace] = 1
        };

        //===== GET ALL HAND VALUES
        private static int[] GetAllHandValues(List<Card> Hand)
        {
            int aceCount = Hand.Count(x => x.Face == Face.Ace);
            int[] result = new int[aceCount + 1];
            int value = Hand.Sum(x => _cardValues[x.Face]);
            result[0] = value;
            if (result.Length == 1) // triggered only if no Aces
            { 
                return result; 
            }
            else
            {
                for (int i = 1; i < result.Length; i++)
                {
                    value += (i * 10);
                    result[i] = value;
                }
                return result;
            }       
        }
        
        //===== CHECK BLACKJACK
        public static bool CheckBlackjack(List<Card> Hand)
        {
            if (Hand.Count == 2) { return (GetAllHandValues(Hand).Max() == 21) ? true : false; }
            return false;
            //---- VERBOSE option
            //if (Hand.Count == 2)
            //{
            //    int[] possibleValues = GetAllHandValues(Hand);
            //    int value = possibleValues.Max();
            //    if (value == 21)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //return false;
        }

        //===== CHECK BUSTED
        public static bool CheckBusted(List<Card> Hand)
        {
            return (GetAllHandValues(Hand).Min() > 21) ? true : false;
        }

        //===== CHECK DEALER STAY
        public static bool CheckDealerStay(List<Card> Hand)
        {
            return (GetAllHandValues(Hand).Any(x => x > 16 && x < 22)) ? true : false;
        }

        //===== CHECK WIN
        public static bool? CheckWin(List<Card> dealerHand, List<Card> playerHand)
        {
            int dealerScore = GetAllHandValues(dealerHand).Where(x => x < 22).Max();
            int playerScore = GetAllHandValues(playerHand).Where(x => x < 22).Max();
            if (playerScore > dealerScore) return true;
            else if (playerScore < dealerScore) return false;
            else return null;
        }
    }
}
