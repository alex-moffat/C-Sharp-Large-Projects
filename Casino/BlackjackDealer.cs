using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Blackjack
{
    public class BlackjackDealer : Dealer
    {
        //===== CONSTRUCTOR
        public BlackjackDealer(string name, int bank) : base(name, bank)
        {
            // using constructor from base Class "Dealer"
            // sets Hand, Name, Balance            
        }
        
        //===== PROPERTIES
        public List<Card> Hand { get; set; }
        public bool Bust { get; set; }
        public bool Blackjack { get; set; }
        public bool Stay { get; set; }

    }
}
