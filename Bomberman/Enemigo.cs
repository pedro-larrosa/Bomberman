using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    abstract class Enemigo : Sprite
    {
        protected Vector2 velocidad;
        protected static Random r;

        public Enemigo(int x, int y, int velocidad) : base(x, y) 
        {
            r = new Random();
            this.velocidad = r.Next(1, 3) == 1 ? new Vector2(velocidad, 0) : new Vector2(0, velocidad);
        }

        public void Mover(GameTime g)
        {
            X += (int)(velocidad.X * (float)g.ElapsedGameTime.TotalSeconds);
            Y += (int)(velocidad.Y * (float)g.ElapsedGameTime.TotalSeconds);
        }

        public abstract void CambiarDireccion();

        public int GetVelocidadX()
        {
            return (int)velocidad.X;
        }

        public int GetVelocidadY()
        {
            return (int)velocidad.Y;
        }
    }
}
