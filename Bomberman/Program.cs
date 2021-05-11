using System;

namespace Bomberman
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Partida(3))
                game.Run();
        }
    }
}
