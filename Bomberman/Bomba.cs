﻿using System;
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
            explosion.Add(new Explosion((int)X, (int)Y));
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
                                if (new Rectangle((int)X + (j * 40), (int)Y, 40, 40).Intersects(
                                    new Rectangle((int)p.X, (int)p.Y, 40, 40)))
                                    colisionan = true;

                            if (!colisionan)
                            {
                                foreach (Obstaculo m in muros)
                                    if (new Rectangle((int)X + (j * 40), (int)Y, 40, 40).Intersects(
                                        new Rectangle((int)m.X, (int)m.Y, 40, 40)))
                                        colisionan = true;
                                explosion.Add(new Explosion((int)X + (j * 40), (int)Y));
                            }
                                
                            break;
                        case 1:
                            foreach (Obstaculo p in paredes)
                                if (new Rectangle((int)X - (j * 40), (int)Y, 40, 40).Intersects(
                                    new Rectangle((int)p.X, (int)p.Y, 40, 40)))
                                    colisionan = true;

                            if (!colisionan)
                            {
                                foreach (Obstaculo m in muros)
                                    if (new Rectangle((int)X - (j * 40), (int)Y, 40, 40).Intersects(
                                        new Rectangle((int)m.X, (int)m.Y, 40, 40)))
                                        colisionan = true;
                                explosion.Add(new Explosion((int)X - (j * 40), (int)Y));
                            }
                            break;
                        case 2:
                            foreach (Obstaculo p in paredes)
                                if (new Rectangle((int)X, (int)Y + (j * 40), 40, 40).Intersects(
                                    new Rectangle((int)p.X, (int)p.Y, 40, 40)))
                                    colisionan = true;

                            if (!colisionan)
                            {
                                foreach (Obstaculo m in muros)
                                    if (new Rectangle((int)X, (int)Y + (j * 40), 40, 40).Intersects(
                                        new Rectangle((int)m.X, (int)m.Y, 40, 40)))
                                        colisionan = true;
                                explosion.Add(new Explosion((int)X, (int)Y + (j * 40)));
                            }
                            break;
                        case 3:
                            foreach (Obstaculo p in paredes)
                                if (new Rectangle((int)X, (int)Y - (j * 40), 40, 40).Intersects(
                                    new Rectangle((int)p.X, (int)p.Y, 40, 40)))
                                    colisionan = true;

                            if (!colisionan)
                            {
                                foreach (Obstaculo m in muros)
                                    if (new Rectangle((int)X, (int)Y - (j * 40), 40, 40).Intersects(
                                        new Rectangle((int)m.X, (int)m.Y, 40, 40)))
                                        colisionan = true;
                                explosion.Add(new Explosion((int)X, (int)Y - (j * 40)));
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
