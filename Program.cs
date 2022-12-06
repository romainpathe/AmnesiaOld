using System;
using Amnesia.classes.Uno;
using Amnesia.components.Uno;

namespace Amnesia
{
    internal static class Program
    {
        public static readonly Random Random = new Random();
        
        /// <summary>
        /// Initializes the program.
        /// </summary>
        public static void Main()
        {
            
            Writer.Init();
            Console.CursorVisible = false;
            
            var gameManager = new GameManager();
            Menu.GameManager = gameManager;
            Menu.Init();
        }
    }
}