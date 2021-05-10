using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman
{
    class EnemigoFinal : Enemigo
    {
        public EnemigoFinal(int x, int y) : base(x, y, 200)
        { }

        public override void Mover()
        {
            throw new NotImplementedException();
        }
    }
}
