using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Player
    {
        //===== CONSTRUCTOR
        public Player(string name, int bank)
        {
            Hand = new List<Card>(); //UNNEEDED - reset at the start of each game 
            Name = name;
            Balance = bank;
            ActivelyPlaying = true;
        }
        
        //===== PROPERTIES
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public int Balance { get; set; }
        public bool ActivelyPlaying { get; set; }
        public bool Stay { get; set; }

        //===== BET method
        public bool Bet(int amount)
        {
            if (Balance - amount < 0)
            {
                Console.WriteLine("Your current balance is {0}, not have enough to place a bet that size.", Balance);
                return false;
            }
            else
            {
                Balance -= amount;
                return true;
            }
        }

    }
}
