using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Magdalene : Character
    {
        public Magdalene() : base("Magdalene", 4, 2, 3, 2, 120, 120)
        {
            Bombs = 0;
            Keys = 0;
            Coins = 5;
        }
    }
}
