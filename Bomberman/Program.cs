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
                        int i = 1;
                        int l = 1;
                        do
                        {
                            partida = new Partida(i++, l);
                            partida.Run();
                            l = partida.GetLongitudBomba();
                        } while (i <= 5 && !partida.JugadorMuerto());
                        break;
                }

                
            }
        }
    }
}
