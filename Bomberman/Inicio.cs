﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public class Inicio : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D imagen;
        SpriteFont texto;
        Vector2 posicionImagen;
        int opcion;


        public Inicio()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            graphics.PreferredBackBufferWidth = 920;
            graphics.PreferredBackBufferHeight = 560;
            graphics.ApplyChanges();
            opcion = 0;
        }

        public int GetOpcion()
        {
            return opcion;
        }

        protected override void Initialize()
        {
            posicionImagen = new Vector2(320, 50);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            imagen = Content.Load<Texture2D>("bomberman");
            texto = Content.Load<SpriteFont>("texto");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState tecla = Keyboard.GetState();

            if (tecla.IsKeyDown(Keys.Escape))
            {
                opcion = -1;
                Exit();
            }

            if (tecla.IsKeyDown(Keys.Enter))
            {
                opcion = 1;
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(imagen, new Rectangle((int) posicionImagen.X, (int) posicionImagen.Y, imagen.Width, imagen.Height), Color.White);
            spriteBatch.DrawString(texto, "Enter: 1 Jugador", new Vector2(350, 250), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
