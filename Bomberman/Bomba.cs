using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman
{
    class Bomba : Sprite
    {
        List<Explosion> explosion;

        public Bomba(int x, int y) : base(x, y)
        {
            explosion = new List<Explosion>();
        }

        public void Explotar()
        {
            //To Do
        }
    }
}
