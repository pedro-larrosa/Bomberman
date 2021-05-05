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

        public Partida()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 920;
            graphics.PreferredBackBufferHeight = 560;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //Inicializamos el jugador
            jugador = new Jugador(40, 80);

            //Se generan las paredes
            paredes = new List<Obstaculo>();
            for (int i = 0; i < mapa.Length; i++)
            {
                for(int j = 0; j < mapa[i].Length; j++)
                {
                    if (mapa[i][j] == 'X')
                        paredes.Add(new Obstaculo(i * 40, (j + 1) * 40));
                }
            }

            //Generar muros
            muros = new List<Obstaculo>();
            int numMuros = 0, x, y;
            r = new Random();
            bool added;
            while (numMuros < 100)
            {
                x = r.Next(1, 22) * 40;
                y = r.Next(2, 13) * 40;
                added = false;

                if(x > 160 || y > 160)
                {
                    for (int i = 0; i < paredes.Count && !added; i++)
                    {
                        if (!new Rectangle((int)paredes[i].X, (int)paredes[i].Y, 40, 40).Intersects(
                            new Rectangle(x, y, 40, 40)))
                        {
                            muros.Add(new Obstaculo(x, y));
                            numMuros++;
                            added = true;
                        }
                    }
                }
            }

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

            //Movimiento jugador
            if (teclado.IsKeyDown(Keys.W))
                jugador.Y -= jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (teclado.IsKeyDown(Keys.A))
                jugador.X -= jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (teclado.IsKeyDown(Keys.S))
                jugador.Y += jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (teclado.IsKeyDown(Keys.D))
                jugador.X += jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Bomba bAux;
            //HACER FUNCIONAMIENTO DE LAS BOMBAS
            if (teclado.IsKeyDown(Keys.E))
            {
                bAux = new Bomba((int)(jugador.X - (jugador.X % 40)),(int)(jugador.Y - (jugador.Y % 40)), longitudBomba);
                bAux.SetImagen(Content.Load<Texture2D>("bomba"));
                bombas.Add(bAux);
            }

            for(int i = 0; i < bombas.Count; i++)
            {
                if ((bombas[i].Contador += gameTime.ElapsedGameTime.TotalSeconds) >= 2)
                {
                    bombas[i].Explotar(Content, paredes);
                    foreach (Explosion e in bombas[i].GetExplosion())
                        e.SetImagen(Content.Load<Texture2D>("centroBomba"));
                }

                if(bombas[i].Contador >= 4)
                    bombas.RemoveAt(i);
            }

            //Se comprueban colisiones con las paredes
            foreach (Obstaculo p in paredes)
            {
                if (new Rectangle((int)p.X, (int)p.Y, 40, 40).Intersects(
                    new Rectangle((int)jugador.X, (int)jugador.Y, 40, 40)))
                {
                    if (teclado.IsKeyDown(Keys.W))
                        jugador.Y += jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (teclado.IsKeyDown(Keys.A))
                        jugador.X += jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (teclado.IsKeyDown(Keys.S))
                        jugador.Y -= jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (teclado.IsKeyDown(Keys.D))
                        jugador.X -= jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            foreach (Obstaculo p in muros)
            {
                if (new Rectangle((int)p.X, (int)p.Y, 30, 30).Intersects(
                    new Rectangle((int)jugador.X, (int)jugador.Y, 40, 40)))
                {
                    if (teclado.IsKeyDown(Keys.W))
                        jugador.Y += jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (teclado.IsKeyDown(Keys.A))
                        jugador.X += jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (teclado.IsKeyDown(Keys.S))
                        jugador.Y -= jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (teclado.IsKeyDown(Keys.D))
                        jugador.X -= jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            spriteBatch.DrawString(texto, "TIEMPO " + (int)tiempo, new Vector2(9, 9), Color.Black);
            spriteBatch.DrawString(texto, "TIEMPO " + (int)tiempo, new Vector2(5, 5), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
