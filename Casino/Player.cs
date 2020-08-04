using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Player
    {
        //===== CONSTRUCTOR
        public Player(string name, int bank=0)
        {
            Id = Guid.NewGuid();
            Hand = new List<Card>(); //UNNEEDED - reset at the start of each game 
            Name = name;
            ActivelyPlaying = true;
            if (bank <= 0)
            {
                Bank();
            }
            else
            {
                Balance = bank;
            }            
        }
        
        //===== PROPERTIES
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public int Balance { get; set; }
        public bool ActivelyPlaying { get; set; }
        public bool Stay { get; set; }

        //===== BET method
        public bool Bet(int amount)
        {
            if (Balance - amount < 0 && amount > 0)
            {
                Console.WriteLine("Your current balance is {0}, not have enough to place a bet that size.", Balance);
                return false;
            }
            else if (amount < 0)
            {
                throw new FraudException(); // used as example
                // Console.WriteLine("You must place a bet greater than zero.");
                // return false;
            }
            else
            {
                Balance -= amount;
                return true;
            }
        }

        //===== BANK method - return true if player has a positive balance, return false for zero balance
        public bool Bank()
        {
            bool validNum = false;
            while (!validNum)
            {
                Console.WriteLine("How much money did you bring today?");
                validNum = Int32.TryParse(Console.ReadLine(), out int amount);
                if (!validNum)
                {
                    Console.WriteLine("Please enter whole numbers only.");
                }
                else if (amount < 0)
                {
                    Console.WriteLine("Please enter an amount greater than zero.");
                    validNum = false;
                }
                else if (amount == 0)
                {
                    Console.WriteLine("Sorry. You need money to play Blackjack.");
                    Balance = 0;
                    return false;
                }                
                else
                {
                    Balance = amount;
                    return true;
                }
            }
            return true;
        }
    }
}
