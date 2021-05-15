using System;
using System.Threading;

namespace Bomberman
{
    public static class Program
    {
        [STAThread]
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
                        int i = 1, l = 1, p = 0;

                        do
                        {
                            partida = new Partida(i++, l, p);
                            partida.Run();
                            l = partida.GetLongitudBomba();
                            p = partida.GetPuntuacion();
                        } while (i <= 5 && !partida.JugadorMuerto());
                        break;
                }

                
            }
        }
    }
}
