using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public abstract class Game
    {
        //===== CONSTRUCTOR
        public Game()
        {
            Players = new List<Player>();
            Dealer = new Dealer("", 0);
            Bets = new Dictionary<Player, int>();
        }
        
        //===== PROPERTIES
        public string Name { get; set; }
        public Dealer Dealer { get; set; }
        public List<Player> Players { get; set; }
        public Dictionary<Player, int> Bets { get; set; }
        

        public abstract void Play(); // Inheriting class must implement the same method with same parameters 
        public virtual void ListPlayers()
        {
            Console.WriteLine("Current Players:");
            foreach (Player p in Players)
            {
                Console.WriteLine(p.Name);
            }
        }
        public static Game operator +(Game game, Player player)
        {
            game.Players.Add(player);
            return game;
        }
        public static Game operator -(Game game, Player player)
        {
            game.Players.Remove(player);
            return game;
        }
    }
}
