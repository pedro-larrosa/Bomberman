using System;

namespace Bomberman
{
    public static class Program
    {
        
        static void Main()
        {
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
                        int i = 1;
                        Console.WriteLine("Introduce tu nombre:");
                        string nombre = Console.ReadLine();
                        do
                        {
                            partida = new Partida(i++);
                            partida.Run();
                        } while (i <= 5 && !partida.JugadorMuerto());
                        break;
                }

                
            }
        }
    }
}
