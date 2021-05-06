using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Bomberman
{
    class Bomba : Sprite
    {
        List<Explosion> explosion;
        int longitud;
        double contador;

        public Bomba(int x, int y, int longitud) : base(x, y)
        {
            explosion = new List<Explosion>();
            contador = 0;
            this.longitud = longitud;
        }

        public void Explotar(List<Obstaculo> paredes, List<Obstaculo> muros)
        {
            explosion.Add(new Explosion(X, Y));
            bool colisionan;
            for(int i = 0; i < 4; i++)
            {
                colisionan = false;
                for(int j = 1; j <= longitud && !colisionan; j++)
                {
                    switch(i)
                    {
                        case 0:
                            foreach (Obstaculo p in paredes)
                                if (new Rectangle(X + (j * 40), Y, 40, 40).Intersects(
                                    new Rectangle(p.X, p.Y, 40, 40)))
                                    colisionan = true;

                            if (!colisionan)
                            {
                                foreach (Obstaculo m in muros)
                                    if (new Rectangle(X + (j * 40), Y, 40, 40).Intersects(
                                        new Rectangle(m.X, m.Y, 40, 40)))
                                        colisionan = true;
                                explosion.Add(new Explosion(X + (j * 40), Y));
                            }
                                
                            break;
                        case 1:
                            foreach (Obstaculo p in paredes)
                                if (new Rectangle(X - (j * 40), Y, 40, 40).Intersects(
                                    new Rectangle(p.X, p.Y, 40, 40)))
                                    colisionan = true;

                            if (!colisionan)
                            {
                                foreach (Obstaculo m in muros)
                                    if (new Rectangle(X - (j * 40), Y, 40, 40).Intersects(
                                        new Rectangle(m.X, m.Y, 40, 40)))
                                        colisionan = true;
                                explosion.Add(new Explosion(X - (j * 40), Y));
                            }
                            break;
                        case 2:
                            foreach (Obstaculo p in paredes)
                                if (new Rectangle(X, Y + (j * 40), 40, 40).Intersects(
                                    new Rectangle(p.X, p.Y, 40, 40)))
                                    colisionan = true;

                            if (!colisionan)
                            {
                                foreach (Obstaculo m in muros)
                                    if (new Rectangle(X, Y + (j * 40), 40, 40).Intersects(
                                        new Rectangle(m.X, m.Y, 40, 40)))
                                        colisionan = true;
                                explosion.Add(new Explosion(X, Y + (j * 40)));
                            }
                            break;
                        case 3:
                            foreach (Obstaculo p in paredes)
                                if (new Rectangle(X, Y - (j * 40), 40, 40).Intersects(
                                    new Rectangle(p.X, p.Y, 40, 40)))
                                    colisionan = true;

                            if (!colisionan)
                            {
                                foreach (Obstaculo m in muros)
                                    if (new Rectangle(X, Y - (j * 40), 40, 40).Intersects(
                                        new Rectangle(m.X, m.Y, 40, 40)))
                                        colisionan = true;
                                explosion.Add(new Explosion(X, Y - (j * 40)));
                            }
                            break;
                    }
                }
            }
        }

        public double Contador
        {
            get
            {
                return contador;
            }
            set
            {
                contador = value;
            }
        }

        public List<Explosion> GetExplosion()
        {
            return explosion;
        }
    }
}
