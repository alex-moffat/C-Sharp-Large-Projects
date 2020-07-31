using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            //========== GAME
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Clear();
            Console.WriteLine("\n===================================================\n=====( Welcome to the Grand Hotel and Casino )=====\n===================================================\n");
            Game game = new BlackjackGame();

            //========== ADD PLAYERS
            bool addPlayers = true;
            int maxPlayers = 7;
            while (addPlayers && game.Players.Count < maxPlayers)
            {
                Console.WriteLine("\nPlayer, what is your name?");
                string playerName = Console.ReadLine();
                Console.WriteLine("How much money did you bring today?");
                int bank = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Hello {0}. Would you like to join a game of Blackjack right now?", playerName);
                if (Console.ReadLine().ToLower().Contains("y"))
                {
                    Player player = new Player(playerName, bank);
                    game += player;
                    Console.WriteLine("-->{0} added to game.", playerName);                    
                }
                if (game.Players.Count < maxPlayers)
                {
                    Console.WriteLine("\nIs someone else joining today?");
                    if (Console.ReadLine().ToLower().Contains("n")) addPlayers = false;
                }                
            }
            
            //========== PLAY - continue to play rounds until NO players are actively playing or have a balance > 0
            while (game.Players.Count > 0)
            {
                game.Play();
                List<Player> removals = new List<Player>();
                removals = game.Players.Where(x => !x.ActivelyPlaying || x.Balance == 0).ToList();
                foreach (Player player in removals)
                {
                    game -= player;
                }                
            }
            
            //========== GAME OVER
            Console.WriteLine("\n===Thank you for playing.");
            Console.WriteLine("Feel free to look aroung the casino. Bye for now.");

            
            //========== TESTS ==========

            //===== CREATE DECK
            //Deck deck = new Deck();
            
            //===== SHUFFLE DECK
            //deck.Shuffle(times: 4, true);

            //===== lambda functions
            //int count = deck.Cards.Count(x => x.Face == Face.Ace); 
            //Console.WriteLine(count);

            //List<Card> newList = deck.Cards.Where(x => x.Face == Face.Ace).ToList();
            //foreach (Card card in newList) { Console.WriteLine(card.Face); }

            //List<int> numList = new List<int>() { 1,2,3,535,342,23 };
            //int sum = numList.Sum();
            //Console.WriteLine(sum);
            //int sumPlus = numList.Sum(x => x + 1);
            //Console.WriteLine(sumPlus);
            //int max = numList.Max();
            //Console.WriteLine(max);
            //int min = numList.Min();
            //Console.WriteLine(min);
            //int sumBig = numList.Where(x => x > 100).Sum();
            //Console.WriteLine(sumBig);
            //Console.WriteLine(numList.Where(x => x > 100).Sum());

            //===== Overloaded operator
            //Game game = new BlackjackGame() { Name = "BlackJack", Dealer = "Doc Holliday", Players = new List<Player>() };
            //Player p1 = new Player() { Name = "Wyatt Earp" };
            //Player p2 = new Player() { Name = "Jesse James" };
            //game = game + p1 + p2;
            //game.ListPlayers();
            //game = game - p2;
            //game.ListPlayers();

            //deck.ListCards(loop: "foreach");

            //Dealer dealer = new Dealer();
            //dealer.Name = "Alex";
            //Console.WriteLine(dealer.Name); // can get/set public properties when base class is not explicitly public

            //Game game2 = new Game(); // can't create a new object based on an abstract class

            //===== HOLD OPEN - till enter is pressed
            Console.ReadLine();
        }
        
    }
}
