using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    class PedirNombre : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        SpriteFont texto;
        string nombre;
        char teclaP;
        bool pulsada;


        public PedirNombre()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 920;
            graphics.PreferredBackBufferHeight = 560;
            graphics.ApplyChanges();
        }

        public string GetNombre()
        {
            return nombre;
        }

        protected override void Initialize()
        {
            pulsada = false;
            nombre = "";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texto = Content.Load<SpriteFont>("texto");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState tecla = Keyboard.GetState();

            if (tecla.IsKeyDown(Keys.Enter))
                Exit();

            if (tecla.IsKeyDown(Keys.Back) && !pulsada)
                if(nombre.Length > 0)
                    nombre = nombre.Remove(nombre.Length - 1);
            pulsada = tecla.IsKeyDown(Keys.Back) ? true : false;

            if ((tecla.GetPressedKeys().Length > 0) && (tecla.GetPressedKeys()[0] >= Keys.A && tecla.GetPressedKeys()[0] <= Keys.Z
                || tecla.GetPressedKeys()[0] == Keys.Space || tecla.GetPressedKeys()[0] >= Keys.D0 && tecla.GetPressedKeys()[0] <= Keys.D9))
            {
                if(nombre.Length < 10)
                {
                    if (!tecla.IsKeyDown((Keys)teclaP))
                        nombre += Convert.ToChar(tecla.GetPressedKeys()[0]).ToString();

                    teclaP = Convert.ToChar(tecla.GetPressedKeys()[0]);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.DrawString(texto, "Introduce tu nombre:" + nombre, new Vector2(250, 250), Color.White);
            spriteBatch.DrawString(texto, "Pulsa Enter para confirmar", new Vector2(270, 280), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
