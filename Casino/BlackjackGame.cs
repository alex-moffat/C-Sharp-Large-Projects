using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Blackjack
{
    public class BlackjackGame : Game, IWalkAway
    {
        //===== CONSTRUCTOR
        public BlackjackGame() : base()
        {
            // using constructor from base Class "Game"
            // creates new instances of Players, Bets
            Dealer = new BlackjackDealer("", 0);
        }

        //===== PROPERTIES
        public BlackjackDealer Dealer { get; set; }

        //===== PLAY
        public override void Play() //override satisfies the requirement for this child to have the parent method
        {
            //----- START set Dealer and declare variables at the beginning of game 
            if (Dealer.Name == "") 
            {
                Dealer = new BlackjackDealer(name: "Doc Holliday", bank: int.MaxValue);
                ListPlayers();
            }
            int bet;
            bool isValid, winner;

            //----- RESET each round
            Dealer.Stay = Dealer.Blackjack = winner = false;
            Dealer.Hand = new List<Card>();
            Dealer.Deck = new Deck(deckCount:Players.Count);
            foreach (Player p in Players)
            {
                p.Hand = new List<Card>();
                p.Stay = false;
            }

            //----- BET for each player
            Console.WriteLine("\n=== Place Bets...");
            foreach (Player p in Players)
            {
                isValid = false;
                while (!isValid)
                {
                    try
                    {
                        Console.WriteLine("{0} your bet:", p.Name);
                        bet = Convert.ToInt32(Console.ReadLine());
                        if (p.Bet(bet)) 
                        { 
                            isValid = true;
                            Bets[p] = bet;
                        }                        
                    }
                    catch (FraudException)
                    {
                        Console.WriteLine("SECURITY! Throw this person out.");
                        Console.ReadLine();
                        return;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Oops...something went wrong:");
                        Console.WriteLine(e.Message);
                    }
                }   
            }

            //----- DEAL START
            Console.WriteLine("\n=== Dealing...");
            for (int i = 0; i < 2; i++)
            {
                Dealer.Deal(Dealer.Hand, Dealer.Name);
                foreach (Player p in Players) Dealer.Deal(p.Hand, p.Name);                
            }
            
            //----- CHECK BLACKJACK - If dealer has Blackjack all players lose except those that also have Blackjack (Draw)
            Dealer.Blackjack = BlackjackRules.CheckBlackjack(Dealer.Hand);
            if (Dealer.Blackjack)
            {
                //--- Dealer Blackjack, all payouts, return
                Console.WriteLine("\n=== Dealer gets Blackjack!");
                foreach (Player p in Players)
                {
                    if (BlackjackRules.CheckBlackjack(p.Hand))
                    {
                        //--- DRAW player blackjack
                        Dealer.Payout(Dealer, p, Bets, condition:"draw");                                                
                    }
                    else
                    {
                        //--- LOSE no blackjack
                        Dealer.Payout(Dealer, p, Bets, condition: "lose");
                    }                    
                }
                return;
            }
            else
            {
                //--- Dealer no Blackjack, check player Blackjack, payout if winner
                winner = Players.Any(p => BlackjackRules.CheckBlackjack(p.Hand));
                if (winner)
                {
                    foreach (Player p in Players)
                    {
                        if (BlackjackRules.CheckBlackjack(p.Hand))
                        {
                            Dealer.Payout(Dealer, p, Bets, condition: "blackjack");
                        }                        
                    }                    
                }
            }

            //----- CONTINUE PLAY
            Console.WriteLine("\n=== Continue Play...");
            foreach (Player p in Players)
            {
                while (!p.Stay)
                {
                    //--- Hand display
                    Console.WriteLine("\nDealer hand:");
                    foreach (Card c in Dealer.Hand) Console.WriteLine("- {0}", c.ToString());
                    Console.WriteLine("\n{0} your cards are:", p.Name);
                    foreach (Card c in p.Hand) Console.WriteLine("- {0}", c.ToString());
                    
                    //--- Hit or Stay
                    Console.WriteLine("\n Hit or Stay?");
                    if (Console.ReadLine().ToLower().Contains("s")) 
                    {
                        p.Stay = true;
                        break;
                    }
                    else
                    {
                        Dealer.Deal(p.Hand, p.Name);
                    }
                    
                    //--- Check Busted
                    if (BlackjackRules.CheckBusted(p.Hand)) Dealer.Payout(Dealer, p, Bets, condition: "bust");                    
                }
            }

            //----- DEALER PLAY
            if (Bets.Count() == 0) return; // check if all players are out
            Dealer.Stay = BlackjackRules.CheckDealerStay(Dealer.Hand);
            while (!Dealer.Stay && !Dealer.Bust)
            {
                Console.WriteLine("\n=== Dealer is hitting...");
                Dealer.Deal(Dealer.Hand, Dealer.Name);
                Dealer.Bust = BlackjackRules.CheckBusted(Dealer.Hand);
                Dealer.Stay = BlackjackRules.CheckDealerStay(Dealer.Hand);
            }
            
            //----- DEALER BUST
            if (Dealer.Bust)
            {
                Console.WriteLine("\n=== Dealer Busted!");
                //--- PAYOUT function - pay only thoses players that still have bets
                foreach (Player p in Players.Where(x=> Bets.ContainsKey(x)))
                {
                    Dealer.Payout(Dealer, p, Bets, condition: "win");
                }
                
                ////--- ALTERNATIVE - Lambda function method
                //foreach (KeyValuePair<Player, int> entry in Bets)
                //{    
                //    Players.Where(p => p == entry.Key).First().Balance += (entry.Value * 2);
                //}                
            }
            //----- DEALER STAY - check only thoses players that still have bets
            else if (Dealer.Stay)
            {
                Console.WriteLine("\n=== Dealer is staying.");
                foreach (Player p in Players.Where(x => Bets.ContainsKey(x))) 
                {
                    switch (BlackjackRules.CheckWin(Dealer.Hand, p.Hand))
                    {
                        case true: //--- player win
                            Dealer.Payout(Dealer, p, Bets, condition: "win");
                            break;
                        case false: //--- dealer win
                            Dealer.Payout(Dealer, p, Bets, condition: "lose");
                            break;
                        default: //--- draw
                            Dealer.Payout(Dealer, p, Bets, condition: "draw");
                            break;                        
                    }
                }                
            }
            return;
        }

        //===== LIST PLAYERS
        public override void ListPlayers()
        {
            Console.WriteLine("===== Welcome To BLACKJACK =====");
            Console.WriteLine("Your dealer is {0}.", Dealer.Name);
            base.ListPlayers(); // this line is the same as all the code within base class method
        }
        //===== WALK AWAY
        public void WalkAway(Player player)
        {
            throw new NotImplementedException();
        }

    }
}
