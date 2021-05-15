using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    class EnemigoFinal : Enemigo
    {
        public EnemigoFinal(int x, int y) : base(x, y, 125)
        { }

        public override void CambiarDireccion()
        {
            switch (r.Next(0, 4))
            {
                case 0:
                    velocidad.X = 125;
                    velocidad.Y = 0;
                    break;
                case 1:
                    velocidad.X = 0;
                    velocidad.Y = 125;
                    break;
                case 2:
                    velocidad.X = -125;
                    velocidad.Y = 0;
                    break;
                case 3:
                    velocidad.X = 0;
                    velocidad.Y = -125;
                    break;
            }
        }
    }
}
