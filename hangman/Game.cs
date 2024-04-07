using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static hangman.ConsoleProps;
using static hangman.WordHandler;

namespace hangman
{
    internal class Game
    {
        private bool isWon;
        private string wordToFind;
        private string badGuesses;
        private int triesLeft;
        private char[] wordLetters;
        private char[] foundLetters;
        private readonly string PATH_OF_BUILT_IN_WORDS = Directory.GetCurrentDirectory() + @"\Words\built_in_words.txt";
        private readonly string PATH_OF_USER_WORDS = Directory.GetCurrentDirectory() + @"\Words\user_words.txt";
        private readonly string exceptions = "0123456789~!@#$%^&*()-_=+[]\\{}|;':\",./<>?";

        public Game()
        {
            this.isWon = false;
            this.wordToFind = "";
            this.badGuesses = "";
            this.triesLeft = 10;
        }

        private static void Welcome()
        {
            DisplayInRed(CreateDivider('-', 33));
            Console.WriteLine("Üdvözöllek az akasztófa játékban!");
            DisplayInGreen(CreateDivider('-', 33));
        }

        private void ChooseGameMode()
        {
            DisplayLines(1);
            Console.WriteLine($"Megadsz egy szót, vagy válasszon a program?");
            Console.WriteLine("1 - Én választok");          // Multi game   - word provided by the user
            Console.WriteLine("2 - A program válasszon");   // Solo game    - word provided by software 
            DisplayLines(1);

            bool inputIsValid = false;

            // TODO try catch for empty input Exception
            do {
                int mode;

                try
                {
                    mode = Convert.ToInt32(Console.ReadLine());

                    if (mode == 1)
                    {
                        inputIsValid = true;
                        SetupMulti();
                    }
                    else if (mode == 2)
                    {
                        inputIsValid = true;
                        SetupSolo();
                    }
                    else
                    {
                        throw new FormatException();
                    }
                } catch (FormatException)
                {
                    DisplayInRed("Hibás bemenet!");
                    continue;
                }

            } while (inputIsValid == false);
        }

        private void SetupSolo()
        {
            this.wordToFind = ChooseWordFromFile(PATH_OF_BUILT_IN_WORDS);
            Console.Clear();

            this.foundLetters = SwapStringToHiddenCharArray(this.wordToFind);
            this.wordLetters = CreateCharArrayFromString(this.wordToFind);
        }

        private void SetupMulti()
        {
            Console.Clear();

            bool inputIsValid = false;
            string input = "";

            do
            {
                DisplayLines(1);
                Console.WriteLine("Add meg a megfejtendő szót!");
                DisplayLines(1);

                try
                {
                    input = Console.ReadLine().ToLower();
                    
                    for (int i = 0; i < input.Length; i++)
                    {
                        CheckInputExceptions(exceptions[i]);
                    }

                    inputIsValid = true;
                }
                catch (FormatException)
                {
                    Console.Clear();
                    DisplayInRed("Hibás bemenet!");
                    continue;
                }

            } while (inputIsValid == false);

            this.wordToFind = input;
            AddWordToFile(PATH_OF_USER_WORDS, this.wordToFind);
            Console.Clear();

            this.foundLetters = SwapStringToHiddenCharArray(this.wordToFind);
            this.wordLetters = CreateCharArrayFromString(this.wordToFind);
        }

        private void ShowFoundLetters()
        {
            ListArray(this.foundLetters);
        }

        private void GetTriesLeft()
        {
            Console.Write($"        Ennyiszer próbálkozhatsz: ");
            DisplayInRed(Convert.ToString(this.triesLeft));
        }

        private void CheckIfPlayerLost()
        {
            if (this.triesLeft == 0)
            {
                this.End();
            }
        }

        private void HandleBadGuess(bool hasFoundLetter, char letter)
        {
            if (hasFoundLetter == false && this.badGuesses.Contains(letter) == true)
            {
                string convertedLetter = Convert.ToString(letter) + ", ";
                this.badGuesses += convertedLetter;
                this.triesLeft--;
            }
        }

        private void CheckOutcome()
        {
            if (Array.IndexOf(this.foundLetters, '_') == -1)
            {
                this.isWon = true;
            }
            else if (this.triesLeft != 0)
            {
                DisplayInRed("Eredmény ellenőrzési hiba!");
            }

            End();
        }

        // Finish exception handling 
        private void CheckInputExceptions(char letter)
        {
            char[] exceptions = this.exceptions.ToCharArray();

            if (Array.IndexOf(exceptions, letter) != -1)
            {
                throw new FormatException();
            }
        }

        private void End()
        {
            Console.Clear();
            if (this.isWon == true)
            {
                DisplayInGreen("Nyertél!");
                DisplayInRed(" <3");
            }
            else
            {
                DisplayInRed("GAME OVER (Stickman-t felkötötték)");
            }

            Console.ResetColor();
            DisplayLines(3);
            Console.Write(" - A megfejtés: " + this.wordToFind);
            DisplayLines(1);
        }

        // TODO - Edit, so try catch requests input until it's not valid (like multiple characters entered)
        private void MakeGuess()
        {
            do
            {
                ShowFoundLetters();
                DisplayLines(1);
                DisplayInRed($"{Environment.NewLine}Nincs benne: {this.badGuesses}");
                Console.WriteLine($"{Environment.NewLine}Adj meg egy betűt!");
                DisplayLines(1);
                char letter;                                                      // needs a more elegant solution
                
                try
                {
                    letter = Convert.ToChar(Console.ReadLine());
                    CheckInputExceptions(letter);
                }
                catch (FormatException)
                {
                    Console.Clear();
                    DisplayLines(1);
                    DisplayInRed("Hibás bemenet!");
                    DisplayLines(2);
                    continue;
                }

                bool hasFoundLetter = CopyIfCharIsFound(this.wordLetters, this.foundLetters, letter);

                this.HandleBadGuess(hasFoundLetter, letter);

                // kiíratjuk a szó állapotát
                Console.Clear();
                ListArray(this.foundLetters);
                GetTriesLeft();

                this.CheckIfPlayerLost();
            } while (Array.IndexOf(this.foundLetters, '_') > -1 && this.triesLeft > 0);
        }

        public void GameStart()
        {
            Welcome();
            this.ChooseGameMode();
            this.MakeGuess();
            this.CheckOutcome();
        }
    }
}
