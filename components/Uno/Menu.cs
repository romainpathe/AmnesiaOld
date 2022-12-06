using System;
using Amnesia.classes.Uno;

namespace Amnesia.components.Uno
{
    public class Menu
    {

        public static GameManager GameManager;
        private static bool _addPlayer = true;

        /// <summary>
        /// Display error message in console
        /// </summary>
        /// <param name="text">Error message</param>
        private static void Error(string text)
        {
            var x = Console.WindowWidth / 2 - text.Length / 2;
            Console.SetCursorPosition(x, 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(text);
        }

        /// <summary>
        /// Initialize the menu
        /// </summary>
        public static void Init()
        {
            while (true)
            {
                Console.CursorVisible = false;
                var text = "Welcome to Uno Game!";
                var x = Console.WindowWidth / 2 - text.Length / 2;
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(x, 0);
                Console.Write(text);

                text = "Add Player";
                x = Console.WindowWidth - (Console.WindowWidth / 4) - text.Length / 2;
                Console.SetCursorPosition(x, 8);
                if (_addPlayer)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.Write(text);

                text = "Start Game";
                x = Console.WindowWidth - (Console.WindowWidth / 4) - text.Length / 2;
                Console.SetCursorPosition(x, 10);
                if (!_addPlayer)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.Write(text);

                text = "List of Players: ";
                x = Console.WindowWidth / 4 - text.Length;
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(x, 5);
                Console.Write(text);

                var count = GameManager.PlayerManager.Players.Count;
                foreach (var player in GameManager.PlayerManager.Players)
                {
                    Console.SetCursorPosition(x, 6 + count);
                    Console.Write("- " + player.Name);
                    count--;
                }

                var key = Console.ReadKey().Key;
                Console.Clear();
                switch (key)
                {
                    case ConsoleKey.Enter:
                        if (_addPlayer)
                        {
                            Console.Clear();
                            AddPlayer();
                        }
                        else
                        {
                            if (GameManager.PlayerManager.Players.Count > 1)
                            {
                                GameManager.StartGame();
                            }
                            else
                            {
                                Error("You need at least 2 players to start the game!");
                                continue;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (_addPlayer)
                        {
                            _addPlayer = false;
                            continue;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (!_addPlayer)
                        {
                            _addPlayer = true;
                            continue;
                        }
                        break;
                    default:
                        continue;
                }
                break;
            }
        }

        /// <summary>
        /// Menu to add a player
        /// </summary>
        private static void AddPlayer()
        {
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Enter the name of the player:");
            var name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name) && name.Length < 20)
            {
                if (!GameManager.PlayerManager.PlayerNameExists(name))
                {
                    GameManager.PlayerManager.AddPlayer(new Player(name));   
                }
                else
                {
                    Console.Clear();
                    Error("Player name already exists!");
                    AddPlayer();
                }
            }
            else
            {
                Console.Clear();
                Error("The name must be between 1 and 20 characters!");
                AddPlayer();
            }
            Console.Clear();
            Init();
        }

    }
}