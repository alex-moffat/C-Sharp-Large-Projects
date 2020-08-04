using System;
using System.Collections.Generic;
using System.Text;

namespace Casino
{
    public class FraudException : Exception
    {
        public FraudException() : base("Security! Kick this person out.")
        { }
        public FraudException(string message) : base(message)
        { }
    }
}
