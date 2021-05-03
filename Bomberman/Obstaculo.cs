using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    class Obstaculo : Sprite
    {
        public Obstaculo(int x, int y) : base(x, y) 
        { }

        public Vector2 GetPosicion()
        {
            return posicion;
        }
    }
}
