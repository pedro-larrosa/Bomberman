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
            this.velocidad = new Vector2(velocidad);
            r = new Random();
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
