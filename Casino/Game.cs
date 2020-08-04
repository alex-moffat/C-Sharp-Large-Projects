using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Casino
{
    public abstract class Game
    {
        //===== CONSTRUCTOR
        public Game()
        {
            Players = new List<Player>();
            Dealer = new Dealer("", 0);
            Bets = new Dictionary<Player, int>();
            LogFile = Directory.GetCurrentDirectory() + @"\logs\log.txt";
        }
        
        //===== PROPERTIES
        public string Name { get; set; }
        public Dealer Dealer { get; set; }
        public List<Player> Players { get; set; }
        public Dictionary<Player, int> Bets { get; set; }
        public string LogFile { get; set; }


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
            using (StreamWriter file = new StreamWriter(game.LogFile, append: true))
            {
                file.WriteLine("{0} Player {1} joined the game. ID = {2}", DateTime.Now , player.Name, player.Id);
            }
            return game;
        }
        public static Game operator -(Game game, Player player)
        {
            game.Players.Remove(player);
            using (StreamWriter file = new StreamWriter(game.LogFile, append: true))
            {
                file.WriteLine("{0} Player {1} left the game. ID = {2}", DateTime.Now, player.Name, player.Id);
            }
            return game;
        }        
    }
}
