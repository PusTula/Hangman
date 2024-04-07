using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace hangman
{
    internal class ConsoleProps
    {
        public static void DisplayInGreen(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void DisplayInRed(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static string CreateDivider(char dividerMark, int length)
        {
            string divider = "";

            for (int i = 0; i < length; i++)
            {
                divider += dividerMark;
            }

            return divider;
        }

        public static void DisplayLines(int numberOfLines)
        {
            for (int i = 0; i < numberOfLines; i++)
            {
                Console.WriteLine();
            }
        }
    }
}
