using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Bomberman
{
    class Tutorial : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        SpriteFont texto;
        Texture2D teclas, teclaE, teclaP, personaje, bomba, enemigo, puerta;
        double t;

        public Tutorial()
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
            t = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texto = Content.Load<SpriteFont>("texto");
            teclas = Content.Load<Texture2D>("teclas");
            teclaE = Content.Load<Texture2D>("teclaE");
            teclaP = Content.Load<Texture2D>("teclaP");
            personaje = Content.Load<Texture2D>("jugador1");
            bomba = Content.Load<Texture2D>("bomba2");
            enemigo = Content.Load<Texture2D>("enemigoFinal");
            puerta = Content.Load<Texture2D>("puerta");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                Exit();

            t += gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            //Movimiento
            spriteBatch.DrawString(texto, "MOVIMIENTO", new Vector2(150, 220), Color.White);
            spriteBatch.Draw(teclas, new Rectangle(150, 260, 150, 100), Color.White);
            spriteBatch.Draw(personaje, new Rectangle(190, 150, 50, 50), Color.White);
            //Pausa
            spriteBatch.DrawString(texto, "PAUSAR JUEGO", new Vector2(330, 120), Color.White);
            spriteBatch.Draw(teclaP, new Rectangle(350, 160, 100, 60), Color.White);
            //Poner Bombas
            spriteBatch.DrawString(texto, "PONER BOMBAS", new Vector2(550, 220), Color.White);
            spriteBatch.Draw(teclaE, new Rectangle(550, 260, 150, 100), Color.White);
            spriteBatch.Draw(personaje, new Rectangle(590, 150, 50, 50), Color.White);
            if((int) t % 3 == 0)
                spriteBatch.Draw(bomba, new Rectangle(650, 150, 50, 50), Color.White);
            //aviso
            spriteBatch.DrawString(texto, "CUIDADO!! Cuando se acabe el tiempo", new Vector2(320, 350), Color.White);
            spriteBatch.DrawString(texto, "los enemigos te mataran", new Vector2(360, 380), Color.White);
            spriteBatch.Draw(enemigo, new Rectangle(450, 420, 70, 70), Color.White);
            //Puerta
            spriteBatch.DrawString(texto, "BUSCA LA SALIDA", new Vector2(120, 400), Color.White);
            spriteBatch.Draw(puerta, new Rectangle(150, 450, 70, 70), Color.White);


            spriteBatch.DrawString(texto, "Pulsa Enter para empezar", new Vector2(330, 520), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
