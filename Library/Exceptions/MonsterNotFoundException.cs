using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Exceptions
{
    public class MonsterNotFoundException : Exception
    {
        public MonsterNotFoundException() { }

        public MonsterNotFoundException(string message) : base(message) { }

        public MonsterNotFoundException(string message, Exception inner) : base(message, inner) { }

        public override string ToString()
        {
            return $"\u001b[31m\u001b[1m{base.ToString()}\u001b[0m";
        }
    }
}
