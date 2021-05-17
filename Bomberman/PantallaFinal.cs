using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Bomberman
{
    class PantallaFinal : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        SpriteFont texto;
        bool muerto;
        int puntuacion;
        string mensaje;

        public PantallaFinal(bool muerto, int puntuacion)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            graphics.PreferredBackBufferWidth = 920;
            graphics.PreferredBackBufferHeight = 560;
            graphics.ApplyChanges();

            this.muerto = muerto;
            this.puntuacion = puntuacion;
        }

        protected override void Initialize()
        {
            if (muerto)
                mensaje = "Has muerto... :(  Otra vez sera...";
            else
                mensaje = "Enhorabuena!! Has conseguido escapar!!!";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texto = Content.Load<SpriteFont>("texto");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.DrawString(texto, mensaje, new Vector2(200, 250), Color.White);
            spriteBatch.DrawString(texto, "Tu puntuacion ha sido de " + puntuacion + " puntos", new Vector2(200, 280), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
