using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Judas : Character
    {
        public Judas() : base("Judas", 5, 3, 4, 3, 20, 20, "The Book Of Belial", 10)
        {
            Bombs = 1;
            Keys = 0;
            Coins = 0;
        }
    }

    public class Tainted_Judas : Character
    {
        public Tainted_Judas() : base("Tainted_Judas", 6, 4, 5, 4, 40, 40, "The Book Of Belial", 20)
        {
            Bombs = 0;
            Keys = 1;
            Coins = 0;
        }
    }
}
