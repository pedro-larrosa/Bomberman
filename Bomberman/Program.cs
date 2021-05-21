using System;
using System.Threading;
using System.IO;

namespace Bomberman
{
    public static class Program
    {
        [STAThread]
        private static void guardarPuntuacion(Usuario u)
        {
            try
            {
                StreamWriter fichero = File.AppendText("puntuaciones.txt");
                fichero.WriteLine(u.GetNombre() + ";" + u.GetPuntuacion() + ";" + u.GetFecha());
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

        static void Main(string[] args)
        {
            Console.Write("fewrwe");
            Inicio inicio;
            int o = 0;

            while(o != -1)
            {
                inicio = new Inicio();
                inicio.Run();
                o = inicio.GetOpcion();

                switch (o)
                {
                    case 1:
                        Partida partida;
                        int i = 1, l = 1, p = 0;
                        PedirNombre pantallaNombre = new PedirNombre();
                        pantallaNombre.Run();
                        string nombre = pantallaNombre.GetNombre();
                        new Tutorial(false).Run();

                        do
                        {
                            partida = new Partida(i++, l, p);
                            partida.Run();
                            l = partida.GetLongitudBomba();
                            p = partida.GetPuntuacion();
                        } while (i <= 5 && !partida.JugadorMuerto());
                        if(!partida.GetPausado())
                        {
                            new PantallaFinal(partida.JugadorMuerto(), p).Run();
                            guardarPuntuacion(new Usuario(nombre, p, DateTime.Now));
                        }
                        break;
                    case 2:
                        new Tutorial(true).Run();
                        new PartidaMultijugador().Run();
                        break;
                    case 3:
                        new PantallaPuntuaciones().Run();
                        break;
                }

                
            }
        }
    }
}
