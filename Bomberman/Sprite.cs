﻿using System;
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
        Texture2D imagen;
        Vector2 posicion;

        public Sprite(int x, int y, Texture2D imagen)
        {
            posicion = new Vector2(x, y);
            this.imagen = imagen;
        }

        public void Dibujar(SpriteBatch s)
        {
            s.Draw(imagen, new Rectangle((int)posicion.X, (int)posicion.Y, imagen.Width, imagen.Height), Color.White);
        }

        public float X
        {
            get
            {
                return posicion.X;
            }
            set
            {
                posicion.X = value;
            }
        }

        public float Y
        {
            get
            {
                return posicion.Y;
            }
            set
            {
                posicion.Y = value;
            }
        }

    }
}