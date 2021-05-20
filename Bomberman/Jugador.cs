using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    class Jugador : Sprite
    {
        int velocidad;
        int longitudBomba;

        public Jugador(int x, int y, int longitudBomba) : base(x, y)
        {
            velocidad = 120;
            this.longitudBomba = longitudBomba;
        }

        new public void Dibujar(SpriteBatch s)
        {
            s.Draw(imagen, new Rectangle((int)posicion.X, (int)posicion.Y, 30, 30), Color.White);
        }

        public int GetVelocidad()
        {
            return velocidad;
        }

        public int LongitudBomba
        {
            get
            {
                return longitudBomba;
            }

            set
            {
                longitudBomba = value;
            }
        }
    }
}
