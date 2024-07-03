using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Magdalene : Character
    {
        public Magdalene() : base("Magdalene", 4, 2, 3, 2, 80, 80, "Yum Heart", 3)
        {
            Bombs = 0;
            Keys = 0;
            Coins = 5;
        }
    }

    public class Tainted_Magdalene : Character
    {
        public Tainted_Magdalene() : base("Tainted_Magdalene", 5, 3, 4, 3, 100, 100, "Yum Heart", 6)
        {
            Bombs = 1;
            Keys = 0;
            Coins = 0;
        }
    }
}