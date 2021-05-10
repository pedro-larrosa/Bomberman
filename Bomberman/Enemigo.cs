using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman
{
    abstract class Enemigo : Sprite
    {
        protected int velocidad;
        protected static Random r;

        public Enemigo(int x, int y, int velocidad) : base(x, y) 
        {
            this.velocidad = velocidad;
        }

        public abstract void Mover();

        public int GetVelocidad()
        {
            return velocidad;
        }
    }
}
