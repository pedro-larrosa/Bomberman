using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Bomberman
{
    class Partida : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        SpriteFont texto;
        Texture2D fondoGris;
        Song musicaFondo;
        static Random r;
        int nivel;
        int puntuacion;
        double tiempo;
        bool muerto;
        bool final;
        bool pausado;
        bool pulsada;

        
        Jugador jugador;
        List<Obstaculo> paredes;
        List<Obstaculo> muros;
        Obstaculo salida;
        Obstaculo mejora;
        List<Bomba> bombas;
        List<Enemigo> enemigos;

        public Partida(int nivel, int longitudBomba, int puntuacion)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            graphics.PreferredBackBufferWidth = 920;
            graphics.PreferredBackBufferHeight = 560;
            graphics.ApplyChanges();

            this.nivel = nivel;
            //Inicializamos el jugador
            jugador = new Jugador(40, 80, longitudBomba);
            this.puntuacion = puntuacion;
        }

        public bool JugadorMuerto()
        {
            return muerto;
        }

        public int GetLongitudBomba()
        {
            return jugador.LongitudBomba;
        }

        public int GetPuntuacion()
        {
            return puntuacion;
        }

        public bool GetPausado()
        {
            return pausado;
        }

        private void generarMapa()
        {
            //Se generan paredes
            paredes = new List<Obstaculo>();
            try
            {
                StreamReader mapa = new StreamReader("mapa.txt");
                string linea;
                int i = 0;

                while((linea = mapa.ReadLine()) != null)
                {
                    for (int j = 0; j < linea.Length; j++)
                        if (linea[j] == 'X')
                            paredes.Add(new Obstaculo(i * 40, (j + 1) * 40));

                    i++;
                }

                mapa.Close();
            }catch(FileNotFoundException e)
            {
                Console.WriteLine("No se ha encontrado el archivo " + e.FileName + " y no se puede generar un mapa");
            }catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }

            //se generan muros
            muros = new List<Obstaculo>();
            int numMuros = 0, x, y;
            r = new Random();
            bool coinciden;
            while (numMuros < Math.Min(80, 35 * nivel))
            {
                x = r.Next(1, 22) * 40;
                y = r.Next(2, 13) * 40;
                coinciden = false;

                if (x > 160 || y > 160)
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

            //Se genera la puerta de salida
            bool generado = false;
            while (!generado)
            {
                for (int i = 0; i < muros.Count && !generado; i++)
                {
                    if (r.Next(1, 6) == 3)
                    {
                        salida = new Obstaculo(muros[i].X, muros[i].Y);
                        generado = true;
                    }
                }
            }

            //Se genera la mejora de las bombas(o no, es una posibilidad)
            generado = false;
            for (int i = 0; i < muros.Count && !generado; i++)
            {
                if (r.Next(1, 51 - nivel) == 3)
                {
                    if (muros[i].X != salida.X && muros[i].Y != salida.Y)
                    {
                        mejora = new Obstaculo(muros[i].X, muros[i].Y);
                        generado = true;
                    }
                }   
            }
        }

        private void generarEnemigos()
        {
            enemigos = new List<Enemigo>();
            int x, y, i = 0;
            r = new Random();

            //Se generan los enemigos de tipo 1. de los que solo habrán en el primer nivel
            while(i < Math.Min(5, 3 * nivel))
            {
                x = r.Next(1, 22) * 40;
                y = r.Next(2, 13) * 40;

                if(x > 160 || y > 160)
                {
                    if (!colisiona(x, y))
                    {
                        enemigos.Add(new Enemigo1(x, y));
                        i++;
                    }
                }
            }

            //Se generan los enemigos de tipo 2. Los cuales apareceran a partir del segundo nivel
            if(nivel > 1)
            {
                i = 0;
                while (i < 2)
                {
                    x = r.Next(1, 22) * 40;
                    y = r.Next(2, 13) * 40;

                    if (x > 160 || y > 160)
                    {
                        if (!colisiona(x, y))
                        {
                            enemigos.Add(new Enemigo2(x, y));
                            i++;
                        }
                    }
                }
            }

            //Se generan los enemigos de tipo 3. Que apareceran en el 4º nivel
            i = 0;
            while(i < Math.Max(0, 1 * (nivel - 3)))
            {
                x = r.Next(1, 22) * 40;
                y = r.Next(2, 13) * 40;

                if(x > 160 || y > 160)
                {
                    if (!colisiona(x, y))
                    {
                        enemigos.Add(new Enemigo3(x, y));
                        i++;
                    }
                }
            }
        }

        private void enemigosFinales()
        {
            final = true;
            int i, x, y;
            for (i = 0; i < enemigos.Count; i++)
            {
                enemigos[i] = new EnemigoFinal(enemigos[i].X, enemigos[i].Y);
            }

            i = 0;
            while (i < 5)
            {
                x = r.Next(1, 22) * 40;
                y = r.Next(2, 13) * 40;

                if (x > jugador.X * 2 || y > jugador.Y * 2)
                {
                    if (!colisiona(x, y))
                    {
                        enemigos.Add(new EnemigoFinal(x, y));
                        i++;
                    }
                }
            }

            foreach (Enemigo e in enemigos)
                e.SetImagen(Content.Load<Texture2D>("enemigoFinal"));
        }

        private bool colisiona(int x, int y, int d = 40, bool esEnemigo = false)
        {
            bool colisiona = false;
            for(int i = 0; i < paredes.Count && !colisiona; i++)
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

            if (!colisiona && esEnemigo)
            {
                for(int i = 0; i < bombas.Count && !colisiona; i++)
                {
                    if (new Rectangle(x, y, d, d).Intersects(
                        new Rectangle(bombas[i].X, bombas[i].Y, 40, 40)))
                        colisiona = true;
                }
            }

            if (!colisiona && !esEnemigo && d != 30)
            {
                for (int i = 0; i < enemigos.Count && !colisiona; i++)
                {
                    if (new Rectangle(x, y, d, d).Intersects(
                        new Rectangle(enemigos[i].X, enemigos[i].Y, 40, 40)))
                    {
                        colisiona = true;
                    }
                }
            }

            return colisiona;
        }

        protected override void Initialize()
        {
            //Inicializamos bombas
            bombas = new List<Bomba>();

            //se genera el mapa
            generarMapa();

            //Se generan los enemigos
            generarEnemigos();

            //Se inicializa el contador
            tiempo = 201;
            muerto = true;
            final = false;
            pausado = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //se carga la imagen del jugador
            jugador.SetImagen(Content.Load<Texture2D>("jugador1"));
            //se cargan las imagenes de las paredes
            foreach (Obstaculo p in paredes)
                p.SetImagen(Content.Load<Texture2D>("muroX"));
            //se cargan las imagenes de los muros
            foreach (Obstaculo m in muros)
                m.SetImagen(Content.Load<Texture2D>("muro"));
            //Se carga la puerta y la mejora si la hubiera
            salida.SetImagen(Content.Load<Texture2D>("puerta"));
            if (mejora != null)
                mejora.SetImagen(Content.Load<Texture2D>("mejora"));
            //se cargan las imagenes de los enemigos
            foreach (Enemigo e in enemigos)
            {
                if (e.GetType() == typeof(Enemigo1))
                    e.SetImagen(Content.Load<Texture2D>("enemigo1"));
                else if (e.GetType() == typeof(Enemigo2))
                    e.SetImagen(Content.Load<Texture2D>("enemigo2"));
                else
                    e.SetImagen(Content.Load<Texture2D>("enemigo3"));
            }

            texto = Content.Load<SpriteFont>("texto");
            fondoGris = Content.Load<Texture2D>("gris");
            musicaFondo = Content.Load<Song>("musica");
            MediaPlayer.Play(musicaFondo);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState teclado = Keyboard.GetState();

            //Pausa
            if (teclado.IsKeyDown(Keys.P) && !pulsada)
                pausado = pausado ? false : true;
            pulsada = teclado.IsKeyDown(Keys.P) ? true : false;
            
            if (pausado)
                if (teclado.IsKeyDown(Keys.Escape))
                    Exit();


            if (!pausado)
            {
                //Colocar bomba
                if (teclado.IsKeyDown(Keys.E))
                {
                    Bomba bAux = new Bomba(jugador.X - (jugador.X % 40), jugador.Y - (jugador.Y % 40), jugador.LongitudBomba);
                    //He cambiado el .Equals() para que compare solo las posiciones X e Y
                    if (!bombas.Contains(bAux))
                    {
                        bAux.SetImagen(Content.Load<Texture2D>("bomba"));
                        bombas.Add(bAux);
                    }
                }


                //Movimiento jugador
                jugador.SetImagen(Content.Load<Texture2D>("jugador1"));
                if (teclado.IsKeyDown(Keys.W))
                {
                    jugador.Y -= (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador.SetImagen(Content.Load<Texture2D>("jugador1U"));
                }
                if (teclado.IsKeyDown(Keys.A))
                {
                    jugador.X -= (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador.SetImagen(Content.Load<Texture2D>("jugador1L"));
                }
                if (teclado.IsKeyDown(Keys.S))
                {
                    jugador.Y += (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador.SetImagen(Content.Load<Texture2D>("jugador1D"));
                }
                if (teclado.IsKeyDown(Keys.D))
                {
                    jugador.X += (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    jugador.SetImagen(Content.Load<Texture2D>("jugador1R"));
                }

                //Se mueven los enemigos
                foreach (Enemigo e in enemigos)
                    e.Mover(gameTime);

                //HACER FUNCIONAMIENTO DE LAS BOMBAS
                for (int i = 0; i < bombas.Count; i++)
                {
                    if ((int)(bombas[i].Contador += gameTime.ElapsedGameTime.TotalSeconds) == 2 && !bombas[i].HaExplotado())
                    {
                        bombas[i].Explotar(paredes, muros);
                        foreach (Explosion e in bombas[i].GetExplosion())
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

                    if (bombas[i].GetExplosion().Count > 0)
                    {
                        foreach (Explosion e in bombas[i].GetExplosion())
                        {
                            //Colision de las bombas con los enemigos para matarlos
                            for (int j = 0; j < enemigos.Count; j++)
                            {
                                if (new Rectangle(enemigos[j].X, enemigos[j].Y, 40, 40).Intersects(
                                    new Rectangle(e.X, e.Y, 40, 40)))
                                {
                                    puntuacion += enemigos[j].GetType() == typeof(EnemigoFinal) ?
                                        300 : 100;
                                    enemigos.RemoveAt(j);
                                }
                            }

                            if (new Rectangle(jugador.X, jugador.Y, 30, 30).Intersects(
                                    new Rectangle(e.X, e.Y, 40, 40)))
                                Exit();
                        }
                    }

                    if (bombas[i].Contador >= 4)
                        bombas.RemoveAt(i);
                }

                //Se comprueban colisiones de los enemigos
                foreach (Enemigo e in enemigos)
                {
                    if (colisiona(e.X, e.Y, 40, true))
                    {
                        e.X -= (int)(e.GetVelocidadX() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        e.Y -= (int)(e.GetVelocidadY() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        e.CambiarDireccion();
                    }

                    //Aquí se comprueba si pasa por al lado de un hueco y hay una probabilidad de que cambie de direccion
                    //Para que no entren en bucles
                    if (e.GetType() != typeof(Enemigo3))
                    {
                        if ((e.Y % 80 == 0 && e.GetVelocidadX() == 0 ||
                        e.X % 40 == 0 && e.X % 80 != 0 && e.GetVelocidadY() == 0)
                        && r.Next(0, 3) == 2)
                            e.CambiarDireccion();
                    }

                    //Si el enemigo final te toca mueres
                    if (new Rectangle(e.X + 10, e.Y + 10, 30, 30).Intersects(
                        new Rectangle(jugador.X, jugador.Y, 30, 30)))
                    {
                        Thread.Sleep(1500);
                        Exit();
                    }

                }

                //Se comprueban colisiones con las paredes
                if (colisiona(jugador.X, jugador.Y, 30))
                {
                    if (teclado.IsKeyDown(Keys.W))
                        jugador.Y += (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (teclado.IsKeyDown(Keys.A))
                        jugador.X += (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (teclado.IsKeyDown(Keys.S))
                        jugador.Y -= (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (teclado.IsKeyDown(Keys.D))
                        jugador.X -= (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                //Calcular segundos
                if (tiempo > 0)
                    tiempo -= gameTime.ElapsedGameTime.TotalSeconds;
                else if (!final)
                    enemigosFinales();

                //Salida de la puerta
                if (new Rectangle(salida.X + 10, salida.Y + 10, 20, 20).Intersects(
                    new Rectangle(jugador.X, jugador.Y, 30, 30)) && enemigos.Count == 0)
                {
                    puntuacion += 10000;
                    muerto = false;
                    Exit();
                }
                //Colision con la mejora
                if (mejora != null && new Rectangle(mejora.X + 10, mejora.Y + 10, 20, 20).Intersects(
                    new Rectangle(jugador.X, jugador.Y, 30, 30)))
                {
                    mejora = null;
                    puntuacion += 1000;
                    jugador.LongitudBomba += 1;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if(!pausado)
                GraphicsDevice.Clear(Color.Green);
            else
                GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (!pausado)
            {
                spriteBatch.Draw(fondoGris, new Rectangle(0, 0, 920, 40), Color.White);
                //Dibujar bombas
                foreach (Bomba b in bombas)
                {
                    b.Dibujar(spriteBatch);
                    foreach (Explosion e in b.GetExplosion())
                        e.Dibujar(spriteBatch);
                }
                //Dibujamos la puerta y la mejora si la hubiera
                salida.Dibujar(spriteBatch);
                if (mejora != null)
                    mejora.Dibujar(spriteBatch);
                //se dibuja el jugador
                jugador.Dibujar(spriteBatch);
                //Dibujar los muros
                foreach (Obstaculo m in muros)
                    m.Dibujar(spriteBatch);
                //Dibujar las paredes
                foreach (Obstaculo p in paredes)
                    p.Dibujar(spriteBatch);
                //Dibujamos a los enemigos
                foreach (Enemigo e in enemigos)
                    e.Dibujar(spriteBatch);
                spriteBatch.DrawString(texto, "TIEMPO " + (int)tiempo, new Vector2(9, 9), Color.Black);
                spriteBatch.DrawString(texto, "TIEMPO " + (int)tiempo, new Vector2(5, 5), Color.White);
                spriteBatch.DrawString(texto, "PUNTUACION: " + puntuacion, new Vector2(254, 9), Color.Black);
                spriteBatch.DrawString(texto, "PUNTUACION: " + puntuacion, new Vector2(250, 5), Color.White);
                spriteBatch.DrawString(texto, "NIVEL " + nivel, new Vector2(754, 9), Color.Black);
                spriteBatch.DrawString(texto, "NIVEL " + nivel, new Vector2(750, 5), Color.White);
            }else
            {
                spriteBatch.DrawString(texto, "PAUSADO", new Vector2(920 / 2 - 50, 560 / 2 - 30), Color.White);
                spriteBatch.DrawString(texto, "Pulsa Esc para salir", new Vector2(920 / 2 - 100, 560 / 2), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
