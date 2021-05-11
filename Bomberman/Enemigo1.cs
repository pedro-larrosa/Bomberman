using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    class Enemigo1 : Enemigo
    {
        public Enemigo1(int x, int y) : base(x, y, 100)
        { }

        public override void CambiarDireccion()
        {
            switch(r.Next(0, 4))
            {
                case 0:
                    velocidad.X = 100;
                    velocidad.Y = 0;
                    break;
                case 1:
                    velocidad.X = 0;
                    velocidad.Y = 100;
                    break;
                case 2:
                    velocidad.X = -100;
                    velocidad.Y = 0;
                    break;
                case 3:
                    velocidad.X = 0;
                    velocidad.Y = -100;
                    break;
            }
        }
    }
}
