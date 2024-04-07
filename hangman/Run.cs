using System.Diagnostics.Tracing;
using static hangman.ConsoleProps;
using static hangman.WordHandler;
using static hangman.Game;

namespace hangman
{
    internal class Runner
    {
        static void Main(string[] args)
        {
            Game g1 = new(); // welcome run
            g1.GameStart();
        }
    }
}
