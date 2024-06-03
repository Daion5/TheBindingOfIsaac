using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Isaac : Character
    {
        public Isaac() : base("Isaac", 3, 2, 4, 4, 70, 70)
        {
            Bombs = 0;
            Keys = 1;
            Coins = 0;
        }

    }
}