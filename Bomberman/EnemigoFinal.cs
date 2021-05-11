using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    class EnemigoFinal : Enemigo
    {
        public EnemigoFinal(int x, int y) : base(x, y, 200)
        { }

        public override void CambiarDireccion()
        {
            throw new NotImplementedException();
        }
    }
}
