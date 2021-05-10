using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    class Partida : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        SpriteFont texto;
        double tiempo;
        int longitudBomba;
        static Random r;

        string[] mapa = {
            "XXXXXXXXXXXXX",
            "X           X",
            "X X X X X X X",
            "X           X",
            "X X X X X X X",
            "X           X",
            "X X X X X X X",
            "X           X",
            "X X X X X X X",
            "X           X",
            "X X X X X X X",
            "X           X",
            "X X X X X X X",
            "X           X",
            "X X X X X X X",
            "X           X",
            "X X X X X X X",
            "X           X",
            "X X X X X X X",
            "X           X",
            "X X X X X X X",
            "X           X",
            "XXXXXXXXXXXXX"};
        Jugador jugador;
        List<Obstaculo> paredes;
        List<Obstaculo> muros;
        List<Bomba> bombas;
        List<Enemigo> enemigos;

        public Partida()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 920;
            graphics.PreferredBackBufferHeight = 560;
            graphics.ApplyChanges();
        }

        private void generarMapa()
        {
            //Se generan paredes
            paredes = new List<Obstaculo>();
            for (int i = 0; i < mapa.Length; i++)
            {
                for (int j = 0; j < mapa[i].Length; j++)
                {
                    if (mapa[i][j] == 'X')
                        paredes.Add(new Obstaculo(i * 40, (j + 1) * 40));
                }
            }
            //se generan muros
            muros = new List<Obstaculo>();
            int numMuros = 0, x, y;
            r = new Random();
            bool added;
            while (numMuros < 100)
            {
                x = r.Next(1, 22) * 40;
                y = r.Next(2, 13) * 40;
                added = false;

                if (x > 160 || y > 160)
                {
                    for (int i = 0; i < paredes.Count && !added; i++)
                    {
                        if (!new Rectangle(paredes[i].X, paredes[i].Y, 40, 40).Intersects(
                            new Rectangle(x, y, 40, 40)))
                        {
                            muros.Add(new Obstaculo(x, y));
                            numMuros++;
                            added = true;
                        }
                    }
                }
            }
        }
        protected override void Initialize()
        {
            //Inicializamos el jugador
            jugador = new Jugador(40, 80);

            //se genera el mapa
            generarMapa();

            //Inicializamos bombas
            bombas = new List<Bomba>();
            longitudBomba = 1;

            //Se inicializa el contador
            tiempo = 201;
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

            texto = Content.Load<SpriteFont>("texto");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var teclado = Keyboard.GetState();

            //Colocar bomba
            Bomba bAux;
            if (teclado.IsKeyDown(Keys.E))
            {
                bAux = new Bomba(jugador.X - (jugador.X % 40), jugador.Y - (jugador.Y % 40), longitudBomba);
                //He cambiado el .Equals() para que compare solo las posiciones X e Y
                if (!bombas.Contains(bAux))
                {
                    bAux.SetImagen(Content.Load<Texture2D>("bomba"));
                    bombas.Add(bAux);
                }
            }

            //Movimiento jugador
            if (teclado.IsKeyDown(Keys.W))
                jugador.Y -= (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (teclado.IsKeyDown(Keys.A))
                jugador.X -= (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (teclado.IsKeyDown(Keys.S))
                jugador.Y += (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (teclado.IsKeyDown(Keys.D))
                jugador.X += (int)(jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds);

            
            //HACER FUNCIONAMIENTO DE LAS BOMBAS
            for(int i = 0; i < bombas.Count; i++)
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

                if(bombas[i].Contador >= 4)
                    bombas.RemoveAt(i);
            }

            //Se comprueban colisiones con las paredes
            foreach (Obstaculo p in paredes)
            {
                if (new Rectangle(p.X, p.Y, 40, 40).Intersects(
                    new Rectangle(jugador.X, jugador.Y, 30, 30)))
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
            }

            foreach (Obstaculo m in muros)
            {
                if (new Rectangle(m.X, m.Y, 40, 40).Intersects(
                    new Rectangle(jugador.X, jugador.Y, 30, 30)))
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
            }

            //Calcular segundos
            if (tiempo > 0)
                tiempo -= gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();
            //Dibujar bombas
            foreach (Bomba b in bombas)
            {
                b.Dibujar(spriteBatch);
                foreach (Explosion e in b.GetExplosion())
                    e.Dibujar(spriteBatch);
            }
            //se dibuja el jugador
            jugador.Dibujar(spriteBatch);
            //Dibujar los muros
            foreach (Obstaculo m in muros)
                m.Dibujar(spriteBatch);
            //Dibujar las paredes
            foreach (Obstaculo p in paredes)
                p.Dibujar(spriteBatch);
            spriteBatch.DrawString(texto, "TIEMPO " + bombas.Count, new Vector2(9, 9), Color.Black);
            spriteBatch.DrawString(texto, "TIEMPO " + bombas.Count, new Vector2(5, 5), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
