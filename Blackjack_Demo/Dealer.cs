using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Dealer
    {
        //===== CONSTRUCTOR
        public Dealer(string name, int bank)
        {
            Name = name;
            Balance = bank;
        }

        //===== PROPERTIES
        public Deck Deck { get; set; }        
        public string Name { get; set; }
        public int Balance { get; set; }

        //===== DEAL
        public void Deal(List<Card> Hand, string name="")
        {
            // Hand.Add(Deck.Cards.First()); -could also use this
            Hand.Add(Deck.Cards[0]); 
            Console.WriteLine("{0}: {1}", name, Deck.Cards.First().ToString());
            Deck.Cards.RemoveAt(0);
        }

        //===== PAYOUT
        public void Payout(Dealer dealer, Player player, Dictionary<Player, int> bets, string condition="draw")
        {
            switch (condition)
            {
                case "blackjack":
                    int payOut = Convert.ToInt32(bets[player] * 1.5);
                    player.Balance += payOut + bets[player];
                    dealer.Balance -= payOut;
                    Console.WriteLine("Blackjack! {0} wins {1}. Your balance is now {2}.", player.Name, payOut, player.Balance);
                    break;
                case "bust":
                    dealer.Balance += bets[player];
                    Console.WriteLine("{0} Busted! You lose your bet of {1}. Your balance is now {2}.", player.Name, bets[player], player.Balance);
                    break;
                case "win":
                    player.Balance += bets[player] * 2;
                    dealer.Balance -= bets[player];
                    Console.WriteLine("{0} wins {1}. Your balance is now {2}.", player.Name, bets[player], player.Balance);
                    break;
                case "lose":
                    dealer.Balance += bets[player];
                    Console.WriteLine("{0} you lose your bet of {1}. Your balance is now {2}.", player.Name, bets[player], player.Balance);
                    break;
                case "draw":
                    player.Balance += bets[player];
                    Console.WriteLine("Push! {0} gets {1} back. Your balance is now {2}.", player.Name, bets[player], player.Balance);
                    break;
            }         
            bets.Remove(player);
            player.Stay = true;
            if (player.Balance > 0)
            {
                Console.WriteLine("{0} do you want to play again?", player.Name);
                if (!Console.ReadLine().ToLower().Contains("y")) player.ActivelyPlaying = false;
            }
            
        }
    }
}
