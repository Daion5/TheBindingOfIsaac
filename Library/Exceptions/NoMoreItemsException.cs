using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Exceptions
{
    public class NoMoreItemsException : Exception
    {
        public NoMoreItemsException() { }

        public NoMoreItemsException(string message) : base(message) { }

        public NoMoreItemsException(string message, Exception inner) : base(message, inner) { }

        public override string ToString()
        {
            return $"\u001b[31m\u001b[1m{base.ToString()}\u001b[0m";
        }
    }
}
