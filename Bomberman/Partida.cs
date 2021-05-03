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
        Texture2D texto;

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
            jugador = new Jugador(80, 80);

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
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //se carga la imagen del jugador
            jugador.SetImagen(Content.Load<Texture2D>("sprite"));
            //se cargan las imagenes de las paredes
            foreach (Obstaculo p in paredes)
                p.SetImagen(Content.Load<Texture2D>("muroX"));
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var teclado = Keyboard.GetState();

            if (teclado.IsKeyDown(Keys.W))
                jugador.Y -= jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (teclado.IsKeyDown(Keys.A))
                jugador.X -= jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (teclado.IsKeyDown(Keys.S))
                jugador.Y += jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (teclado.IsKeyDown(Keys.D))
                jugador.X += jugador.GetVelocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Se comprueban colisiones con las paredes
            foreach(Obstaculo p in paredes)
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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();
            jugador.Dibujar(spriteBatch);
            foreach (Obstaculo p in paredes)
                p.Dibujar(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
