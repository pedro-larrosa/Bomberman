using System;

namespace Bomberman
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Partida(4))
                game.Run();
        }
    }
}
