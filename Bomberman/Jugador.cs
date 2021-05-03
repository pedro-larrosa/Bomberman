﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    class Jugador : Sprite
    {
        Vector2 velocidad;

        public Jugador(int x, int y) : base(x, y)
        {
            velocidad.X = (int)100;
            velocidad.Y = (int)100;
        }
    }
}