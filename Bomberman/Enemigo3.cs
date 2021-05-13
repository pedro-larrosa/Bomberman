using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    class Enemigo3 : Enemigo
    {
        public Enemigo3(int x, int y) : base(x, y , 100)
        { }

        public override void CambiarDireccion()
        {
            velocidad.X *= -1;
            velocidad.Y *= -1;
        }
    }
}
