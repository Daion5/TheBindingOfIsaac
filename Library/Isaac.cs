using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Isaac : Character
    {
        public Isaac() : base("Isaac", 3, 2, 4, 4, 60, 60, "Wooden Nickel", 2)
        {
            Bombs = 0;
            Keys = 1;
            Coins = 0;
        }
    }

    public class Tainted_Isaac : Character
    {
        public Tainted_Isaac() : base("Tainted_Isaac", 4, 3, 5, 5, 80, 80, "Wooden Nickel", 4)
        {
            Bombs = 1;
            Keys = 0;
            Coins = 0;
        }
    }
}
