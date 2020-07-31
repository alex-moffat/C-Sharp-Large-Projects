using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Deck
    {
        //========== CONSTRUCTOR
        public Deck(int deckCount=1)
        {
            Cards = new List<Card>();
            //===== DECKS - iterate for a number of decks
            for (int d=0; d < deckCount; d++)
            {
                //===== ENUM - construct a deck of cards using enum datatype
                for (int f = 0; f < Enum.GetNames(typeof(Face)).Length; f++) // using the GetNames() method to build an array to get the length of
                {
                    for (int s = 0; s < Enum.GetNames(typeof(Suit)).Length; s++) // using the GetNames() method to build an array to get the length of
                    {
                        Card card = new Card();
                        card.Face = (Face)f;
                        card.Suit = (Suit)s;
                        Cards.Add(card);
                    }
                }
            }
            Console.WriteLine("Created a deck of {0} cards.", Cards.Count);
            
            //===== SHUFFLE DECK - automatically shuffle deck 4 times when new deck is created
            Shuffle(times: 4, display: false);

            //===== STRING - UNUSED alternative, construct a deck of cards using strings
            //string[] Suits = { "Clubs", "Diamonds", "Hearts", "Spades" };
            //string[] Faces = { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
            //foreach (string f in Faces)
            //{
            //    foreach (string s in Suits)
            //    {
            //        Card card = new Card();
            //        card.Suit = s;
            //        card.Face = f;
            //        Cards.Add(card);
            //    }
            //} 
        }

        //========== PROPERTIES
        Random random = new Random();
        public List<Card> Cards { get; set; }

        //========== SHUFFLE method - input number of time to shuffle, set the default shuffle times to 1 if not provided
        public void Shuffle(int times=1, bool display=false)
        {
            List<Card> tempList;
            int rIndex;
            int sCount = 0;
            for (int i=0; i < times; i++)
            {
                tempList = new List<Card>();
                while (Cards.Count > 0)
                {
                    rIndex = random.Next(0, Cards.Count);
                    tempList.Add(Cards[rIndex]);
                    Cards.RemoveAt(rIndex);
                }
                Cards = tempList;
                if (display) { Console.WriteLine("A shuffle complete. Top card in deck is now {0} of {1}", Cards[0].Face, Cards[0].Suit); }
                sCount++;                
            }
            if (display) { Console.WriteLine((sCount == 1) ? "Shuffled 1 time" : String.Format("Shuffled {0} times.", sCount)); }
        }

        //========== LIST CARDS
        public void ListCards(string loop="for")
        {
            if (loop == "for")
            {
                //===== PRINT LIST OF CARDS
                Console.WriteLine("===== SHOW CARDS - using for loop");
                for (int i = 0; i < Cards.Count; i++)
                {
                    Console.WriteLine(Cards[i].Face + " of " + Cards[i].Suit);
                }
            }
            else
            {
                //===== PRINT LIST OF CARDS
                Console.WriteLine("===== SHOW CARDS - using foreach loop");
                foreach (Card c in Cards)
                {
                    Console.WriteLine(c.Face + " of " + c.Suit);
                }
            }          
        }
    }
}
