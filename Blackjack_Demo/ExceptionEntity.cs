using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class ExceptionEntity
    {
        //===== CONSTRUCTOR
        public ExceptionEntity()
        {
        }
        public ExceptionEntity(int id, string type, string msg, DateTime time)
        {
            Id = id;
            ExceptionType = type;
            ExceptionMessage = msg;
            TimeStamp = time;
        }

        //===== PROPERTIES
        public int Id { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
