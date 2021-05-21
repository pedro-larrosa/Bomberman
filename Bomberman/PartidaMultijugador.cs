using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    class PartidaMultijugador : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Random r;
        bool ganaJugador1;
        bool haTerminado;
        double contador;
        SpriteFont texto;

        Jugador jugador1;
        Jugador jugador2;
        List<Bomba> bombas1;
        List<Bomba> bombas2;
        List<Obstaculo> paredes;
        List<Obstaculo> muros;
        List<Obstaculo> mejoras;

        public PartidaMultijugador()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 520;
            graphics.PreferredBackBufferHeight = 520;
            graphics.ApplyChanges();
        }

        private bool colisiona(int x, int y, int d = 40, bool esjugador1 = true)
        {
            bool colisiona = false;
            for (int i = 0; i < paredes.Count && !colisiona; i++)
            {
                if (new Rectangle(x, y, d, d).Intersects(
                    new Rectangle(paredes[i].X, paredes[i].Y, 40, 40)))
                    colisiona = true;
            }

            if (!colisiona)
            {
                for (int i = 0; i < muros.Count && !colisiona; i++)
                {
                    if (new Rectangle(x, y, d, d).Intersects(
                        new Rectangle(muros[i].X, muros[i].Y, 40, 40)))
                        colisiona = true;
                }
            }

            if (!colisiona)
            {
                if (esjugador1)
                {
                    for (int i = 0; i < bombas2.Count && !colisiona; i++)
                    {
                        if (new Rectangle(x, y, d, d).Intersects(
                            new Rectangle(bombas2[i].X, bombas2[i].Y, 40, 40)))
                            colisiona = true;
                    }
                }
                else
                {
                    for (int i = 0; i < bombas1.Count && !colisiona; i++)
                    {
                        if (new Rectangle(x, y, d, d).Intersects(
                            new Rectangle(bombas1[i].X, bombas1[i].Y, 40, 40)))
                            colisiona = true;
                    }
                }
            }

            return colisiona;
        }

        private void controlBombas(List<Bomba> b, GameTime g)
        {
            for (int i = 0; i < b.Count; i++)
            {
                if ((int)(b[i].Contador += g.ElapsedGameTime.TotalSeconds) == 2 && !b[i].HaExplotado())
                {
                    b[i].Explotar(paredes, muros);
                    foreach (Explosion e in b[i].GetExplosion())
                    {
                        e.SetImagen(Content.Load<Texture2D>("explosion"));
                        //Colisiones de las explosiones con los muros para destruirlos
                        for (int j = 0; j < muros.Count; j++)
                        {
                            if (new Rectangle(muros[j].X, muros[j].Y, 40, 40).Intersects(
                                new Rectangle(e.X, e.Y, 40, 40)))
                                muros.RemoveAt(j);
                        }
                    }
                }

                if (b[i].GetExplosion().Count > 0)
                {
                    foreach (Explosion e in b[i].GetExplosion())
                    {

                        if (new Rectangle(jugador1.X, jugador1.Y, 30, 30).Intersects(
                                new Rectangle(e.X, e.Y, 40, 40)))
                        {
                            ganaJugador1 = false;
                            haTerminado = true;
                        }
                        if (new Rectangle(jugador2.X, jugador2.Y, 30, 30).Intersects(
                                new Rectangle(e.X, e.Y, 40, 40)))
                        {
                            ganaJugador1 = true;
                            haTerminado = true;
                        }
                    }
                }

                if (b[i].Contador >= 4)
                    b.RemoveAt(i);
            }
        }

        protected override void Initialize()
        {
            paredes = new List<Obstaculo>();
            muros = new List<Obstaculo>();
            mejoras = new List<Obstaculo>();
            bombas1 = new List<Bomba>();
            bombas2 = new List<Bomba>();
            haTerminado = false;
            contador = 0;

            try
            {
                StreamReader mapa = new StreamReader("mapaMultijugador.txt");
                string linea;
                int i = 0;
                while((linea = mapa.ReadLine()) != null)
                {
                    for(int j = 0; j < linea.Length; j++)
                    {
                        if (linea[j] == 'X')
                            paredes.Add(new Obstaculo(i * 40, j * 40));
                        if (linea[j] == '1')
                            jugador1 = new Jugador(i * 40, j * 40, 2);
                        if (linea[j] == '2')
                            jugador2 = new Jugador(i * 40, j * 40, 2);
                    }

                    i++;
                }

                mapa.Close();
            }catch(FileNotFoundException e)
            {
                Console.WriteLine("No se ha encontrado el archivo " + e.Message);
                Exit();
            }catch(IOException)
            {
                Exit();
            }

            int numMuros = 0, x, y;
            r = new Random();
            bool coinciden;
            while (numMuros < 80)
            {
                x = r.Next(1, 12) * 40;
                y = r.Next(1, 12) * 40;
                coinciden = false;

                if ((x > jugador1.X * 2 && x < jugador2.X - 40) || (y > jugador1.Y * 2 && y < jugador2.Y - 40))
                {
                    for (int i = 0; i < paredes.Count && !coinciden; i++)
                    {
                        if (paredes[i].X == x && paredes[i].Y == y)
                            coinciden = true;
                    }

                    if (!coinciden)
                    {
                        muros.Add(new Obstaculo(x, y));
                        numMuros++;
                    }
                }
            }

            int idMuro;
            for (int i = 0; i < 3; i++)
            {
                idMuro = r.Next(0, muros.Count);
                if(!mejoras.Contains(new Obstaculo(muros[idMuro].X, muros[idMuro].Y)))
                    mejoras.Add(new Obstaculo(muros[idMuro].X, muros[idMuro].Y));
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texto = Content.Load<SpriteFont>("texto");

            jugador1.SetImagen(Content.Load<Texture2D>("jugador1"));
            jugador2.SetImagen(Content.Load<Texture2D>("jugador2"));

            foreach (Obstaculo p in paredes)
                p.SetImagen(Content.Load<Texture2D>("muroX"));
            foreach (Obstaculo m in muros)
                m.SetImagen(Content.Load<Texture2D>("muro"));
            foreach (Obstaculo m in mejoras)
                m.SetImagen(Content.Load<Texture2D>("mejora"));
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState teclado = Keyboard.GetState();

            if(haTerminado)
            {
                contador += gameTime.ElapsedGameTime.TotalSeconds;
                if (contador >= 5)
                    Exit();
            }

            if (!haTerminado)
            {
                if (teclado.IsKeyDown(Keys.Escape))
                    Exit();


                //Colocar bomba jugador1
                if (teclado.IsKeyDown(Keys.E))
                {
                    Bomba bAux = new Bomba(jugador1.X - (jugador1.X % 40), jugador1.Y - (jugador1.Y % 40), jugador1.LongitudBomba);
                    //He cambiado el .Equals() para que compare solo las posiciones X e Y
                    if (!bombas1.Contains(bAux))
                    {
                        bAux.SetImagen(Content.Load<Texture2D>("bomba"));
                        bombas1.Add(bAux);
                    }
                }

                //Colocar bomba jugador2
                if (teclado.IsKeyDown(Keys.Space))
                {
                    Bomba bAux = new Bomba(jugador2.X - (jugador2.X % 40), jugador2.Y - (jugador2.Y % 40), jugador2.LongitudBomba);
                    //He cambiado el .Equals() para que compare solo las posiciones X e Y
                    if (!bombas2.Contains(bAux))
                    {
                        bAux.SetImagen(Content.Load<Texture2D>("bomba2"));
                        bombas2.Add(bAux);
                    }
                }


                //Jugador 1
                jugador1.SetImagen(Content.Load<Texture2D>("jugador1"));
                if (teclado.IsKeyDown(Keys.W))
                {
                    jugador1.Y -= (int)(jugador1.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador1.SetImagen(Content.Load<Texture2D>("jugador1U"));
                }
                if (teclado.IsKeyDown(Keys.A))
                {
                    jugador1.X -= (int)(jugador1.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador1.SetImagen(Content.Load<Texture2D>("jugador1L"));
                }
                if (teclado.IsKeyDown(Keys.S))
                {
                    jugador1.Y += (int)(jugador1.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador1.SetImagen(Content.Load<Texture2D>("jugador1D"));
                }
                if (teclado.IsKeyDown(Keys.D))
                {
                    jugador1.X += (int)(jugador1.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador1.SetImagen(Content.Load<Texture2D>("jugador1R"));
                }

                //Jugador 2
                jugador2.SetImagen(Content.Load<Texture2D>("jugador2"));
                if (teclado.IsKeyDown(Keys.Up))
                {
                    jugador2.Y -= (int)(jugador2.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador2.SetImagen(Content.Load<Texture2D>("jugador2U"));
                }
                if (teclado.IsKeyDown(Keys.Left))
                {
                    jugador2.X -= (int)(jugador2.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador2.SetImagen(Content.Load<Texture2D>("jugador2L"));
                }
                if (teclado.IsKeyDown(Keys.Down))
                {
                    jugador2.Y += (int)(jugador2.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador2.SetImagen(Content.Load<Texture2D>("jugador2D"));
                }
                if (teclado.IsKeyDown(Keys.Right))
                {
                    jugador2.X += (int)(jugador2.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador2.SetImagen(Content.Load<Texture2D>("jugador2R"));
                }


                //Bombas1
                controlBombas(bombas1, gameTime);

                //Bombas2
                controlBombas(bombas2, gameTime);



                //Colisiones jugador1
                if (colisiona(jugador1.X, jugador1.Y, 30))
                {
                    if (teclado.IsKeyDown(Keys.W))
                        jugador1.Y += (int)(jugador1.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (teclado.IsKeyDown(Keys.A))
                        jugador1.X += (int)(jugador1.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (teclado.IsKeyDown(Keys.S))
                        jugador1.Y -= (int)(jugador1.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (teclado.IsKeyDown(Keys.D))
                        jugador1.X -= (int)(jugador1.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                //Colisiones jugador2
                if (colisiona(jugador2.X, jugador2.Y, 30, false))
                {
                    if (teclado.IsKeyDown(Keys.Up))
                        jugador2.Y += (int)(jugador2.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (teclado.IsKeyDown(Keys.Left))
                        jugador2.X += (int)(jugador2.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (teclado.IsKeyDown(Keys.Down))
                        jugador2.Y -= (int)(jugador2.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (teclado.IsKeyDown(Keys.Right))
                        jugador2.X -= (int)(jugador2.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }


                for (int i = 0; i < mejoras.Count; i++)
                {
                    //mejora con jugador1
                    if (new Rectangle(mejoras[i].X + 10, mejoras[i].Y + 10, 20, 20).Intersects(
                        new Rectangle(jugador1.X, jugador1.Y, 30, 30)))
                    {
                        jugador1.LongitudBomba += 1;
                        mejoras.RemoveAt(i);
                    }

                    //mejora con jugador2
                    if (new Rectangle(mejoras[i].X + 10, mejoras[i].Y + 10, 20, 20).Intersects(
                        new Rectangle(jugador2.X, jugador2.Y, 30, 30)))
                    {
                        jugador2.LongitudBomba += 1;
                        mejoras.RemoveAt(i);
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if(!haTerminado)
                GraphicsDevice.Clear(Color.Green);
            else
                GraphicsDevice.Clear(Color.Black);


            spriteBatch.Begin();
            if (!haTerminado)
            {
                //jugadores
                jugador1.Dibujar(spriteBatch);
                jugador2.Dibujar(spriteBatch);
                //mapa
                foreach (Obstaculo m in mejoras)
                    m.Dibujar(spriteBatch);
                foreach (Obstaculo p in paredes)
                    p.Dibujar(spriteBatch);
                foreach (Obstaculo m in muros)
                    m.Dibujar(spriteBatch);

                //Bombas
                foreach (Bomba b in bombas1)
                {
                    b.Dibujar(spriteBatch);
                    foreach (Explosion e in b.GetExplosion())
                        e.Dibujar(spriteBatch);
                }
                foreach (Bomba b in bombas2)
                {
                    b.Dibujar(spriteBatch);
                    foreach (Explosion e in b.GetExplosion())
                        e.Dibujar(spriteBatch);
                }
            }else
                spriteBatch.DrawString(texto, "EL GANADOR ES EL JUGADOR " + (ganaJugador1 ? 1 : 2) + "!", new Vector2(520 / 2 - 150, 520 / 2 - 20), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
