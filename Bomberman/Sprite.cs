using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Bomberman
{
    public abstract class Sprite 
    {
        protected Texture2D imagen;
        protected Vector2 posicion;

        public Sprite(int x, int y)
        {
            posicion = new Vector2(x, y);
        }

        public void Dibujar(SpriteBatch s)
        {
            s.Draw(imagen, new Rectangle((int)posicion.X, (int)posicion.Y, 40, 40), Color.White);
        }

        public int X
        {
            get
            {
                return (int)posicion.X;
            }
            set
            {
                posicion.X = value;
            }
        }

        public int Y
        {
            get
            {
                return (int)posicion.Y;
            }
            set
            {
                posicion.Y = value;
            }
        }

        public void SetImagen(Texture2D imagen)
        {
            this.imagen = imagen;
        }
    }
}
