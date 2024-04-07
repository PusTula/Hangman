using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hangman
{
    internal class WordHandler
    {
        public static void ListArray(char[] guessList)
        {
            foreach (char guess in guessList)
            {
                Console.Write($"{guess} ");
            }
        }

        public static bool CopyIfCharIsFound(char[] original, char[] destination, char letter)
        {
            bool hasFoundLetter = false;

            // ellenőrizzük, hogy a megadott betű szerepel-e a szóban
            // ha igen, helyezzük át a guesses tömb-be
            for (int i = 0; i < original.Length; i++)
            {
                if (letter == original[i])
                {
                    destination[i] = original[i];
                    hasFoundLetter = true;
                }
            }

            return hasFoundLetter;
        }

        // TODO - Extend so chooses from User generated words too
        // IMPROVEMENT - Set word length according to difficulty level
        public static string ChooseWordFromFile(string path)
        {
            Random rnd = new();
            int wordIndex = rnd.Next(1, File.ReadLines(path).Count());  // A fájl sorainak száma
            StreamReader sr = new(path);

            string line;

            for (int i = 0; i <= wordIndex; i++)
            {
                line = sr.ReadLine();

                if (i == wordIndex)
                {
                    sr.Close();
                    return line.ToLower();
                }
            }

            sr.Close();
            return "FAIL";
        }

        // TODO - return type not clarified yet
        public static void AddWordToFile(string path, string word)
        {
            StreamWriter sw = new(path, true);
            sw.WriteLine(word);
            sw.Close();
        }

        public static char[] SwapStringToHiddenCharArray(string word)
        {
            char[] charArray = new char[word.Length];

            for (int i = 0; i < charArray.Length; i++)
            {
                if (charArray[i] != ' ') charArray[i] = '_';
            }

            return charArray;
        }

        public static char[] CreateCharArrayFromString(string word)
        {
            return word.ToCharArray();
        }
    }
}
