using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Judas : Character
    {
        public Judas() : base("Judas", 4, 3, 4, 2, 40, 40)
        {
            Bombs = 1;
            Keys = 0;
            Coins = 0;
        }
    }
}
