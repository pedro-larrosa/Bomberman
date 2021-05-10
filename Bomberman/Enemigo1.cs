using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman
{
    class Enemigo1 : Enemigo
    {
        public Enemigo1(int x, int y) : base(x, y, 100)
        { }

        public override void Mover()
        {
            throw new NotImplementedException();
        }
    }
}
