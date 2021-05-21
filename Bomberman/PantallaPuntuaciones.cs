using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Bomberman
{
    class PantallaPuntuaciones :Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        SpriteFont texto;
        Dictionary<string, Usuario> puntuaciones;
        int contLineas;

        public PantallaPuntuaciones()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 920;
            graphics.PreferredBackBufferHeight = 560;
            graphics.ApplyChanges();
        }

        private void guardarPuntuaciones()
        {
            try
            {
                StreamWriter fichero = new StreamWriter("puntuaciones.txt");
                foreach (var u in puntuaciones)
                    fichero.WriteLine(u.Value.ToString());

                fichero.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("No se ha encontrado el archivo " + e.FileName);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected override void Initialize()
        {
            puntuaciones = new Dictionary<string, Usuario>();
            try
            {
                StreamReader fichero = new StreamReader("puntuaciones.txt");
                string linea;
                contLineas = 0;
                string[] p, fecha;

                while((linea = fichero.ReadLine()) != null)
                {
                    p = linea.Split(";");
                    fecha = p[2].Split(" ")[0].Split("/");
                    if (puntuaciones.ContainsKey(p[0]))
                        puntuaciones.Remove(p[0]);
                    
                    puntuaciones.Add(p[0], new Usuario(p[0], Convert.ToInt32(p[1]),
                    new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]))));

                    contLineas++;
                }

                fichero.Close();
            }catch(FileNotFoundException e)
            {
                Console.WriteLine("No se ha encontrado el archivo " + e.FileName);
            }catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texto = Content.Load<SpriteFont>("texto");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (contLineas != puntuaciones.Count)
                    guardarPuntuaciones();
                Exit();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.DrawString(texto, "TABLA DE PUNTUACIONES", new Vector2(280, 10), Color.White);
            int i = 1;
            foreach (KeyValuePair<string, Usuario> p in puntuaciones.OrderBy(key => key.Value))
            {
                if(i <= 10)
                {
                    spriteBatch.DrawString(texto, i + ". " + p.Key + " - " + p.Value.GetPuntuacion() + " pts. - " + p.Value.GetFecha().ToString("d"), new Vector2(230, 10 + (i * 30)), Color.White);
                }
                i++;
            }

            for(i = i; i <= 10; i++)
            {
                spriteBatch.DrawString(texto, i + ". XXX - 0 pts. - XX", new Vector2(230, 10 + (i * 30)), Color.White);
            }
            spriteBatch.DrawString(texto, "Pulsa Enter para salir", new Vector2(280, 10 + (11 * 30)), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
